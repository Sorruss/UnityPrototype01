using UnityEngine;

namespace FG
{
    public class AICharacterLocomotionManager : CharacterLocomotionManager
    {
        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 0.5f;

        // ------------------
        // ROTATING FUNCTIONS
        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            if (!aiCharacter.aiCharacterLocomotionManager.canRotate)
                return;

            aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }

        public void RotateTowardsTarget(AICharacterManager aiCharacter)
        {
            if (!aiCharacter.aiCharacterLocomotionManager.canRotate)
                return;

            // CONFIG DIRECTION PROPERLY
            Vector3 direction = aiCharacter.aiCombatManager.targetDirection;
            direction.y = 0.0f;
            direction.Normalize();

            if (direction == Vector3.zero)
                direction = aiCharacter.transform.forward;

            // DO ROTATION
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, rotationSpeed);
        }

        public virtual void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerfomingAction)
                return;

            float angle = aiCharacter.aiCombatManager.angleToTarget;

            if (angle >= 20.0f && angle <= 60.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_R_45", true);
            }
            else if (angle <= -20.0f && angle >= -60.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_L_45", true);
            }

            if (angle >= 61.0f && angle <= 110.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_R_90", true);
            }
            else if (angle <= -61.0f && angle >= -110.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_L_90", true);
            }

            if (angle >= 110.0f && angle <= 145.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_R_135", true);
            }
            else if (angle <= -110.0f && angle >= -145.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_L_135", true);
            }

            if (angle >= 145.0f && angle <= 180.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_R_180", true);
            }
            else if (angle <= -145.0f && angle >= -180.0f)
            {
                aiCharacter.aiCharacterAnimatorManager.PerformAnimationAction("turn_L_180", true);
            }
        }
    }
}
