using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace FG
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        private PlayerManager player;

        // CHARACTER INFORMATION
        [HideInInspector] public NetworkVariable<FixedString64Bytes> networkPlayerName = 
            new("Character", 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);

        // EQUIPMENT INFORMATION
        public NetworkVariable<int> networkLeftHandWeaponID = 
            new(0,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> networkRightHandWeaponID = 
            new(0,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        // WEAPON ACTIONS INFORMATION
        public NetworkVariable<bool> networkIsUsingRightHand = 
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsUsingLeftHand = 
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> networkCurrentWeaponInUseID = 
            new(0,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        // TWO HANDING INFORMATION
        public NetworkVariable<int> networkTwoHandWeaponID =
            new(0,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsTwoHanding =
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsTwoHandingLeftWeapon =
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsTwoHandingRightWeapon =
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        // ----------------------------
        // "ON VALUE CHANGED" FUNCTIONS
        // STATS
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

        // EQUIPMENT INFORMATION
        public void OnLeftHandWeaponIDChanged(int prevID, int newID)
        {
            WeaponItem newWeapon = ItemDatabase.instance.GetWeaponItemByID(newID);

            player.playerEquipmentManager.UnloadLeftWeapon();
            player.playerInventoryManager.LeftHandWeaponScriptable = newWeapon;
            player.playerEquipmentManager.LoadLeftWeapon();

            if (player.IsOwner)
                PlayerUIManager.instance.hudManager.SetQuickSlotLeftWeaponSprite(newID);
        }

        public void OnRightHandWeaponIDChanged(int prevID, int newID)
        {
            WeaponItem newWeapon = ItemDatabase.instance.GetWeaponItemByID(newID);

            player.playerInventoryManager.RightHandWeaponScriptable = newWeapon;
            player.playerEquipmentManager.UnloadRightWeapon();
            player.playerEquipmentManager.LoadRightWeapon();

            if (player.IsOwner)
            {
                PlayerUIManager.instance.hudManager.SetQuickSlotRightWeaponSprite(newID);
            }
        }

        // WEAPON ACTIONS INFORMATION
        public void OnCurrentWeaponInUseIDChanged(int prevID, int newID)
        {
            WeaponItem newWeapon = ItemDatabase.instance.GetWeaponItemByID(newID);
            player.playerCombatManager.currentWeaponItemBeingUsed = newWeapon;

            if (player.IsOwner)
                return;

            player.playerAnimatorManager.UpdateAnimatorOverrider(newWeapon.animatorOverriderOH);
        }

        // FLAGS
        public override void OnIsBlockingChanged(bool oldValue, bool newValue)
        {
            base.OnIsBlockingChanged(oldValue, newValue);

            if (!player.IsOwner)
                return;

            if (player.playerInventoryManager.LeftHandWeaponScriptable == null)
                return;

            player.playerStatsManager.damageAbsorbtionPhysical = 
                player.playerInventoryManager.LeftHandWeaponScriptable.physicalDamageAbsorbtion;
            player.playerStatsManager.damageAbsorbtionMagic =
                player.playerInventoryManager.LeftHandWeaponScriptable.magicDamageAbsorbtion;
            player.playerStatsManager.damageAbsorbtionFire =
                player.playerInventoryManager.LeftHandWeaponScriptable.fireDamageAbsorbtion;
            player.playerStatsManager.damageAbsorbtionLightning =
                player.playerInventoryManager.LeftHandWeaponScriptable.lightningDamageAbsorbtion;
            player.playerStatsManager.damageAbsorbtionHoly =
                player.playerInventoryManager.LeftHandWeaponScriptable.holyDamageAbsorbtion;

            player.playerStatsManager.stability = player.playerInventoryManager.LeftHandWeaponScriptable.stability;
        }

        // TWO HANDING INFORMATION
        public void OnIsTwoHandingChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                // TWO HANDING IS ACTIVE
                TwoHandingStaticEffect effect = Instantiate(EffectsManager.instance.twoHandingEffect);
                player.playerEffectsManager.ApplyStaticEffect(effect);
            }
            else
            {
                // TWO HANDING IS NOT ACTIVE
                player.playerEffectsManager.RemoveStaticEffect(EffectsManager.instance.twoHandingEffect.staticEffectID);
                player.playerEquipmentManager.UnTwoHandWeapon();        // ORDER IS IMPORTANT

                if (player.IsOwner)
                {
                    networkIsTwoHandingLeftWeapon.Value = false;
                    networkIsTwoHandingRightWeapon.Value = false;
                }
            }
        }

        public void OnIsTwoHandingLeftWeaponChanged(bool oldValue, bool newValue)
        {
            if (!newValue)
                return;

            if (player.IsOwner)
            {
                networkIsTwoHanding.Value = true;
                networkTwoHandWeaponID.Value = networkLeftHandWeaponID.Value;
            }

            player.playerInventoryManager.TwoHandedWeaponScriptable = player.playerInventoryManager.LeftHandWeaponScriptable;
            player.playerEquipmentManager.TwoHandLeftWeapon();
        }

        public void OnIsTwoHandingRightWeaponChanged(bool oldValue, bool newValue)
        {
            if (!newValue)
                return;

            if (player.IsOwner)
            {
                networkIsTwoHanding.Value = true;
                networkTwoHandWeaponID.Value = networkRightHandWeaponID.Value;
            }

            player.playerInventoryManager.TwoHandedWeaponScriptable = player.playerInventoryManager.RightHandWeaponScriptable;
            player.playerEquipmentManager.TwoHandRightWeapon();
        }

        // -----------------
        // SUPPORT FUNCTIONS
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

        // ------------------
        // WEAPON ACTION RPCs
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

            player.playerAnimatorManager.UpdateAnimatorOverrider(weapon.animatorOverriderOH);
            action.TryToPerformAction(player, weapon);
        }
    }
}
