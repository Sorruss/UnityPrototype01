using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class AICharacterInventoryManager : CharacterInventoryManager
    {
        private AICharacterManager aiCharacter;

        [Header("Item Drop Config")]
        [SerializeField] private bool doesDropItemOnDeath = true;
        [SerializeField] private Item[] itemsItCanDrop;
        [SerializeField] private float chanceToDrop = 0.1f;

        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
        }
    
        public void DropItemOnDeath()
        {
            // 1. CHECKS
            if (!doesDropItemOnDeath)
                return;

            // ONLY THE SERVER (OWNER OF AI) CAN SPAWN NETWORK OBJECTS
            if (!aiCharacter.IsOwner)
                return;

            // 2. ROLL THE CHANCE TO DROP AN ITEM
            float roll = Random.Range(0.0f, 1.0f);
            if (roll > chanceToDrop)
                return;

            // 3. GET RANDOM ITEM FROM INVENTORY
            Item randomItem = itemsItCanDrop[Random.Range(0, itemsItCanDrop.Length - 1)];

            // 4. CREATE INTERACTABLE_ITEM W/THIS ITEM
            GameObject interactableItem = Instantiate(ItemDatabase.instance.interactableItemPickUp);
            InteractableItemPickUp itemScript = interactableItem.GetComponent<InteractableItemPickUp>();

            // 5. SPAWN ITS NETWORK OBJECT
            interactableItem.GetComponent<NetworkObject>().Spawn();

            // 6. CONFIG THE NETWORK VARIABLES
            itemScript.networkItemID.Value = randomItem.ID;
            itemScript.networkCreatureToTrackID.Value = aiCharacter.NetworkObjectId;
            itemScript.networkItemPosition.Value = aiCharacter.transform.position;
        }
    }
}
