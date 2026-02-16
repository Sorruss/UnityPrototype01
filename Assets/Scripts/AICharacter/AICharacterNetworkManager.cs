using UnityEngine;

namespace FG
{
    public class AICharacterNetworkManager : CharacterNetworkManager
    {
        private AICharacterManager aiCharacter;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
        }

        public override void OnIsDeadChanged(bool oldValue, bool newValue)
        {
            base.OnIsDeadChanged(oldValue, newValue);

            if (newValue)
                aiCharacter.aiCharacterInventoryManager.DropItemOnDeath();
        }
    }
}
