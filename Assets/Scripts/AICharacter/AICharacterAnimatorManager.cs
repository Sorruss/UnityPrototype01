using UnityEngine;

namespace FG
{
    public class AICharacterAnimatorManager : CharacterAnimatorManager
    {
        private void OnAnimatorMove()
        {
            if (!character.characterLocomotionManager.isGrounded)
                return;

            // BECAUSE THE AI_CHARACTER IS MOVING USING ROOT_MOTION
            // ANIMATOR KINDA FIGHTING WITH NETWORK FOR THE POSITION OF AN AI_CHARACTER
            // SO THE BEST WE CAN DO IS TO BALANCE BETWEEN THEM IF USER IS NOT A SERVER

            // THAT'S THE SAME FOR EVERYONE - TO MOVE AI_CHARACTER BASED ON ANIMATOR
            // AND TO ROTATE CHARACTER BASED ON ANIMATOR
            character.characterController.Move(character.animator.deltaPosition);
            character.transform.rotation *= character.animator.deltaRotation;

            // CLIENT
            if (!character.IsOwner)
            {
                // TRYING TO SMOOTH OUT DIFFERENCE BETWEEN LOCAL AND NETWORK VALUES
                character.transform.position = Vector3.SmoothDamp(
                    transform.position,
                    character.characterNetwork.networkPosition.Value,
                    ref character.characterNetwork.networkPositionVelocity,
                    character.characterNetwork.networkPositionSmoothTime);
            }
        }
    }
}
