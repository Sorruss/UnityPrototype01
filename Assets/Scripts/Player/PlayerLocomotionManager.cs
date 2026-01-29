using System;
using UnityEngine;

namespace FG
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager player;

        private float horizontalMovement;
        private float verticalMovement;
        private float moveAmount;
        private float currentHorizontalSpeed;

        [Header("Speed settings")]
        [SerializeField] private float walkMoveSpeed = 2.0f;
        [SerializeField] private float runMoveSpeed = 5.0f;
        [SerializeField] private float sprintMoveSpeed = 8.0f;
        [SerializeField] private float rotationTime = 15.0f;

        [Header("Jump")]
        [SerializeField] private float jumpHeight = 20.0f;
        [SerializeField] private float airMoveSpeed = 3.0f;

        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;

        private float sprintStaminaTimer = 0.0f;
        private float sprintStaminaWaste = 0.4f;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            HandleNetworkVariables();
        }

        private void HandleNetworkVariables()
        {
            if (player.IsOwner)
            {
                player.characterNetwork.networkHorizontal.Value = horizontalMovement;
                player.characterNetwork.networkVertical.Value = verticalMovement;
                player.characterNetwork.networkMoveAmount.Value = moveAmount;
            }
            else
            {
                horizontalMovement = player.characterNetwork.networkHorizontal.Value;
                verticalMovement = player.characterNetwork.networkVertical.Value;
                moveAmount = player.characterNetwork.networkMoveAmount.Value;

                if (!player.playerNetwork.networkIsLockedOn.Value)
                {   // NOT LOCKED ON MOVEMENT (ONE DIRECTIONAL)
                    player.playerAnimatorManager.UpdateMovementValues(0.0f, moveAmount, player.characterNetwork.networkIsSprinting.Value);
                }
                else
                {   // LOCKED ON MOVEMENT (8 DIRECTIONAL)
                    player.playerAnimatorManager.UpdateMovementValues(horizontalMovement, verticalMovement, player.characterNetwork.networkIsSprinting.Value);
                }
            }
        }

        // FUNDAMENTAL ACTIONS
        private void GetMovementInputs()
        {
            if (!player.characterLocomotionManager.canMove && !player.characterLocomotionManager.canRotate)
                return;

            horizontalMovement = PlayerInputManager.instance.horizontalMovement;
            verticalMovement = PlayerInputManager.instance.verticalMovement;
            moveAmount = PlayerInputManager.instance.moveAmount;
        }

        public void HandleAllMovement()
        {
            GetMovementInputs();
            HandleGroundedMovement();
            HandleAirMovement();
            HandleRotation();
        }

        private void HandleAirMovement()
        {
            if (!player.characterLocomotionManager.isGrounded && player.characterNetwork.networkIsJumping.Value)   // Give player a horizontal speed when he jumps.
            {
                player.characterController.Move(currentHorizontalSpeed * Time.deltaTime * moveDirection);
            }

            if (!player.characterLocomotionManager.isGrounded)     // Give player air control when he falls.
            {
                Vector3 airMoveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
                airMoveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
                airMoveDirection.y = 0;
                airMoveDirection.Normalize();

                player.characterController.Move(airMoveSpeed * Time.deltaTime * airMoveDirection);
            }
        }

        private void HandleGroundedMovement()
        {
            if (!player.characterLocomotionManager.canMove)
                return;

            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.y = 0;
            moveDirection.Normalize();

            if (player.playerNetwork.networkIsSprinting.Value)
            {
                currentHorizontalSpeed = sprintMoveSpeed;
            }
            else
            {
                if (moveAmount <= 0.5f)
                {
                    currentHorizontalSpeed = walkMoveSpeed;
                }
                else if (moveAmount > 0.5f)
                {
                    currentHorizontalSpeed = runMoveSpeed;
                }
            }

            player.characterController.Move(currentHorizontalSpeed * Time.deltaTime * moveDirection);
        }

        private void HandleRotation()
        {
            if (player.playerNetwork.networkIsDead.Value)
                return;

            if (!player.characterLocomotionManager.canRotate)
                return;

            if (player.playerNetwork.networkIsLockedOn.Value)
            {
                if (player.playerNetwork.networkIsSprinting.Value ||
                    player.playerNetwork.networkIsRolling.Value)
                {
                    FreeRotation();
                }
                else
                {   // LOCKED ON ROTATION
                    targetRotationDirection = player.playerCombatManager.currentTarget.transform.position - player.transform.position;
                    targetRotationDirection.y = 0.0f;
                    targetRotationDirection.Normalize();

                    Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationTime * Time.deltaTime);
                    transform.rotation = targetRotation;
                }
            }
            else
            {
                FreeRotation();
            }
        }

        private void FreeRotation()
        {
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.y = 0;
            targetRotationDirection.Normalize();

            if (targetRotationDirection == Vector3.zero)
                targetRotationDirection = transform.forward;

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationTime * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        // ADDITIONAL ACTIONS
        public void TryToPerformDodgeAction()
        {
            if (player.isPerfomingAction 
                || player.characterNetwork.networkIsJumping.Value
                || !player.playerStatsManager.IsEnoughStamina(player.playerStatsManager.dodgeStaminaCost))
            {
                return;
            }

            if (moveAmount > 0)
            {
                // Rotate the player in direction of his movement.
                Vector3 rollDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                rollDirection.y = 0.0f;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                transform.rotation = playerRotation;

                // Roll action.
                player.playerAnimatorManager.PerformAnimationAction("Roll_F_01", true);
            }
            else
            {
                // Backstep action.
                player.playerAnimatorManager.PerformAnimationAction("Backstep_01", true);
            }

            player.playerNetwork.networkIsRolling.Value = true;
            player.playerStatsManager.TryDecreaseStamina(player.playerStatsManager.dodgeStaminaCost);
        }

        public void HandleSprinting()
        {
            if (player.isPerfomingAction 
                || !player.playerStatsManager.IsEnoughStamina(player.playerStatsManager.sprintStaminaCost))
            {
                player.playerNetwork.networkIsSprinting.Value = false;
                return;
            }

            if (moveAmount >= 0.5f)
            {
                player.playerNetwork.networkIsSprinting.Value = true;
            }
            else
            {
                player.playerNetwork.networkIsSprinting.Value = false;
                return;
            }

            sprintStaminaTimer += Time.deltaTime;
            if (sprintStaminaTimer >= sprintStaminaWaste)
            {
                sprintStaminaTimer = 0.0f;
                player.playerStatsManager.TryDecreaseStamina(player.playerStatsManager.sprintStaminaCost);
            }
        }

        public void TryToPerformJumpAction()
        {
            if (player.isPerfomingAction
                || player.characterNetwork.networkIsJumping.Value
                || !player.characterLocomotionManager.isGrounded
                || !player.playerStatsManager.IsEnoughStamina(player.playerStatsManager.jumpStaminaCost))
            {
                return;
            }

            player.playerAnimatorManager.PerformAnimationAction("Main_Jump_01_Start", false, true, true, false);
            player.playerStatsManager.TryDecreaseStamina(player.playerStatsManager.jumpStaminaCost);
            player.characterNetwork.networkIsJumping.Value = true;
        }

        // ANIMATION EVENTS
        public void ApplyJumpingForce()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -1 * gravityForce);
        }
    }
}
