using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Item/Weapon Art/Parry")]
    public class ParryWeaponArt : WeaponArtItem
    {
        public override void PerformWeaponArt(PlayerManager player)
        {
            base.PerformWeaponArt(player);

            // 1. PLAY PARRY ANIMATION
            player.playerAnimatorManager.PerformInstantAnimationAction(
                GetParryAnimationBasedOnWeaponClass(player.playerCombatManager.currentWeaponItemBeingUsed.weaponClass), 
                true);

            // 2. 

        }

        protected override bool CanPerformThisWeaponArt(PlayerManager player)
        {
            if (player.isPerfomingAction)
                return false;

            if (!player.playerLocomotion.isGrounded)
                return false;

            if (player.playerNetwork.networkIsJumping.Value)
                return false;

            return true;
        }

        private string GetParryAnimationBasedOnWeaponClass(WeaponClass weaponClass)
        {
            string parryAnimation = null;

            switch (weaponClass)
            {
                case WeaponClass.STRAIGHT_SWORD:
                    break;
                case WeaponClass.SHIELD:
                    parryAnimation = "Parry_Slow_01";
                    break;
                case WeaponClass.FIST:
                    parryAnimation = "Parry_Fast_01";
                    break;
                case WeaponClass.LIGHT_SHIELD:
                    parryAnimation = "Parry_Medium_01";
                    break;
            }

            return parryAnimation;
        }
    }
}
