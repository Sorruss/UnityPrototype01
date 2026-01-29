using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        private PlayerManager player;

        // CHARACTER INFORMATION.
        [HideInInspector] public NetworkVariable<FixedString64Bytes> networkPlayerName = 
            new("Character", 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);

        // EQUIPMENT INFORMATION.
        [Header("Equipment Information")]
        public NetworkVariable<int> networkLeftHandWeaponID = 
            new(0,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> networkRightHandWeaponID = 
            new(0,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        // WEAPON ACTIONS INFORMATION.
        [HideInInspector] public NetworkVariable<bool> networkIsUsingRightHand = 
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        [HideInInspector] public NetworkVariable<bool> networkIsUsingLeftHand = 
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> networkCurrentWeaponInUseID = 
            new(0,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        // "ON VALUE CHANGED" FUNCTIONS.
        public void OnEnduranceValueChanged(int prevEndurance, int newEndurance)
        {
            networkMaxStamina.Value = player.playerStatsManager.GetMaxStaminaOfEnduranceLevel(newEndurance);
            networkCurrentStamina.Value = networkMaxStamina.Value;
        }

        public void OnVitalityValueChanged(int prevVitality, int newVitality)
        {
            networkMaxHealth.Value = player.playerStatsManager.GetMaxHealthOfVitalityLevel(newVitality);
            networkCurrentHealth.Value = networkMaxHealth.Value;
        }

        public void OnLeftHandWeaponIDChanged(int prevID, int newID)
        {
            WeaponItem newWeapon = ItemDatabase.instance.GetWeaponItemByID(newID);

            player.playerInventoryManager.LeftHandWeaponSciptable = newWeapon;
            player.playerEquipmentManager.UnloadLeftWeapon();
            player.playerEquipmentManager.LoadLeftWeapon();

            if (player.IsOwner)
            {
                PlayerUIManager.instance.hudManager.SetQuickSlotLeftWeaponSprite(newID);
            }
        }

        public void OnRightHandWeaponIDChanged(int prevID, int newID)
        {
            WeaponItem newWeapon = ItemDatabase.instance.GetWeaponItemByID(newID);

            player.playerInventoryManager.RightHandWeaponSciptable = newWeapon;
            player.playerEquipmentManager.UnloadRightWeapon();
            player.playerEquipmentManager.LoadRightWeapon();

            if (player.IsOwner)
            {
                PlayerUIManager.instance.hudManager.SetQuickSlotRightWeaponSprite(newID);
            }
        }

        public void OnCurrentWeaponInUseIDChanged(int prevID, int newID)
        {
            WeaponItem newWeapon = ItemDatabase.instance.GetWeaponItemByID(newID);
            player.playerCombatManager.currentWeaponItemBeingUsed = newWeapon;
        }

        // SUPPORT FUNCTIONS.
        public void SetCurrentActiveHand(bool rightHand)
        {
            if (rightHand)
            {
                networkIsUsingRightHand.Value = true;
                networkIsUsingLeftHand.Value = false;
            }
            else // USING LEFT HAND.
            {
                networkIsUsingLeftHand.Value = true;
                networkIsUsingRightHand.Value = false;
            }
        }

        // WEAPON ACTION RPCs.
        [ServerRpc]
        public void NotifyServerOfWeaponActionServerRpc(ulong clientId, int actionId, int weaponId)
        {
            if (IsServer)
            {
                NotifyClientsOfWeaponActionClientRpc(clientId, actionId, weaponId);
            }
        }

        [ClientRpc]
        private void NotifyClientsOfWeaponActionClientRpc(ulong clientId, int actionId, int weaponId)
        {
            // WE CHECK THIS ID SO WE PLAY THE ANIMATION ON OTHER INSTANCES OF THIS PLAYER BESIDES THIS INSTANCE.
            if (NetworkManager.Singleton.LocalClientId != clientId)
            {
                ApplyWeaponAction(actionId, weaponId);
            }
        }

        private void ApplyWeaponAction(int actionId, int weaponId)
        {
            WeaponItem weapon = ItemDatabase.instance.GetWeaponItemByID(weaponId);
            WeaponAction action = ActionDatabase.instance.GetWeaponActionByID(actionId);

            action.TryToPerformAction(player, weapon);
        }
    }
}
