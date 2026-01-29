using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class WeaponActionHeavyAttack : WeaponAction
    {
        [Header("Animations")]
        public string MainHeavyAttack01Anim = "main_heavy_attack_01_start";
        public string MainHeavyAttack02Anim = "main_heavy_attack_02_start";

        public override void TryToPerformAction(PlayerManager player, WeaponItem weapon)
        {
            base.TryToPerformAction(player, weapon);

            if (!player.IsOwner)
                return;

            // DO STOPS.
            if (player.playerNetwork.networkCurrentStamina.Value <= 0)
                return;

            if (!player.characterLocomotionManager.isGrounded)
                return;

            DoHeavyAttack(player, weapon);
        }

        private void DoHeavyAttack(PlayerManager player, WeaponItem weapon)
        {
            if (player.isPerfomingAction && player.playerCombatManager.CanDoComboMainHand)  // COMBO LOGIC
            {
                player.playerCombatManager.CanDoComboMainHand = false;

                if (player.playerCombatManager.lastAttackAnimationPerfomed == MainHeavyAttack01Anim)
                {
                    player.playerAnimatorManager.PerformAttackAnimationAction(
                    MainHeavyAttack02Anim,
                    true,
                    WeaponMeleeAttackType.HEAVY_ATTACK_02);
                }
                else if (player.playerCombatManager.lastAttackAnimationPerfomed == MainHeavyAttack02Anim)
                {
                    player.playerAnimatorManager.PerformAttackAnimationAction(
                    MainHeavyAttack01Anim,
                    true,
                    WeaponMeleeAttackType.HEAVY_ATTACK_01);
                }
            }
            else if (!player.isPerfomingAction)   // FIRST ATTACK LOGIC
            {
                player.playerAnimatorManager.PerformAttackAnimationAction(
                    MainHeavyAttack01Anim,
                    true,
                    WeaponMeleeAttackType.HEAVY_ATTACK_01);
            }
        }
    }
}
