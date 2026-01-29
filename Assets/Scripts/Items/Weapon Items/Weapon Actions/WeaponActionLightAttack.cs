using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class WeaponActionLightAttack : WeaponAction
    {
        [Header("Animations - Light Attacks")]
        public string MainLightAttack01Anim = "main_light_attack_01";
        public string MainLightAttack02Anim = "main_light_attack_02";

        [Header("Animations - Run Attacks")]
        public string MainRunAttack01Anim = "main_run_attack_01";

        [Header("Animations - Roll Attacks")]
        public string MainRollAttack01Anim = "main_roll_attack_01";

        [Header("Animations - Backstep Attacks")]
        public string MainBackstepAttack01Anim = "main_backstep_attack_01";

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

            // CHOOSE WHICH ATTACK TYPE TO DO (RUNNING, ROLLING, ETC.)
            if (player.playerNetwork.networkIsSprinting.Value)
                DoRunningAttack(player, weapon);
            else if (player.playerCombatManager.isAllowedToDoRollAttack)
                DoRollingAttack(player, weapon);
            else if (player.playerCombatManager.isAllowedToDoBackstepAttack)
                DoBackstepAttack(player, weapon);
            else
                DoLightAttack(player, weapon);
        }

        // ------------
        // ATTACK TYPES
        private void DoLightAttack(PlayerManager player, WeaponItem weapon)
        {
            if (player.isPerfomingAction && player.playerCombatManager.CanDoComboMainHand)      // COMBO LOGIC
            {
                player.playerCombatManager.CanDoComboMainHand = false;

                if (player.playerCombatManager.lastAttackAnimationPerfomed == MainLightAttack01Anim)
                {
                    player.characterAnimatorManager.PerformAttackAnimationAction(
                        MainLightAttack02Anim,
                        true,
                        WeaponMeleeAttackType.LIGHT_ATTACK_02);
                }
                else if (player.playerCombatManager.lastAttackAnimationPerfomed == MainLightAttack02Anim)
                {
                    player.characterAnimatorManager.PerformAttackAnimationAction(
                        MainLightAttack01Anim,
                        true,
                        WeaponMeleeAttackType.LIGHT_ATTACK_01);
                }
            }
            else if (!player.isPerfomingAction)     // FIRST ATTACK LOGICs
            {
                player.characterAnimatorManager.PerformAttackAnimationAction(
                    MainLightAttack01Anim, 
                    true,
                    WeaponMeleeAttackType.LIGHT_ATTACK_01);
            }
        }
    
        private void DoRunningAttack(PlayerManager player, WeaponItem weapon)
        {
            player.characterAnimatorManager.PerformAttackAnimationAction(
                MainRunAttack01Anim,
                true,
                WeaponMeleeAttackType.RUN_ATTACK_01);
        }

        private void DoRollingAttack(PlayerManager player, WeaponItem weapon)
        {
            player.playerCombatManager.DontAllowToDoRollAttack();
            player.characterAnimatorManager.PerformAttackAnimationAction(
                MainRollAttack01Anim,
                true,
                WeaponMeleeAttackType.ROLL_ATTACK_01);
        }

        private void DoBackstepAttack(PlayerManager player, WeaponItem weapon)
        {
            player.playerCombatManager.DontAllowToDoBackstepAttack();
            player.characterAnimatorManager.PerformAttackAnimationAction(
                MainBackstepAttack01Anim,
                true,
                WeaponMeleeAttackType.BACKSTEP_ATTACK_01);
        }
    }
}
