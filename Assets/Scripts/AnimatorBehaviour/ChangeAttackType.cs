using UnityEngine;

namespace FG
{
    public class Utility_ChangeAttackType : StateMachineBehaviour
    {
        [SerializeField] private WeaponMeleeAttackType type;
        private CharacterManager character;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                character = animator.GetComponent<CharacterManager>();
            }

            character.characterCombatManager.currentAttackTypeBeingUsed = type;
        }
    }
}
