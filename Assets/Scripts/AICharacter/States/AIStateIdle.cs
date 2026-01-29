using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "AI/State/Idle")]
    public class AIStateIdle : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCombatManager.currentTarget != null)
            {
                return SwitchState(aiCharacter, aiCharacter.pursueTargetState);
            }
            else
            {
                aiCharacter.aiCombatManager.TryToGetATarget(aiCharacter);
                return this;
            }
        }
    }
}
