using UnityEngine;

namespace FG
{
    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(AICharacterManager aiCharacter)
        {
            return this;
        }

        public virtual AIState SwitchState(AICharacterManager aiCharacter, AIState nextState)
        {
            ResetStateFlags(aiCharacter);
            return nextState;
        }

        protected virtual void ResetStateFlags(AICharacterManager aiCharacter)
        {

        }
    }
}
