using UnityEngine;

namespace FG
{
    public class DurkLocomotionManager : AICharacterLocomotionManager
    {
        public override void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerfomingAction)
                return;

            float angle = aiCharacter.aiCombatManager.angleToTarget;

            if (angle >= 61.0f && angle <= 110.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_R_90", true);
            }
            else if (angle <= -61.0f && angle >= -110.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_L_90", true);
            }

            if (angle >= 145.0f && angle <= 180.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_L_180", true);
            }
            else if (angle <= -145.0f && angle >= -180.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_L_180", true);
            }
        }
    }
}
