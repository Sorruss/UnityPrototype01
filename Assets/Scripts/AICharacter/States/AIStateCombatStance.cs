using UnityEngine;
using System.Collections.Generic;

namespace FG
{
    [CreateAssetMenu(menuName = "AI/State/Combat Stance")]
    public class AIStateCombatStance : AIState
    {
        [Header("Attacks")]
        [SerializeField] private AICharacterAttackAction[] attacks;     // COMPLETE PULL OF ALL ATTACKS
        private List<AICharacterAttackAction> attacksSorted;            // ATTACKS WHICH ARE USABLE IN CURRENT SITUATION (distance, angle etc.)

        [Header("Combo")]
        [SerializeField] private float chanceToDoCombo = 25.0f;
        private bool canDoCombo = false;

        private bool hasAttack = false;
        private AICharacterAttackAction attack;
        private AICharacterAttackAction lastAttack;

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerfomingAction)
                return this;

            // ENABLE NAVMESH SAFEGUARD
            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            // STATE SWITCHERS
            if (aiCharacter.aiCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idleState);

            if (aiCharacter.aiCombatManager.distanceToTarget >= aiCharacter.aiCombatManager.combatStateRadius)
                return SwitchState(aiCharacter, aiCharacter.pursueTargetState);

            // ROTATE/PIVOT AI_CHARATER TOWARDS THE TARGET
            if (aiCharacter.aiCombatManager.doPivot && aiCharacter.aiCombatManager.IsAngleOutOfFOV(aiCharacter.aiCombatManager.angleToTarget))
                aiCharacter.aiCharacterLocomotionManager.PivotTowardsTarget(aiCharacter);
            else
                aiCharacter.aiCharacterLocomotionManager.RotateTowardsTarget(aiCharacter);

            // PATH BUILDING
            aiCharacter.navMeshAgent.SetDestination(aiCharacter.aiCombatManager.currentTarget.transform.position);

            // FIND SUITABLE ATTACK
            if (!hasAttack)
                TryToFindAttack(aiCharacter);

            if (hasAttack)
            {
                if (chanceToDoCombo > Random.Range(1, 101))
                    canDoCombo = true;

                aiCharacter.attackState.canDoCombo = canDoCombo;
                aiCharacter.attackState.currentAttack = attack;
                return SwitchState(aiCharacter, aiCharacter.attackState);
            }

            return this;
        }

        protected override void ResetStateFlags(AICharacterManager aiCharacter)
        {
            base.ResetStateFlags(aiCharacter);

            hasAttack = false;
            canDoCombo = false;
        }

        private void TryToFindAttack(AICharacterManager aiCharacter)
        {
            attacksSorted = new List<AICharacterAttackAction>();

            // 1. REMOVE NOT SUITED ATTACKS
            foreach (var attack in attacks)
            {
                // MINIMUM DISTANCE CHECK
                if (attack.minimumAttackDistance > aiCharacter.aiCombatManager.distanceToTarget)
                    continue;

                // MAXIMUM DISTANCE CHECK
                if (attack.maximumAttackDistance < aiCharacter.aiCombatManager.distanceToTarget)
                    continue;

                // MINIMUM FOV CHECK
                if (attack.minimumFOVNeeded > aiCharacter.aiCombatManager.angleToTarget)
                    continue;

                // MAXIMUM FOV CHECK
                if (attack.maximumFOVNeeded < aiCharacter.aiCombatManager.angleToTarget)
                    continue;

                attacksSorted.Add(attack);
            }

            if (attacksSorted.Count <= 0)
                return;

            // 2. GET RANDOM ATTACK
            float totalWeight = 0.0f;
            foreach (var attack in attacksSorted)
                totalWeight += attack.attackWeight;

            float randomWeight = Random.Range(1, totalWeight + 1);
            totalWeight = 0.0f;
            foreach (var attack in attacksSorted)
            {
                totalWeight += attack.attackWeight;

                // 3. ASSIGN THE ATTACK
                if (totalWeight >= randomWeight)
                {
                    this.attack = attack;
                    lastAttack = this.attack;
                    hasAttack = true;
                    return;
                }
            }

            hasAttack = false;
        }
    }
}
