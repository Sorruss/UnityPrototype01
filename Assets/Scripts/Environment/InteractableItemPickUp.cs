using UnityEngine;

namespace FG
{
    public class InteractableItemPickUp : Interactable
    {
        [Header("Config - Item Pick Up")]
        [SerializeField] private int ID;
        [SerializeField] private ItemPickUpType itemPickUpType;
        [SerializeField] private Item item;
        [SerializeField] private int amount = 1;

        protected override void Start()
        {
            base.Start();

            if (itemPickUpType == ItemPickUpType.WORLD_ITEM)
                CheckIfWorldItemWasPickedUpBefore();
        }

        private void CheckIfWorldItemWasPickedUpBefore()
        {
            // IF IT HAS THIS KEY
            if (SaveGameManager.instance.currentSaveData.worldItemsIDs.ContainsKey(ID))
            {
                // IF ITEM WAS PICKED UP
                if (SaveGameManager.instance.currentSaveData.worldItemsIDs[ID] == true)
                {
                    // DESTROY ITEM
                    Destroy(gameObject);
                }
            }
            // IF IT DOESN'T HAVE THIS KEY
            else
            {
                // ADD KEY
                SaveGameManager.instance.currentSaveData.worldItemsIDs.Add(ID, false);
            }
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            // IF IT'S WORLD ITEM, WE NEED TO REMEMBER IT BEING PICKED UP
            if (itemPickUpType == ItemPickUpType.WORLD_ITEM)
            {
                // IF IT HAS THIS KEY -> REMOVE THIS KEY
                if (SaveGameManager.instance.currentSaveData.worldItemsIDs.ContainsKey(ID))
                    SaveGameManager.instance.currentSaveData.worldItemsIDs.Remove(ID);
                
                // ADD THIS KEY AS TRUE
                SaveGameManager.instance.currentSaveData.worldItemsIDs.Add(ID, true);
            }

            // PLAY SOUND
            if (interactedSoundFX != null)
                player.playerSFXManager.PlayAudioClip(SFXManager.instance.itemPickUpSoundFX);

            // SEND POP UP
            PlayerUIManager.instance.popUpManager.SendItemPickedUpPopUp(item, amount);
            
            // ADD ITEM TO INVENTORY
            player.playerInventoryManager.AddItemToInventory(item);

            // DESTROY
            Destroy(gameObject);
        }
    }
}
