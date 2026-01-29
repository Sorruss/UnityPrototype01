using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "AI/Action/Attack")]
    public class AICharacterAttackAction : ScriptableObject
    {
        [Header("Animation")]
        public string animationName;

        [Header("Attack Params")]
        public WeaponMeleeAttackType attackType;
        public float attackWeight = 25.0f;              // Weight (Attack Probability)
        public float actionRecoveryTime = 2.0f;         // Cooldown before next action
        public AICharacterAttackAction nextComboAttack; // Attack in combo after this one

        [Header("Distance Params")]
        public float minimumAttackDistance = 1.0f;
        public float maximumAttackDistance = 2.0f;

        [Header("FOV Params")]
        public float minimumFOVNeeded = -30.0f;
        public float maximumFOVNeeded = 30.0f;

        public void TryToPerformAttackAction(AICharacterManager aiCharacterManager)
        {
            aiCharacterManager.aiCharacterAnimatorManager.PerformAttackAnimationAction(animationName, true, attackType);
        }
    }
}
