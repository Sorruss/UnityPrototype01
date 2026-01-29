using UnityEngine;
using UnityEngine.AI;

namespace FG
{
    [CreateAssetMenu(menuName = "AI/State/Pursue Target")]
    public class AIStatePursue : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerfomingAction)
                return this;

            // STATE SWITCHES
            if (aiCharacter.aiCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idleState);

            if (aiCharacter.aiCombatManager.distanceToTarget < aiCharacter.navMeshAgent.stoppingDistance)
                return SwitchState(aiCharacter, aiCharacter.combatStanceState);

            // CHECK IF NAVMESH_AGENT IS ENABLED
            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            // ROTATION
            if (!aiCharacter.isPerfomingAction)
            {
                // DO PIVOTING IF POSSIBLE
                if (aiCharacter.aiCombatManager.doPivot && aiCharacter.aiCombatManager.IsAngleOutOfFOV(aiCharacter.aiCombatManager.angleToTarget))
                    aiCharacter.aiCharacterLocomotionManager.PivotTowardsTarget(aiCharacter);

                // ROTATE AI_CHARATER TOWARDS THE AGENT IF AI_CHARACTER IS MOVING TOWARDS TARGET
                else if (aiCharacter.characterNetwork.networkIsMoving.Value)
                    aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

                // ROTATE AI_CHARATER TOWARDS THE TARGET IF AI_CHARACTER IS NEAR TARGET
                else if (aiCharacter.aiCombatManager.distanceToTarget < aiCharacter.navMeshAgent.stoppingDistance)
                    aiCharacter.aiCharacterLocomotionManager.RotateTowardsTarget(aiCharacter);
            }

            // BUILD A PATH TOWARDS THE TARGET AND FOLLOW IT I GUESS
            // OPTION 1 : ASYNC
            aiCharacter.navMeshAgent.SetDestination(aiCharacter.aiCombatManager.currentTarget.transform.position);

            // OPTION 2 : SYNC
            //NavMeshPath path = new NavMeshPath();
            //aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCombatManager.currentTarget.transform.position, path);
            //aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }
    }
}
