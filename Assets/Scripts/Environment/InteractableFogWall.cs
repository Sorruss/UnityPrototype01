using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class InteractableFogWall : Interactable
    {
        [Header("Config")]
        public BossID relatedBossID;
        [SerializeField] private Collider hardCollider;
        [SerializeField] private GameObject[] fogObjects;

        [Header("Network Variables")]
        public NetworkVariable<bool> networkIsActive = 
            new(false, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);

        // NETWORK SPAWN/DESPAWN
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            OnIsActiveChanged(false, networkIsActive.Value);
            networkIsActive.OnValueChanged += OnIsActiveChanged;
        
            NetworkObjectsManager.instance.AddFogWall(this);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            networkIsActive.OnValueChanged -= OnIsActiveChanged;

            NetworkObjectsManager.instance.RemoveFogWall(this);
        }

        // NETWORK VARIABLES LISTENERS
        private void OnIsActiveChanged(bool oldValue, bool newValue)
        {
            foreach (var obj in fogObjects)
                obj.SetActive(newValue);
        }

        // INTERACTABLE LOGIC
        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            if (!player.IsOwner)
                return;

            // MAKE PLAYER FACE THE WALL
            player.transform.rotation = Quaternion.LookRotation(transform.forward);

            // DISABLE COLLISION WITH THE WALL
            // PLAY WALKTHROUGH WALL ANIMATION
            // ENABLE COLLISION WITH THE WALL
            // DO SO EVERY OTHER PLAYER SEES US GOING THROUGH WALL WITH ITS COLLISION DISABLED SO
            // WE DON'T JUST WALK FORWARD AND TELEPORT ON THEIR SCREEN
            player.playerAnimatorManager.PerformAnimationAction("interact_fogWall_01", true);
            AllowPlayerToPassThroughFogWallServerRpc(player.NetworkObjectId);
        }

        private IEnumerator PlayAnimationAndWaitTillItFinishes(PlayerManager player)
        {
            Physics.IgnoreCollision(player.characterController, hardCollider, true);
            yield return new WaitForSeconds(2.5f);
            Physics.IgnoreCollision(player.characterController, hardCollider, false);
        }

        // RPCs
        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        private void AllowPlayerToPassThroughFogWallServerRpc(ulong playerNetworkID)
        {
            if (IsServer)
                AllowPlayerToPassThroughFogWallClientRpc(playerNetworkID);
        }

        [ClientRpc]
        private void AllowPlayerToPassThroughFogWallClientRpc(ulong playerNetworkID)
        {
            AllowPlayerToPassThroughFogWall(playerNetworkID);
        }

        private void AllowPlayerToPassThroughFogWall(ulong playerNetworkID)
        {
            PlayerManager player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerNetworkID].GetComponent<PlayerManager>();
            if (player == null)
                return;

            StartCoroutine(PlayAnimationAndWaitTillItFinishes(player));
        }
    }
}
