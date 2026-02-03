using UnityEngine;

namespace FG
{
    public class OnActionFinished : StateMachineBehaviour
    {
        CharacterManager character;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
                character = animator.GetComponent<CharacterManager>();

            // IS PERFOMING ACTION / LOCOMOTION / ROOT MOTION
            character.isPerfomingAction = false;
            character.characterLocomotionManager.canRotate = true;
            character.characterLocomotionManager.canMove = true;
            character.characterAnimatorManager.applyRootMotion = false;

            // COMBAT STUFF
            character.characterCombatManager.DisableCanDoCombo();
            character.characterCombatManager.DontAllowToDoRollAttack();
            character.characterCombatManager.DontAllowToDoBackstepAttack();

            // DISABLE COLLIDERS JUST IN CASE
            character.characterCombatManager.DisableAllDamageColliders();

            // ALL THE NETWORK STUFF
            if (character.IsOwner)
            {
                character.characterNetwork.networkIsJumping.Value = false;
                character.characterNetwork.networkIsRolling.Value = false;
                character.characterNetwork.networkIsInvincible.Value = false;
                character.characterNetwork.networkIsAttacking.Value = false;
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
