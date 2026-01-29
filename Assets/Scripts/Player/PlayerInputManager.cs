using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace FG
{
    public class PlayerInputManager : MonoBehaviour
    {
        [HideInInspector] public static PlayerInputManager instance {  get; private set; }

        [HideInInspector] public PlayerManager player;

        private PlayerInput playerInput;
        
        // ALL THE FLAGS/VALUES FOR MANAGING THE INPUTS
        [Header("Inputs")]
        [SerializeField] private Vector2 MovementInput;
        [SerializeField] private Vector2 CameraInput;

        [Header("Flags")]
        [SerializeField] public bool isDodging = false;
        [SerializeField] public bool isSprinting = false;
        [SerializeField] private bool isJumping = false;
        [SerializeField] private bool isInteracting = false;

        [Header("Action Flags")]
        [SerializeField] private bool isRBActionActive = false;
        [SerializeField] private bool isRTActionActive = false;
        [SerializeField] private bool isHoldingRT = false;

        [SerializeField] private bool isLBActionActive = false;

        [Header("Lock On Flag")]
        [SerializeField] private bool isTryingToLockOn = false;
        [SerializeField] private bool isTryingSwitchLockOnLeft = false;
        [SerializeField] private bool isTryingSwitchLockOnRight = false;

        [Header("Quick Slots Flags")]
        [SerializeField] private bool isDPadLeftActionActive = false;
        [SerializeField] private bool isDPadRightActionActive = false;

        [Header("Input Queue Config")]
        [SerializeField] private float inputQueueTime = 0.35f;
        private float inputQueueTimer = 0.0f;

        [Header("Input Queue Flags")]
        [SerializeField] private bool isInputQueueActive = false;
        [SerializeField] private bool isRBQueueActive = false;
        [SerializeField] private bool isRTQueueActive = false;

        // PUBLIC VARIABLES FOR OTHER COMPONENTS TO GET VALUES FROM
        // CHARACTER MOVEMENT VALUES
        [HideInInspector] public float horizontalMovement { get; private set; }
        [HideInInspector] public float verticalMovement { get; set; }
        [HideInInspector] public float moveAmount { get; private set; }

        // CAMERA MOVEMENT VALUES
        [HideInInspector] public float cameraHorizontalMovement { get; private set; }
        [HideInInspector] public float cameraVerticalMovement { get; private set; }

        // ----------------------------------
        // INITIALIZATION AND STUFF LIKE THAT.
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
            }

            playerInput = new PlayerInput();
        }

        private void Update()
        {
            HandleAllActions();
        }

        private void HandleAllActions()
        {
            HandleMovementInput();
            HandleCameraInput();
            // MOVEMENT ACTIONS
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            // INTERACTION
            HandleInteractionInput();
            // ACTIONS
            HandleRBActionInput();
            HandleRTActionInput();
            HandleRTHoldActionInput();
            HandleLBActionInput();
            // LOCK ON STUFF
            HandleLockOnInput();
            HandleSwitchLockOnInput();
            // QUICK SLOTS
            HandleAllQuickSlotsInputs();
            // INPUT QUEUE
            HandleAllInputQueue();
        }

        // -----------------------
        // INPUT HANDLER FUNCTIONS
        private void HandleMovementInput()
        {
            // SAFERY MEASURE
            if (player == null)
            {
                return;
            }

            // GET ALL THE VALUES WE NEED
            verticalMovement = MovementInput.y;
            horizontalMovement = MovementInput.x;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalMovement) + Mathf.Abs(verticalMovement));
            
            // CLIP THE MOVE_AMOUNT VALUE SO THE MOVEMENT IS NICER
            if (moveAmount > 0.0f && moveAmount <= 0.5f)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5f && moveAmount <= 1.0f)
            {
                moveAmount = 1.0f;
            }

            // SET THE IS_MOVING VALUE DEPENDING OF THE MOVE_AMOUNT VALUE
            // SO ANIMATOR KNOWS TO APPLY IDLE ANIMATION OR WHOLE LOCOMOTION
            player.playerNetwork.networkIsMoving.Value = moveAmount != 0.0f;

            // LOCKED ON/OFF MOVEMENT
            if (!player.playerNetwork.networkIsLockedOn.Value)
            {   // NOT LOCKED ON MOVEMENT (ONE DIRECTIONAL)
                player.playerAnimatorManager.UpdateMovementValues(0.0f, moveAmount, player.characterNetwork.networkIsSprinting.Value);
            }
            else
            {   // LOCKED ON MOVEMENT (8 DIRECTIONAL)
                player.playerAnimatorManager.UpdateMovementValues(horizontalMovement, verticalMovement, player.characterNetwork.networkIsSprinting.Value);
            }
        }

        private void HandleCameraInput()
        {
            cameraHorizontalMovement = CameraInput.x;
            cameraVerticalMovement = CameraInput.y;
        }

        private void OnSceneChanged(Scene prevScene, Scene newScene)
        {
            if (newScene.buildIndex == SaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
                PlayerUIManager.instance.EnableCursor(false);
            }
            else
            {
                instance.enabled = false;
            }
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChanged;

            instance.enabled = false;
        }

        private void OnEnable()
        {
            playerInput.Enable();

            playerInput.Default.Movement.performed += _ => MovementInput = playerInput.Default.Movement.ReadValue<Vector2>();
            playerInput.Default.LookAround.performed += _ => CameraInput = playerInput.Default.LookAround.ReadValue<Vector2>();

            // MOVEMENT ACTIONS
            playerInput.Default.Button_East.performed += _ => isDodging = true;

            playerInput.Default.Button_West.performed += _ => isSprinting = true;
            playerInput.Default.Button_West.canceled += OnSprintActionEnd;

            playerInput.Default.Button_South.performed += _ => isJumping = true;

            // INTERACTING
            playerInput.Default.Button_North.performed += _ => isInteracting = true;

            // ACTIONS
            playerInput.Default.RightBumper.performed += _ => isRBActionActive = true;
            playerInput.Default.RightBumperQueue.performed += _ => EnableInputQueue(ref isRBQueueActive);
            playerInput.Default.RightTrigger.performed += _ => isRTActionActive = true;
            playerInput.Default.RightTriggerQueue.performed += _ => EnableInputQueue(ref isRTQueueActive);

            playerInput.Default.RightTriggerHold.performed += _ => isHoldingRT = true;
            playerInput.Default.RightTriggerHold.canceled += _ => isHoldingRT = false;

            playerInput.Default.LeftBumper.performed += _ => isLBActionActive = true;
            playerInput.Default.LeftBumper.canceled += _ => isLBActionActive = false;

            // LOCK ON INPUTS
            playerInput.Default.RightStick.performed += _ => isTryingToLockOn = true;
            playerInput.Default.RightStickLeft.performed += _ => isTryingSwitchLockOnLeft = true;
            playerInput.Default.RightStickRight.performed += _ => isTryingSwitchLockOnRight = true;

            // QUICK SLOTS INPUTS
            playerInput.Default.DPadLeft.performed += _ => isDPadLeftActionActive = true;
            playerInput.Default.DPadRight.performed += _ => isDPadRightActionActive = true;
        }

        // --------------------
        // "ON"/"OFF" FUNCTIONS.
        private void OnSprintActionEnd(InputAction.CallbackContext obj)
        {
            isSprinting = false;
            player.playerNetwork.networkIsSprinting.Value = false;
        }

        // ---------------------------------
        // MOVEMENT ACTION HANDLER FUNCTIONS.
        private void HandleJumpInput()
        {
            if (isJumping)
            {
                isJumping = false;
                player.playerLocomotion.TryToPerformJumpAction();
            }
        }

        private void HandleSprintInput()
        {
            if (isSprinting)
            {
                player.playerLocomotion.HandleSprinting();
            }
        }

        private void HandleDodgeInput()
        {
            if (isDodging)
            {
                isDodging = false;
                player.playerLocomotion.TryToPerformDodgeAction();
            }
        }

        // -----------
        // INTERACTION
        private void HandleInteractionInput()
        {
            if (isInteracting)
            {
                isInteracting = false;
                player.playerInteractableManager.Interact();
            }
        }

        // ------------------------
        // ACTION HANDLER FUNCTIONS
        private void HandleRBActionInput()
        {
            if (isRBActionActive)
            {
                // DISABLE LOOPING.
                isRBActionActive = false;

                // INFORM THAT CLIENT IS USING RIGHT HAND WEAPON ACTION.
                player.playerNetwork.SetCurrentActiveHand(true);

                // DO RIGHT HAND WEAPON ACTION.
                player.playerCombatManager.TryToPerformWeaponAction(
                    player.playerInventoryManager.RightHandWeaponSciptable.OH_RB_Action,
                    player.playerInventoryManager.RightHandWeaponSciptable);
            }
        }

        private void HandleRTActionInput()
        {
            if (isRTActionActive)
            {
                isRTActionActive = false;
                player.playerNetwork.SetCurrentActiveHand(true);
                player.playerCombatManager.TryToPerformWeaponAction(
                    player.playerInventoryManager.RightHandWeaponSciptable.OH_RT_Action,
                    player.playerInventoryManager.RightHandWeaponSciptable);
            }
        }

        private void HandleRTHoldActionInput()
        {
            if (player.isPerfomingAction)
            {
                if (player.playerNetwork.networkIsUsingRightHand.Value)
                {
                    player.playerNetwork.networkIsChargingAttack.Value = isHoldingRT;
                }
            }
        }

        private void HandleLBActionInput()
        {
            if (!isLBActionActive ||
                player.isPerfomingAction ||
                player.playerNetwork.networkLeftHandWeaponID.Value == 0)
            {
                player.playerNetwork.networkIsBlocking.Value = false;
                return;
            }

            player.playerNetwork.networkIsBlocking.Value = true;
        }

        // ---------------
        // LOCK ON HANDLER
        private void HandleLockOnInput()
        {
            // UNLOCK IF WE ARE LOCKED ON AND THE TARGET IS DEAD
            if (player.playerNetwork.networkIsLockedOn.Value)
            {
                if (player.playerCombatManager.currentTarget == null)
                    return;

                if (player.playerCombatManager.currentTarget.characterNetwork.networkIsDead.Value)
                {
                    player.playerNetwork.networkIsLockedOn.Value = false;
                    PlayerCamera.instance.WaitAndTrySwitchLockOnTarget();
                }
            }

            // LOCK ON IF WE ARE NOT LOCKED ON
            if (isTryingToLockOn && !player.playerNetwork.networkIsLockedOn.Value)
            {
                isTryingToLockOn = false;
                PlayerCamera.instance.TryToFindTargetsToLockOn();
                if (PlayerCamera.instance.closestTarget != null)
                {
                    player.playerNetwork.networkIsLockedOn.Value = true;
                    player.playerCombatManager.SetCurrentTarget(PlayerCamera.instance.closestTarget);
                }
            }

            // UNLOCK IF WE ARE LOCKED ON
            if (isTryingToLockOn && player.playerNetwork.networkIsLockedOn.Value)
            {
                isTryingToLockOn = false;
                player.playerNetwork.networkIsLockedOn.Value = false;
                player.playerCombatManager.SetCurrentTarget(null);
                PlayerCamera.instance.ClearLockOnTargets();
            }
        }
        
        private void HandleSwitchLockOnInput()
        {
            if (isTryingSwitchLockOnLeft)
            {
                isTryingSwitchLockOnLeft = false;
            
                if (player.playerNetwork.networkIsLockedOn.Value)
                {
                    PlayerCamera.instance.TryToFindTargetsToLockOn();
                    if (PlayerCamera.instance.closestLeftTarget != null)
                    {
                        player.playerCombatManager.SetCurrentTarget(PlayerCamera.instance.closestLeftTarget);
                    }
                }
            }

            if (isTryingSwitchLockOnRight)
            {
                isTryingSwitchLockOnRight = false;

                if (player.playerNetwork.networkIsLockedOn.Value)
                {
                    PlayerCamera.instance.TryToFindTargetsToLockOn();
                    if (PlayerCamera.instance.closestRightTarget != null)
                    {
                        player.playerCombatManager.SetCurrentTarget(PlayerCamera.instance.closestRightTarget);
                    }
                }
            }
        }

        // --------------------
        // QUICK SLOTS HANDLERS
        private void HandleAllQuickSlotsInputs()
        {
            HandleSwitchLeftWeaponInput();
            HandleSwitchRightWeaponInput();
        }

        private void HandleSwitchLeftWeaponInput()
        {
            if (isDPadLeftActionActive)
            {
                isDPadLeftActionActive = false;
                player.playerEquipmentManager.SwitchLeftWeapon();
            }
        }

        private void HandleSwitchRightWeaponInput()
        {
            if (isDPadRightActionActive)
            {
                isDPadRightActionActive = false;
                player.playerEquipmentManager.SwitchRightWeapon();
            }
        }

        // -----------
        // INPUT QUEUE
        private void HandleAllInputQueue()
        {
            if (player.playerNetwork.networkIsDead.Value)
                isInputQueueActive = false;

            if (!isInputQueueActive)
            {
                ResetInputQueue();
                return;
            }

            ProcessInputQueue();
        }

        private void ProcessInputQueue()
        {
            if (inputQueueTimer < inputQueueTime)
            {
                inputQueueTimer += Time.deltaTime;

                if (isRTQueueActive)
                    isRTActionActive = true;

                if (isRBQueueActive)
                    isRBActionActive = true;
            }
            else
            {
                isInputQueueActive = false;
            }
        }

        private void EnableInputQueue(ref bool inputQueueAction)
        {
            ResetInputQueue();

            if (player.isPerfomingAction || player.characterNetwork.networkIsJumping.Value)
            {
                inputQueueAction = true;
                isInputQueueActive = true;
            }
        }

        private void ResetInputQueue()
        {
            inputQueueTimer = 0.0f;
            isRTQueueActive = false;
            isRBQueueActive = false;
        }

        // ---------------
        // EVERYTHING ELSE.
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerInput.Enable();
                }
                else
                {
                    playerInput.Disable();
                }
            }
        }
        
        private void OnDisable()
        {
            if (playerInput != null)
            {
                playerInput.Default.Button_West.canceled -= OnSprintActionEnd;
                playerInput.Disable();
            }
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
    }
}
