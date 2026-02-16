using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class InteractableItemPickUp : Interactable
    {
        [Header("Config - General")]
        [SerializeField] private int ID;
        [SerializeField] private ItemPickUpType itemPickUpType;
        public Item item;
        public int amount = 1;

        [Header("Config - Creature drop")]
        [HideInInspector] public Transform creaturePositionToTrack;
        [SerializeField] private bool shouldTrackCreature = true;

        [Header("Netcode for creature drop")]
        [HideInInspector] public NetworkVariable<Vector3> networkItemPosition = 
            new(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [HideInInspector] public NetworkVariable<ulong> networkCreatureToTrackID = 
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [HideInInspector] public NetworkVariable<int> networkItemID = 
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        // -------------
        // UNITY METHODS
        protected override void Start()
        {
            base.Start();

            if (itemPickUpType == ItemPickUpType.WORLD_ITEM)
            {
                if (NetworkManager.Singleton.IsServer)
                    CheckIfWorldItemWasPickedUpBefore();
                else
                    Destroy(gameObject);
            }
        }

        // -------
        // NETCODE
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (itemPickUpType != ItemPickUpType.TEMPORARY_ITEM)
                return;

            // ITEM IT HOLDS
            networkItemID.OnValueChanged += OnItemIDChanged;

            // CREATURE'S POSITION TO TRACK
            networkCreatureToTrackID.OnValueChanged += OnCreatureToTrackIDChanged;

            // ITEM'S POSITION
            networkItemPosition.OnValueChanged += OnItemPositionChanged;

            // NOT OWNER = NOT SERVER IN THIS EXAMPLE
            if (!IsOwner)
            {
                OnItemIDChanged(0, networkItemID.Value);
                OnCreatureToTrackIDChanged(0, networkCreatureToTrackID.Value);
                OnItemPositionChanged(Vector3.zero, networkItemPosition.Value);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            if (itemPickUpType != ItemPickUpType.TEMPORARY_ITEM)
                return;

            networkItemID.OnValueChanged -= OnItemIDChanged;
            networkCreatureToTrackID.OnValueChanged -= OnCreatureToTrackIDChanged;
            networkItemPosition.OnValueChanged -= OnItemPositionChanged;
        }

        private void OnItemIDChanged(int oldValue, int newValue)
        {
            item = ItemDatabase.instance.GetItemByID(newValue);
        }

        private void OnCreatureToTrackIDChanged(ulong oldValue, ulong newValue)
        {
            if (!shouldTrackCreature)
                return;

            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(newValue))
            {
                StartCoroutine(WaitOnNeededCreatureSpawnCoroutine(newValue));
                return;
            }    

            CharacterManager creature = NetworkManager.Singleton.SpawnManager.SpawnedObjects[newValue].gameObject.GetComponent<CharacterManager>();
            if (creature == null)
                return;

            creaturePositionToTrack = creature.characterCombatManager.lockOnTransform;
            StartCoroutine(TrackCreaturePositionCoroutine());
        }

        private void OnItemPositionChanged(Vector3 oldValue, Vector3 newValue)
        {
            transform.position = newValue;
        }

        // -------------------------
        // INTERACTABLE ITEM METHODS
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
                player.playerSFXManager.PlayAudioClip(SFXManager.instance.itemPickUpSFX);

            // PLAY ANIMATION
            player.playerAnimatorManager.PerformAnimationAction("interact_pickup_item_01", true);

            // SEND POP UP
            PlayerUIManager.instance.popUpManager.SendItemPickedUpPopUp(item, amount);
            
            // ADD ITEM TO INVENTORY
            player.playerInventoryManager.AddItemToInventory(item);

            // DESTROY
            NotifyServerOfItemInteractableDestroyedServerRpc(NetworkObjectId);
        }
    
        // ----------
        // COROUTINES
        private IEnumerator TrackCreaturePositionCoroutine()
        {
            while (shouldTrackCreature)
            {
                if (!gameObject.activeInHierarchy || creaturePositionToTrack == null)
                {
                    shouldTrackCreature = false;
                    break;
                }

                if (IsServer)
                    networkItemPosition.Value = creaturePositionToTrack.position;

                yield return null;
            }

            yield return null;
        }

        private IEnumerator WaitOnNeededCreatureSpawnCoroutine(ulong ID)
        {
            while (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.ContainsKey(ID))
            {
                yield return new WaitForFixedUpdate();
            }

            OnCreatureToTrackIDChanged(0, ID);
            yield return null;
        }

        // ----
        // RPCs
        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        private void NotifyServerOfItemInteractableDestroyedServerRpc(ulong interactableItemID)
        {
            if (IsServer)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}
