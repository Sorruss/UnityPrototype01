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
        [HideInInspector] public PlayerEffectsManager playerEffectsManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotion;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerBodyManager playerBodyManager;
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
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerBodyManager = GetComponent<PlayerBodyManager>();
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

            // PLAYER INFORMATION
            playerNetwork.networkIsMale.OnValueChanged += playerNetwork.OnIsMaleChanged;

            // CHANGE WIELDING WEAPON UPON ID CHANGE
            playerNetwork.networkLeftHandWeaponID.OnValueChanged += playerNetwork.OnLeftHandWeaponIDChanged;
            playerNetwork.networkRightHandWeaponID.OnValueChanged += playerNetwork.OnRightHandWeaponIDChanged;

            // LOCK ON
            playerNetwork.networkIsLockedOn.OnValueChanged += playerNetwork.OnIsLockedOnChanged;

            // STATES
            playerNetwork.networkIsChargingAttack.OnValueChanged += playerNetwork.OnIsChargingAttackChanged;

            // WEAPONS STUFF
            playerNetwork.networkCurrentWeaponInUseID.OnValueChanged += playerNetwork.OnCurrentWeaponInUseIDChanged;

            // TWO HANDING
            playerNetwork.networkIsTwoHanding.OnValueChanged += playerNetwork.OnIsTwoHandingChanged;
            playerNetwork.networkIsTwoHandingLeftWeapon.OnValueChanged += playerNetwork.OnIsTwoHandingLeftWeaponChanged;
            playerNetwork.networkIsTwoHandingRightWeapon.OnValueChanged += playerNetwork.OnIsTwoHandingRightWeaponChanged;

            // ARMOR
            playerNetwork.networkArmorHelmetID.OnValueChanged += playerNetwork.OnArmorHelmetIDChanged;
            playerNetwork.networkArmorChestplateID.OnValueChanged += playerNetwork.OnArmorChestplateIDChanged;
            playerNetwork.networkArmorGauntletsID.OnValueChanged += playerNetwork.OnArmorGauntletsIDChanged;
            playerNetwork.networkArmorLegginsID.OnValueChanged += playerNetwork.OnArmorLegginsIDChanged;

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

            // PLAYER INFORMATION
            playerNetwork.networkIsMale.OnValueChanged -= playerNetwork.OnIsMaleChanged;

            // CHANGE WIELDING WEAPON UPON ID CHANGE
            playerNetwork.networkLeftHandWeaponID.OnValueChanged -= playerNetwork.OnLeftHandWeaponIDChanged;
            playerNetwork.networkRightHandWeaponID.OnValueChanged -= playerNetwork.OnRightHandWeaponIDChanged;

            // LOCK ON
            playerNetwork.networkIsLockedOn.OnValueChanged -= playerNetwork.OnIsLockedOnChanged;

            // STATES
            playerNetwork.networkIsChargingAttack.OnValueChanged -= playerNetwork.OnIsChargingAttackChanged;

            // WEAPONS STUFF
            playerNetwork.networkCurrentWeaponInUseID.OnValueChanged -= playerNetwork.OnCurrentWeaponInUseIDChanged;

            // TWO HANDING
            playerNetwork.networkIsTwoHanding.OnValueChanged -= playerNetwork.OnIsTwoHandingChanged;
            playerNetwork.networkIsTwoHandingLeftWeapon.OnValueChanged -= playerNetwork.OnIsTwoHandingLeftWeaponChanged;
            playerNetwork.networkIsTwoHandingRightWeapon.OnValueChanged -= playerNetwork.OnIsTwoHandingRightWeaponChanged;

            // ARMOR
            playerNetwork.networkArmorHelmetID.OnValueChanged -= playerNetwork.OnArmorHelmetIDChanged;
            playerNetwork.networkArmorChestplateID.OnValueChanged -= playerNetwork.OnArmorChestplateIDChanged;
            playerNetwork.networkArmorGauntletsID.OnValueChanged -= playerNetwork.OnArmorGauntletsIDChanged;
            playerNetwork.networkArmorLegginsID.OnValueChanged -= playerNetwork.OnArmorLegginsIDChanged;
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
            // PLAYER INFORMATION
            playerNetwork.OnIsMaleChanged(false, playerNetwork.networkIsMale.Value);

            // WEAPONS
            playerNetwork.OnLeftHandWeaponIDChanged(0, playerNetwork.networkLeftHandWeaponID.Value);
            playerNetwork.OnRightHandWeaponIDChanged(0, playerNetwork.networkRightHandWeaponID.Value);

            // TWO HANDING
            playerNetwork.OnIsTwoHandingLeftWeaponChanged(false, playerNetwork.networkIsTwoHandingLeftWeapon.Value);
            playerNetwork.OnIsTwoHandingRightWeaponChanged(false, playerNetwork.networkIsTwoHandingRightWeapon.Value);

            // LOCK ON
            if (playerNetwork.networkIsLockedOn.Value)
                playerNetwork.OnTargetNetworkObjectIDChanged(0, playerNetwork.networkTargetNetworkObejectID.Value);

            // ARMOR
            playerNetwork.OnArmorHelmetIDChanged(0, playerNetwork.networkArmorHelmetID.Value);
            playerNetwork.OnArmorChestplateIDChanged(0, playerNetwork.networkArmorChestplateID.Value);
            playerNetwork.OnArmorGauntletsIDChanged(0, playerNetwork.networkArmorGauntletsID.Value);
            playerNetwork.OnArmorLegginsIDChanged(0, playerNetwork.networkArmorLegginsID.Value);
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
            // CHARACTER INFORMATION
            playerNetwork.networkPlayerName.Value = saveData.characterName;
            playerNetwork.networkIsMale.Value = saveData.isMale;

            // STATS
            playerNetwork.networkEndurance.Value = saveData.enduranceLevel;
            playerNetwork.networkVitality.Value = saveData.vitalityLevel;
            playerNetwork.networkStrength.Value = saveData.strengthLevel;

            // POSITION
            transform.position = saveData.GetPosition();

            // Setting camera position so it doesn't lurp to the player on the start of the game
            PlayerCamera.instance.AdjustPositionToPlayers();

            // Setting up HEALTH values
            characterNetwork.networkMaxHealth.Value = playerStatsManager.GetMaxHealthOfVitalityLevel(saveData.vitalityLevel);
            characterNetwork.networkCurrentHealth.Value = saveData.currentHealth;

            // Setting up STAMINA values
            characterNetwork.networkMaxStamina.Value = playerStatsManager.GetMaxStaminaOfEnduranceLevel(saveData.enduranceLevel);
            characterNetwork.networkCurrentStamina.Value = saveData.currentStamina;

            // Weapons quick slots
            playerInventoryManager.LeftHandWeaponIndex = saveData.leftHandWeaponQuickSlotIndex;
            playerInventoryManager.RightHandWeaponIndex = saveData.rightHandWeaponQuickSlotIndex;

            WeaponItem weapon = ItemDatabase.instance.GetWeaponItemByID(saveData.leftHandWeaponID_01);
            if (weapon == null)
                weapon = ItemDatabase.instance.unarmedWeapon;
            playerInventoryManager.LeftHandWeaponScriptables[0] = weapon;

            weapon = ItemDatabase.instance.GetWeaponItemByID(saveData.leftHandWeaponID_02);
            if (weapon == null)
                weapon = ItemDatabase.instance.unarmedWeapon;
            playerInventoryManager.LeftHandWeaponScriptables[1] = weapon;

            weapon = ItemDatabase.instance.GetWeaponItemByID(saveData.leftHandWeaponID_03);
            if (weapon == null)
                weapon = ItemDatabase.instance.unarmedWeapon;
            playerInventoryManager.LeftHandWeaponScriptables[2] = weapon;

            weapon = ItemDatabase.instance.GetWeaponItemByID(saveData.rightHandWeaponID_01);
            if (weapon == null)
                weapon = ItemDatabase.instance.unarmedWeapon;
            playerInventoryManager.RightHandWeaponScriptables[0] = weapon;

            weapon = ItemDatabase.instance.GetWeaponItemByID(saveData.rightHandWeaponID_02);
            if (weapon == null)
                weapon = ItemDatabase.instance.unarmedWeapon;
            playerInventoryManager.RightHandWeaponScriptables[1] = weapon;

            weapon = ItemDatabase.instance.GetWeaponItemByID(saveData.rightHandWeaponID_03);
            if (weapon == null)
                weapon = ItemDatabase.instance.unarmedWeapon;
            playerInventoryManager.RightHandWeaponScriptables[2] = weapon;

            // Equipped weapons
            playerNetwork.networkLeftHandWeaponID.Value =
                playerInventoryManager.LeftHandWeaponScriptables[saveData.leftHandWeaponQuickSlotIndex].ID;
            playerNetwork.networkRightHandWeaponID.Value = 
                playerInventoryManager.RightHandWeaponScriptables[saveData.rightHandWeaponQuickSlotIndex].ID;

            // Armor
            HeadArmorItem headArmor = ItemDatabase.instance.GetHeadArmorItemByID(saveData.headArmorID);
            if (headArmor != null)
                headArmor = Instantiate(headArmor);
            playerEquipmentManager.EquipHeadArmor(headArmor);

            ChestArmorItem chestArmor = ItemDatabase.instance.GetChestArmorItemByID(saveData.chestArmorID);
            if (chestArmor != null)
                chestArmor = Instantiate(chestArmor);
            playerEquipmentManager.EquipChestArmor(chestArmor);

            HandArmorItem handArmor = ItemDatabase.instance.GetHandArmorItemByID(saveData.handArmorID);
            if (handArmor != null)
                handArmor = Instantiate(handArmor);
            playerEquipmentManager.EquipHandArmor(handArmor);

            LegArmorItem legArmor = ItemDatabase.instance.GetLegArmorItemByID(saveData.legArmorID);
            if (legArmor != null)
                legArmor = Instantiate(legArmor);
            playerEquipmentManager.EquipLegArmor(legArmor);

            // INVENTORY
            foreach (int itemID in saveData.itemsInInventoryIDs)
                playerInventoryManager.AddItemToInventory(ItemDatabase.instance.GetItemByID(itemID));
        }

        public void ExportSaveData(CharacterSaveData saveData)
        {
            // CHARACTER INFORMATION
            saveData.characterName = playerNetwork.networkPlayerName.Value.ToString();
            saveData.isMale = playerNetwork.networkIsMale.Value;

            // STATS
            saveData.enduranceLevel = playerNetwork.networkEndurance.Value;
            saveData.vitalityLevel = playerNetwork.networkVitality.Value;
            saveData.strengthLevel = playerNetwork.networkStrength.Value;

            // RESOURCES
            saveData.currentStamina = playerNetwork.networkCurrentStamina.Value;
            saveData.currentHealth = playerNetwork.networkCurrentHealth.Value;

            // TRANSFORM
            saveData.SavePosition(transform.position);

            // WEAPON QUICK SLOTS
            saveData.leftHandWeaponID_01 = playerInventoryManager.LeftHandWeaponScriptables[0].ID;
            saveData.leftHandWeaponID_02 = playerInventoryManager.LeftHandWeaponScriptables[1].ID;
            saveData.leftHandWeaponID_03 = playerInventoryManager.LeftHandWeaponScriptables[2].ID;
            saveData.rightHandWeaponID_01 = playerInventoryManager.RightHandWeaponScriptables[0].ID;
            saveData.rightHandWeaponID_02 = playerInventoryManager.RightHandWeaponScriptables[1].ID;
            saveData.rightHandWeaponID_03 = playerInventoryManager.RightHandWeaponScriptables[2].ID;

            // EQUPPED WEAPONS
            saveData.leftHandWeaponQuickSlotIndex = playerInventoryManager.LeftHandWeaponIndex;
            saveData.rightHandWeaponQuickSlotIndex = playerInventoryManager.RightHandWeaponIndex;

            // ARMOR
            saveData.headArmorID = playerNetwork.networkArmorHelmetID.Value;
            saveData.chestArmorID = playerNetwork.networkArmorChestplateID.Value;
            saveData.handArmorID = playerNetwork.networkArmorGauntletsID.Value;
            saveData.legArmorID = playerNetwork.networkArmorLegginsID.Value;

            // INVENTORY
            foreach (Item item in playerInventoryManager.itemsInInventory)
                saveData.itemsInInventoryIDs.Add(item.ID);
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
