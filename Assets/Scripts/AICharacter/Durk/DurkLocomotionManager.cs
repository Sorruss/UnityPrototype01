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

            if (angle >= 61.0f && angle <= 125.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_R_90", true);
                return;
            }
            else if (angle <= -61.0f && angle >= -125.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_L_90", true);
                return;
            }

            if (angle > 125.0f && angle <= 180.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_L_180", true);
                return;
            }
            else if (angle < -125.0f && angle >= -180.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_L_180", true);
                return;
            }
        }
    }
}
