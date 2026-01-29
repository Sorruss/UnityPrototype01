using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace FG
{
    public class PlayerCamera : MonoBehaviour
    {
        [HideInInspector] public static PlayerCamera instance { get; private set; }
        [HideInInspector] public Camera cameraObject { get; private set; }
        [HideInInspector] public PlayerManager player { get; set; }

        [Header("Camera Follow Settings")]
        [SerializeField] private float cameraSpeedSmooth = 1.0f;
        [SerializeField] private float cameraZPosition = -2.5f;
        [SerializeField] private float cameraHeight = 1.5f;

        [Header("Camera Rotate Settings")]
        [SerializeField] private float minimumAngle = -30.0f;
        [SerializeField] private float maximumAngle = 60.0f;
        [SerializeField] private float leftAndRightSpeed = 20.0f;
        [SerializeField] private float upAndDownSpeed = 30.0f;

        [Header("Camera Collision Settings")]
        [SerializeField] private float cameraCollisionRadius = 0.2f;
        [SerializeField] private float cameraSmoothTime = 0.2f;
        [SerializeField] private LayerMask cameraCollisionLayers;

        [Header("Lock On Settings")]
        [SerializeField] private float lockOnCastRadius = 20.0f;
        [SerializeField] private float lockOnMinimumAngle = -50.0f;
        [SerializeField] private float lockOnMaximumAngle = 50.0f;
        [SerializeField] private float lockOnCameraHeight = 2.0f;
        [SerializeField] private float lockOnSmoothTime = 0.2f;
        [SerializeField] private float cameraHeightChangeSmoothTime = 0.15f;

        [HideInInspector] private List<CharacterManager> potentialTargets = new List<CharacterManager>();
        [HideInInspector] public CharacterManager closestTarget;
        [HideInInspector] public CharacterManager closestLeftTarget;
        [HideInInspector] public CharacterManager closestRightTarget;
        private Coroutine lockOnCoroutine;
        private Coroutine lockOnSwitchCoroutine;

        // VALUES FOR PROPER CAMERA BEHAVIOUR
        private Vector3 cameraVelocity = Vector3.zero;
        private float leftAndRightLookAngle;
        private float upAndDownLookAngle;

        private Transform pivotTransform;

        private float targetCameraZPosition;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                cameraObject = GetComponentInChildren<Camera>();
                pivotTransform = GameObject.FindGameObjectWithTag("CameraPivot").transform;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraObject.transform.localPosition = new Vector3(0.0f, 0.0f, cameraZPosition);
            pivotTransform.transform.localPosition = new Vector3(0.0f, cameraHeight, 0.0f);
        }

        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                HandleFollowTarget();
                HandleRotation();
                HandleCollisions();
            }
        }

        // CAMERA HANDLING METHODS
        private void HandleFollowTarget()
        {
            Vector3 cameraTargetPosition =
                Vector3.SmoothDamp(
                    transform.position,
                    player.transform.position,
                    ref cameraVelocity,
                    cameraSpeedSmooth);
            transform.position = cameraTargetPosition;
        }

        private void HandleRotation()
        {
            if (player.playerNetwork.networkIsLockedOn.Value)
            {
                // HORIZONTAL ROTATION (CAMERA OBJECT)
                Vector3 rotationDirection = 
                    player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.transform.position - 
                    transform.position;
                rotationDirection.Normalize();
                rotationDirection.y = 0.0f;
                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnSmoothTime);

                // VERTICAL ROTATION (CAMERA PIVOT)
                rotationDirection = 
                    player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.transform.position - 
                    pivotTransform.position;
                rotationDirection.Normalize();
                targetRotation = Quaternion.LookRotation(rotationDirection);
                pivotTransform.rotation = Quaternion.Slerp(pivotTransform.rotation, targetRotation, lockOnSmoothTime);

                // SAVE VALUES TO ANGLES TO REMOVE CAMERA SNAP AFTER UNLOCK
                leftAndRightLookAngle = transform.eulerAngles.y;
                upAndDownLookAngle = transform.eulerAngles.x;
            }
            else
            {
                leftAndRightLookAngle += PlayerInputManager.instance.cameraHorizontalMovement * leftAndRightSpeed * Time.deltaTime;
                upAndDownLookAngle -= PlayerInputManager.instance.cameraVerticalMovement * upAndDownSpeed * Time.deltaTime;
                upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumAngle, maximumAngle);

                // Left and Right rotation (Camera object).
                Vector3 targetRotation = Vector3.zero;
                targetRotation.y = leftAndRightLookAngle;
                Quaternion targetRotationQuaternion = Quaternion.Euler(targetRotation);
                transform.rotation = targetRotationQuaternion;

                // Up and Down rotation (Camera Pivot object).
                targetRotation = Vector3.zero;
                targetRotation.x = upAndDownLookAngle;
                targetRotationQuaternion = Quaternion.Euler(targetRotation);
                pivotTransform.localRotation = targetRotationQuaternion;
            }
        }
        
        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            Vector3 direction = cameraObject.transform.position - pivotTransform.position;
            direction.Normalize();

            RaycastHit hit;
            if (Physics.SphereCast(
                pivotTransform.position, 
                cameraCollisionRadius, 
                direction, 
                out hit, 
                Mathf.Abs(targetCameraZPosition), 
                cameraCollisionLayers))
            {
                float distance = Vector3.Distance(pivotTransform.position, hit.point);
                targetCameraZPosition = -(distance - cameraCollisionRadius);
            }

            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            Vector3 targetCameraPosition = new Vector3(0.0f, 0.0f, targetCameraZPosition);
            targetCameraPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, cameraSmoothTime);
            cameraObject.transform.localPosition = targetCameraPosition;
        }

        // LOCK ON METHODS
        public void TryToFindTargetsToLockOn()
        {
            ClearLockOnTargets();

            // VALUES TO UDENTIFY MOST FITTING TARGET
            float shortestDistanceToTarget = Mathf.Infinity;
            float shortestDistanceToRightTarget = Mathf.Infinity;
            float shortestDistanceToLeftTarget = -Mathf.Infinity;

            // GET ALL THE CHARACTERS IN THE LOCK ON SPHERE'S RADIUS
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnCastRadius, UtilityManager.instance.GetCharacterMasks());
            
            // SORT ALL FOUND COLLIDERS TO GET ALL POTENTIAL TARGETS
            foreach (Collider collider in colliders)
            {
                CharacterManager character = collider.GetComponent<CharacterManager>();

                // VALID CHECK
                if (character == null)
                    continue;

                // CHECK IF DEAD
                if (character.characterNetwork.networkIsDead.Value)
                    continue;

                // CHECK IF IT NOT OURSELVES
                if (character.transform.root == player.transform.root)
                    continue;

                // GET ANGLE TO TARGET AND CHECK
                Vector3 directionToTarget = character.transform.position - player.transform.position;
                float angleToTarget = Vector3.Angle(directionToTarget, cameraObject.transform.forward);
                if (angleToTarget < lockOnMinimumAngle || angleToTarget > lockOnMaximumAngle)
                    continue;

                // CHECK IF THERE ARE ANY OBSTACLES BETWEEN PLAYER AND TARGET
                RaycastHit hit;
                if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position,
                    character.characterCombatManager.lockOnTransform.position,
                    out hit,
                    UtilityManager.instance.GetEnvironmentMasks()))
                    continue;

                potentialTargets.Add(character);
            }

            // TRYNA FIND 3 CLOSEST TARGETS TO PLAYER
            foreach (var target in potentialTargets)
            {
                if (target == null)
                {
                    player.playerNetwork.networkIsLockedOn.Value = false;
                    ClearLockOnTargets();
                    continue;
                }

                // TRYNA FIND CLOSEST TARGET
                float distanceToTarget = Vector3.Distance(player.transform.position, target.transform.position);
                if (distanceToTarget < shortestDistanceToTarget)
                {
                    shortestDistanceToTarget = distanceToTarget;
                    closestTarget = target;
                }

                // TRYNA FIND LEFT&RIGHT CLOSEST TARGETS
                if (player.playerNetwork.networkIsLockedOn.Value)
                {
                    if (target == player.playerCombatManager.currentTarget)
                        continue;

                    // IF THIS DISTANCE IS NEGATIVE, THE TARGET IS ON THE LEFT FROM CAMERA
                    // IF POSITIVE - ON THE RIGHT FROM CAMERA
                    Vector3 relativeTargetPosition = transform.InverseTransformPoint(target.transform.position);
                    float relativeDistance = relativeTargetPosition.x;

                    if (relativeDistance >= 0.0f)       // TARGET ON THE RIGHT FROM CAMERA
                    {
                        if (relativeDistance < shortestDistanceToRightTarget)
                        {
                            shortestDistanceToRightTarget = relativeDistance;
                            closestRightTarget = target;
                        }
                    }
                    else if (relativeDistance < 0.0f)  // TARGET ON THE LEFT FROM CAMERA
                    {
                        if (relativeDistance > shortestDistanceToLeftTarget)
                        {
                            shortestDistanceToLeftTarget = relativeDistance;
                            closestLeftTarget = target;
                        }
                    }
                }
            }
        }

        public void ClearLockOnTargets()
        {
            closestTarget = null;
            closestLeftTarget = null;
            closestRightTarget = null;
            potentialTargets.Clear();
        }

        // ADJUST CAMERA METHOD + ITS ENUMERATOR
        public void AdjustCameraHeight()
        {
            if (lockOnCoroutine != null)
            {
                StopCoroutine(lockOnCoroutine);
            }
            lockOnCoroutine = StartCoroutine(AdjustCameraHeightEnumerator());
        }

        private IEnumerator AdjustCameraHeightEnumerator()
        {
            float duration = 1.0f;
            float timer = 0.0f;

            Vector3 velocity = Vector3.zero;
            Vector3 targetPositionLockedOn = new Vector3(pivotTransform.localPosition.x, lockOnCameraHeight);
            Vector3 targetPositionLockedOff = new Vector3(pivotTransform.localPosition.x, cameraHeight);

            while (timer < duration)
            {
                timer += Time.deltaTime;

                if (player.playerCombatManager.currentTarget != null)
                {
                    // BRING CAMERA UP
                    pivotTransform.localPosition = Vector3.SmoothDamp(
                        pivotTransform.localPosition, 
                        targetPositionLockedOn, 
                        ref velocity,
                        cameraHeightChangeSmoothTime);

                    // KEEP CAMERA LOOKING FORWARD
                    /*pivotTransform.localRotation = Quaternion.Slerp(
                        pivotTransform.localRotation,
                        Quaternion.identity,
                        lockOnSmoothTime);*/
                }
                else
                {
                    // BRING CAMERA DOWN
                    pivotTransform.localPosition = Vector3.SmoothDamp(
                        pivotTransform.localPosition,
                        targetPositionLockedOff,
                        ref velocity,
                        cameraHeightChangeSmoothTime);
                }

                yield return null;
            }

            // IF UNTIL THIS MOMENT THE CAMERA DIDN'T FULLY ADJUST, SET IT HERE
            if (player.playerCombatManager.currentTarget != null)
            {
                pivotTransform.localPosition = targetPositionLockedOn;
                //pivotTransform.localRotation = Quaternion.identity;
            }
            else
            {
                pivotTransform.localPosition = targetPositionLockedOff;
            }

            yield return null;
        }

        // SWITCH LOCK ON + ITS ENUMERATOR
        public void WaitAndTrySwitchLockOnTarget()
        {
            if (lockOnSwitchCoroutine != null)
            {
                StopCoroutine(lockOnSwitchCoroutine);
            }
            lockOnSwitchCoroutine = StartCoroutine(WaitAndTrySwitchLockOnTargetEnumerator());
        }

        private IEnumerator WaitAndTrySwitchLockOnTargetEnumerator()
        {
            // WAITING UNTIL PLAYER FINISHES AN ACTION
            while (player.isPerfomingAction)
            {
                yield return null;
            }

            TryToFindTargetsToLockOn();
            if (closestTarget != null)
            {
                player.playerNetwork.networkIsLockedOn.Value = true;
                player.playerCombatManager.SetCurrentTarget(closestTarget);
            }

            yield return null;
        }

        // HELPERS
        public void AdjustPositionToPlayers()
        {
            transform.position = player.transform.position;
        }
    }
}
