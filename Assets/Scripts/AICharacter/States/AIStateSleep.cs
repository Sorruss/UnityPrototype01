using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "AI/State/Sleep")]
    public class AIStateSleep : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            return this;
        }
    }
}
