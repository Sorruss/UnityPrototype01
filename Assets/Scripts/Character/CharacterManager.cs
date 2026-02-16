using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Components")]
        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;
        [HideInInspector] public CharacterStatsManager characterStatsManager;
        [HideInInspector] public CharacterNetworkManager characterNetwork;
        [HideInInspector] public CharacterSFXManager characterSFXManager;
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public CharacterUIManager characterUIManager;
        [HideInInspector] public Animator animator;

        [Header("Identification")]
        public CharacterTeam characterTeam;

        [Header("Flags")]
        public bool isPerfomingAction = false;

        [Header("Ground Check")]
        [SerializeField] private float groundCheckSphereRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        // ------------
        // UNITY EVENTS
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterNetwork = GetComponent<CharacterNetworkManager>();
            characterSFXManager = GetComponent<CharacterSFXManager>();
            characterController = GetComponent<CharacterController>();
            characterUIManager = GetComponent<CharacterUIManager>();
            animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            HandleNetworkVariables();
            HandleIsGrounded();

            if (!IsOwner)
                return;

            characterStatsManager.HandlePoiseReset();
        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void FixedUpdate()
        {
            
        }

        // -------
        // NETWORK
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            characterNetwork.OnIsMovingChanged(false, characterNetwork.networkIsMoving.Value);
            characterNetwork.networkIsMoving.OnValueChanged += characterNetwork.OnIsMovingChanged;

            characterNetwork.OnIsActiveChanged(false, characterNetwork.networkIsActive.Value);
            characterNetwork.networkIsActive.OnValueChanged += characterNetwork.OnIsActiveChanged;

            characterNetwork.OnIsDeadChanged(false, characterNetwork.networkIsDead.Value);
            characterNetwork.networkIsDead.OnValueChanged += characterNetwork.OnIsDeadChanged;

            characterNetwork.OnIsBlockingChanged(false, characterNetwork.networkIsBlocking.Value);
            characterNetwork.networkIsBlocking.OnValueChanged += characterNetwork.OnIsBlockingChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            characterNetwork.networkIsMoving.OnValueChanged -= characterNetwork.OnIsMovingChanged;
            characterNetwork.networkIsActive.OnValueChanged -= characterNetwork.OnIsActiveChanged;
            characterNetwork.networkIsDead.OnValueChanged -= characterNetwork.OnIsDeadChanged;
            characterNetwork.networkIsBlocking.OnValueChanged -= characterNetwork.OnIsBlockingChanged;
        }

        // ------
        // GIZMOS
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, groundCheckSphereRadius);
        }

        // ------------
        // SUPP METHODS
        private void HandleIsGrounded()
        {
            characterLocomotionManager.isGrounded = Physics.CheckSphere(transform.position, groundCheckSphereRadius, groundLayer);
        }

        private void HandleNetworkVariables()
        {
            if (IsOwner)
            {
                characterNetwork.networkPosition.Value = transform.position;
                characterNetwork.networkRotation.Value = transform.rotation;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    characterNetwork.networkPosition.Value,
                    ref characterNetwork.networkPositionVelocity,
                    characterNetwork.networkPositionSmoothTime);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    characterNetwork.networkRotation.Value,
                    characterNetwork.networkRotationSmoothTime);
            }
        }

        protected virtual void Respawn()
        {

        }

        protected virtual void CheckHealth(float prevHealth, float newHealth)
        {
            if (newHealth <= 0.0f)
            {
                StartCoroutine(ProcessDeath());
            }

            if (IsOwner)
            {
                if (newHealth > characterNetwork.networkMaxHealth.Value)
                {
                    characterNetwork.networkCurrentHealth.Value = characterNetwork.networkMaxHealth.Value;
                }
            }
        }

        public virtual IEnumerator ProcessDeath(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                // Reset/set all needed flags.
                characterNetwork.networkCurrentHealth.Value = 0.0f;
                characterNetwork.networkIsDead.Value = true;

                // Do aerial death animation if in air.
                // Loose souls.

                // DEATH ANIMATION
                if (!manuallySelectDeathAnimation && !characterNetwork.networkIsBeingCriticallyDamaged.Value)
                {
                    characterAnimatorManager.PerformAnimationAction("Death_01", true);
                }
            }

            // DISABLE DAMAGE COLLIDERS
            characterCombatManager.DisableAllDamageColliders();

            // Play SFX death sound.
            characterSFXManager.PlayAudioClip(SFXManager.instance.GetRandomSFX(ref characterSFXManager.deathGrunts));

            yield return null;

            // Award player with souls.

            // Enable ability to respawn.
        }

        protected virtual void IgnoreMyOwnColliders()
        {
            // GET EVERY NEEDED COLLIDER AND ADD THEM TO ONE LIST.
            Collider controllerCollider = GetComponent<Collider>();
            Collider[] colliders = GetComponentsInChildren<Collider>();
            List<Collider> collidersToIgnore = new List<Collider>();

            foreach (Collider collider in colliders)
            {
                collidersToIgnore.Add(collider);
            }

            collidersToIgnore.Add(controllerCollider);

            // SET THOSE COLLIDERS TO IGNORE EACH OTHER.
            foreach (Collider collider1 in collidersToIgnore)
            {
                foreach (Collider collider2 in collidersToIgnore)
                {
                    Physics.IgnoreCollision(collider1, collider2, true);
                }
            }
        }
    }
}
