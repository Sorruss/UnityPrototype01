using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "AI/State/Attack")]
    public class AIStateAttack : AIState
    {
        [HideInInspector] public AICharacterAttackAction currentAttack;
        [HideInInspector] public bool canDoCombo = false;

        private bool hasPerformedAttack = false;
        private bool hasPerformedCombo = false;

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            // STATE SWITCHES
            if (aiCharacter.aiCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idleState);

            if (aiCharacter.aiCombatManager.currentTarget.characterNetwork.networkIsDead.Value)
            {
                aiCharacter.aiCombatManager.SetCurrentTarget(null);
                return SwitchState(aiCharacter, aiCharacter.idleState);
            }

            if (!aiCharacter.isPerfomingAction && aiCharacter.aiCombatManager.distanceToTarget > aiCharacter.aiCombatManager.combatStateRadius)
                return SwitchState(aiCharacter, aiCharacter.pursueTargetState);

            // RESET MOVEMENT VALUES TO 0 JUST IN CASE
            aiCharacter.aiCharacterAnimatorManager.UpdateMovementValues(0.0f, 0.0f);

            // ROTATION
            aiCharacter.aiCharacterLocomotionManager.RotateTowardsTarget(aiCharacter);

            // COMBO
            if (canDoCombo && !hasPerformedCombo && currentAttack.nextComboAttack != null)
            {
                //hasPerformedCombo = true;
                //currentAttack.nextComboAttack.TryToPerformAttackAction(aiCharacter);
            }

            if (aiCharacter.isPerfomingAction)
                return this;

            // ATTACK
            if (!hasPerformedAttack)
            {
                if (aiCharacter.aiCombatManager.actionRecoveryTime > 0.0f)
                    return this;    // RETURN TO THE TOP TO WAIT FOR THE RECOVERY TIME
                
                PerformAnAttack(aiCharacter);
                return this;    // RETURN TO THE TOP SO MAYBE WE PERFORM COMBO
            }

            // IF WE DID EVERYTHING WE COULD WITH CURRENTATTACK, GO BACK TO THE STANCE
            return SwitchState(aiCharacter, aiCharacter.combatStanceState);
        }

        private void PerformAnAttack(AICharacterManager aiCharacter)
        {
            hasPerformedAttack = true;
            aiCharacter.aiCombatManager.actionRecoveryTime = currentAttack.actionRecoveryTime;
            currentAttack.TryToPerformAttackAction(aiCharacter);
        }

        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);

            canDoCombo = false;
            hasPerformedAttack = false;
            hasPerformedCombo = false;
        }
    }
}
