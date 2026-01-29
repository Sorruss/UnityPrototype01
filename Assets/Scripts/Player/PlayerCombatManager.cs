using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        private PlayerManager player;

        [HideInInspector] public bool CanDoComboMainHand = false;
        [HideInInspector] public bool CanDoComboOffHand = false;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public override void DisableAllDamageColliders()
        {
            base.DisableAllDamageColliders();

            player.playerEquipmentManager.LeftHandWeaponManager.ActivateDamageCollider(false);
            player.playerEquipmentManager.RightHandWeaponManager.ActivateDamageCollider(false);
        }

        // LOCK ON STUFF
        public override void SetCurrentTarget(CharacterManager target)
        {
            base.SetCurrentTarget(target);

            if (player.IsOwner)
            {
                PlayerCamera.instance.AdjustCameraHeight();
            }
        }

        // WEAPON ACTIONS
        public void TryToPerformWeaponAction(WeaponAction weaponAction, WeaponItem weapon)
        {
            // PERFORM ACTION ON THIS CLIENT.
            weaponAction.TryToPerformAction(player, weapon);

            if (player.IsOwner)
            { // NOTIFY ALL OTHER CLIENTS TO MIMIC THE SAME ACTION ON THIS CLIENT'S INSTANCE.
                player.playerNetwork.NotifyServerOfWeaponActionServerRpc(
                    NetworkManager.Singleton.LocalClientId, 
                    weaponAction.ActionID, 
                    weapon.ID);
            }
        }

        // ---------------------------------
        // ACTION ANIMATIONS - STAMINA DRAIN
        public void TryToDrainStaminaBasedOnAttackType()
        {
            // SINCE THIS METHOD IS RUN AS AN ANIMATION EVENT, IT'S GONNA BE RUN BY EVERYONE
            // BUT IT'S NEEDED TO DRAIN STAMINA ONLY ON ONE PLAYER INSTANCE SO THIS CHECK IS HERE
            if (!player.IsOwner)
                return;

            // AT THIS POINT THE VARIABLE 'currentWeaponItemBeingUsed' WAS SET FROM INSIDE WEAPONACTION
            // BUT IF NOT THEN THERE IS THIS CHECK BELOW
            if (currentWeaponItemBeingUsed == null)
                return;

            UpdateStaminaNeededForCurrentMove();
            player.playerStatsManager.TryDecreaseStamina(staminaNeededForCurrentAction);
        }

        // ACTION ANIMATIONS - COMBO EVENTS
        public override void EnableCanDoCombo()
        {
            if (player.playerNetwork.networkIsUsingLeftHand.Value)
            {
                player.playerCombatManager.CanDoComboOffHand = true;
            }
            else if (player.playerNetwork.networkIsUsingRightHand.Value)
            {
                player.playerCombatManager.CanDoComboMainHand = true;
            }
        }

        public override void DisableCanDoCombo()
        {
            player.playerCombatManager.CanDoComboMainHand = false;
            player.playerCombatManager.CanDoComboOffHand = false;
        }
    }
}
