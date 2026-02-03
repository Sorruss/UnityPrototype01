using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerInteractableManager playerInteractableManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotion;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerNetworkManager playerNetwork;
        [HideInInspector] public PlayerSFXManager playerSFXManager;

        // ------------
        // UNITY EVENTS
        protected override void Awake()
        {
            base.Awake();

            playerInteractableManager = GetComponent<PlayerInteractableManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerNetwork = GetComponent<PlayerNetworkManager>();
            playerSFXManager = GetComponent<PlayerSFXManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner)
                return;

            playerLocomotion.HandleAllMovement();
            playerStatsManager.RecoverStamina();
            playerStatsManager.HandlePoiseReset();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner)
            {
                return;
            }

            base.LateUpdate();
            
            PlayerCamera.instance.HandleAllCameraActions();
        }

        // --------------
        // NETWORK EVENTS
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectedCallback;

            // SETUP HEALTH BAR FOR OTHER PLAYERS TO SEE
            if (!IsOwner)
            {
                characterNetwork.networkCurrentHealth.OnValueChanged += characterUIManager.UpdateHealthBar;
            }

            // SETUP SINGLETONS
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;
                SaveGameManager.instance.player = this;
            }

            // SETUP STATS
            SetupUI_Stamina();
            SetupUI_Health();

            // CHANGE WIELDING WEAPON UPON ID CHANGE
            playerNetwork.networkLeftHandWeaponID.OnValueChanged += playerNetwork.OnLeftHandWeaponIDChanged;
            playerNetwork.networkRightHandWeaponID.OnValueChanged += playerNetwork.OnRightHandWeaponIDChanged;

            // LOCK ON
            playerNetwork.networkIsLockedOn.OnValueChanged += playerNetwork.OnIsLockedOnChanged;

            // STATES
            playerNetwork.networkIsChargingAttack.OnValueChanged += playerNetwork.OnIsChargingAttackChanged;

            // WEAPON ACTIONS STUFF
            playerNetwork.networkCurrentWeaponInUseID.OnValueChanged += playerNetwork.OnCurrentWeaponInUseIDChanged;

            // TWO HANDING
            playerNetwork.networkIsTwoHanding.OnValueChanged += playerNetwork.OnIsTwoHandingChanged;
            playerNetwork.networkIsTwoHandingLeftWeapon.OnValueChanged += playerNetwork.OnIsTwoHandingLeftWeaponChanged;
            playerNetwork.networkIsTwoHandingRightWeapon.OnValueChanged += playerNetwork.OnIsTwoHandingRightWeaponChanged;

            // TODO: IF THE OWNER IS A CLIENT (NOT A SERVER), WE NEED TO LOAD HIS DATA
            if (IsOwner && !IsServer)
            {
                //ImportSaveData(SaveGameManager.instance.GetCurrentCharacterData());
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectedCallback;

            if (!IsOwner)
            {
                characterNetwork.networkCurrentHealth.OnValueChanged -= characterUIManager.UpdateHealthBar;
            }

            // SETUP STATS
            if (IsOwner)
            {
                characterNetwork.networkEndurance.OnValueChanged -= playerNetwork.OnEnduranceValueChanged;
                characterNetwork.networkCurrentStamina.OnValueChanged -= PlayerUIManager.instance.hudManager.OnStaminaChanged;
                characterNetwork.networkMaxStamina.OnValueChanged -= PlayerUIManager.instance.hudManager.OnMaxStaminaChanged;

                characterNetwork.networkVitality.OnValueChanged -= playerNetwork.OnVitalityValueChanged;
                characterNetwork.networkCurrentHealth.OnValueChanged -= PlayerUIManager.instance.hudManager.OnHealthChanged;
                characterNetwork.networkMaxHealth.OnValueChanged -= PlayerUIManager.instance.hudManager.OnMaxHealthChanged;
            }
            characterNetwork.networkCurrentHealth.OnValueChanged -= CheckHealth;

            // CHANGE WIELDING WEAPON UPON ID CHANGE
            playerNetwork.networkLeftHandWeaponID.OnValueChanged -= playerNetwork.OnLeftHandWeaponIDChanged;
            playerNetwork.networkRightHandWeaponID.OnValueChanged -= playerNetwork.OnRightHandWeaponIDChanged;

            // LOCK ON
            playerNetwork.networkIsLockedOn.OnValueChanged -= playerNetwork.OnIsLockedOnChanged;

            // STATES
            playerNetwork.networkIsChargingAttack.OnValueChanged -= playerNetwork.OnIsChargingAttackChanged;

            // WEAPON ACTIONS STUFF
            playerNetwork.networkCurrentWeaponInUseID.OnValueChanged -= playerNetwork.OnCurrentWeaponInUseIDChanged;

            // TWO HANDING
            playerNetwork.networkIsTwoHanding.OnValueChanged -= playerNetwork.OnIsTwoHandingChanged;
            playerNetwork.networkIsTwoHandingLeftWeapon.OnValueChanged -= playerNetwork.OnIsTwoHandingLeftWeaponChanged;
            playerNetwork.networkIsTwoHandingRightWeapon.OnValueChanged -= playerNetwork.OnIsTwoHandingRightWeaponChanged;
        }

        // NETWORK CALLBACKS
        private void OnClientConnectedCallback(ulong clientID)
        {
            GameSessionManager.instance.AddPlayerToList(this);

            // LOAD OTHER CLIENT'S LOADOUT. DO NOT LOAD FOR SERVER
            if (IsServer || !IsOwner)
                return;

            foreach (PlayerManager player in GameSessionManager.instance.players)
            {
                if (player == this)
                    continue;
                
                player.UpdateThisPlayerNetworkValues();
            }
        }

        private void OnClientDisconnectedCallback(ulong clientID)
        {
            GameSessionManager.instance.RemovePlayerFromList(this);
        }

        // NETWORK HELPER FUNCTIONS
        public void UpdateThisPlayerNetworkValues()
        {
            // WEAPONS
            playerNetwork.OnLeftHandWeaponIDChanged(0, playerNetwork.networkLeftHandWeaponID.Value);
            playerNetwork.OnRightHandWeaponIDChanged(0, playerNetwork.networkRightHandWeaponID.Value);

            // TWO HANDING
            playerNetwork.OnIsTwoHandingChanged(false, playerNetwork.networkIsTwoHanding.Value);
            playerNetwork.OnIsTwoHandingLeftWeaponChanged(false, playerNetwork.networkIsTwoHandingLeftWeapon.Value);
            playerNetwork.OnIsTwoHandingRightWeaponChanged(false, playerNetwork.networkIsTwoHandingRightWeapon.Value);

            // LOCK ON
            if (playerNetwork.networkIsLockedOn.Value)
                playerNetwork.OnTargetNetworkObjectIDChanged(0, playerNetwork.networkTargetNetworkObejectID.Value);
        }

        private void SetupUI_Stamina()
        {
            if (IsOwner)
            {
                characterNetwork.networkEndurance.OnValueChanged += playerNetwork.OnEnduranceValueChanged;

                characterNetwork.networkCurrentStamina.OnValueChanged += PlayerUIManager.instance.hudManager.OnStaminaChanged;
                characterNetwork.networkMaxStamina.OnValueChanged += PlayerUIManager.instance.hudManager.OnMaxStaminaChanged;

                PlayerUIManager.instance.hudManager.SetMaxStamina(characterNetwork.networkMaxStamina.Value);
                PlayerUIManager.instance.hudManager.SetStamina(characterNetwork.networkCurrentStamina.Value);
            }
        }

        private void SetupUI_Health()
        {
            if (IsOwner)
            {
                characterNetwork.networkVitality.OnValueChanged += playerNetwork.OnVitalityValueChanged;

                characterNetwork.networkCurrentHealth.OnValueChanged += PlayerUIManager.instance.hudManager.OnHealthChanged;
                characterNetwork.networkMaxHealth.OnValueChanged += PlayerUIManager.instance.hudManager.OnMaxHealthChanged;

                PlayerUIManager.instance.hudManager.SetMaxHealth(characterNetwork.networkMaxHealth.Value);
                PlayerUIManager.instance.hudManager.SetHealth(characterNetwork.networkCurrentHealth.Value);
            }
            
            characterNetwork.networkCurrentHealth.OnValueChanged += CheckHealth;
        }

        // ---------------------
        // SAVE SYSTEM FUNCTIONS
        public void ImportSaveData(CharacterSaveData saveData)
        {
            // Importing values from save file.
            playerNetwork.networkPlayerName.Value = saveData.characterName;
            playerNetwork.networkEndurance.Value = saveData.enduranceLevel;
            playerNetwork.networkVitality.Value = saveData.vitalityLevel;
            transform.position = saveData.GetPosition();

            // Setting camera position so it doesn't lurp to the player on the start of the game.
            PlayerCamera.instance.AdjustPositionToPlayers();

            // Setting up HEALTH values.
            characterNetwork.networkMaxHealth.Value = playerStatsManager.GetMaxHealthOfVitalityLevel(saveData.vitalityLevel);
            characterNetwork.networkCurrentHealth.Value = saveData.currentHealth;

            // Setting up STAMINA values.
            characterNetwork.networkMaxStamina.Value = playerStatsManager.GetMaxStaminaOfEnduranceLevel(saveData.enduranceLevel);
            characterNetwork.networkCurrentStamina.Value = saveData.currentStamina;

            // Inventory.


            // Equipment.
            //playerNetwork.networkLeftHandWeaponID.Value = saveData.weaponLeftHandID;
            //playerNetwork.networkRightHandWeaponID.Value = saveData.weaponRightHandID;
        }

        public void ExportSaveData(CharacterSaveData saveData)
        {
            // VALUES.
            saveData.characterName = playerNetwork.networkPlayerName.Value.ToString();

            // STATS.
            saveData.enduranceLevel = playerNetwork.networkEndurance.Value;
            saveData.vitalityLevel = playerNetwork.networkVitality.Value;

            // RESOURCES.
            saveData.currentStamina = playerNetwork.networkCurrentStamina.Value;
            saveData.currentHealth = playerNetwork.networkCurrentHealth.Value;

            // TRANSFORM.
            saveData.SavePosition(transform.position);

            // INVENTORY.


            // EQUIPMENT.
            //saveData.weaponLeftHandID = playerNetwork.networkLeftHandWeaponID.Value;
            //saveData.weaponRightHandID = playerNetwork.networkRightHandWeaponID.Value;
        }

        // -----
        // OTHER
        protected override void Respawn()
        {
            base.Respawn();

            if (!IsOwner)
            {
                return;
            }

            characterNetwork.networkIsDead.Value = false;
            characterNetwork.networkCurrentHealth.Value = characterNetwork.networkMaxHealth.Value;
            characterNetwork.networkCurrentStamina.Value = characterNetwork.networkMaxStamina.Value;

            characterAnimatorManager.PerformAnimationAction("Empty", false);
        }

        public override IEnumerator ProcessDeath(bool manuallySelectDeathAnimation = false)
        {
            yield return base.ProcessDeath(manuallySelectDeathAnimation);

            if (IsOwner)
                PlayerUIManager.instance.popUpManager.SendYouDiedPopUp();

            yield return null;
        }
    }
}
