using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class Interactable : NetworkBehaviour
    {
        [Header("Config")]
        [SerializeField] private Collider interactableCollider;
        [SerializeField] private string interactableText = "Interact With Me";
        [SerializeField] private bool onlyHostAllowedToInteract = true;
        [SerializeField] private bool destroyOnInteraction = false;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip interactedSoundFX;

        // ------
        // EVENTS
        private void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                return;

            if (!player.IsOwner)
                return;

            if (onlyHostAllowedToInteract && !player.IsHost)
                return;

            player.playerInteractableManager.AddInteractable(this);
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();

            if (player == null)
                return;

            if (!player.IsOwner)
                return;

            if (onlyHostAllowedToInteract && !player.IsHost)
                return;

            player.playerInteractableManager.RemoveInteractable(this);
        }

        // --------------
        // NETWORK EVENTS
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }

        // -----
        // LOGIC
        public virtual void Interact(PlayerManager player)
        {
            if (player == null)
                return;

            if (!player.IsOwner)
                return;

            // PLAY SOUND
            if (audioSource != null)
                audioSource.PlayOneShot(interactedSoundFX);

            // DESTROY
            if (destroyOnInteraction)
                Destroy(gameObject);
        }

        public virtual string GetInteractableText() => interactableText;

        protected void RetriggerCollider()
        {
            interactableCollider.enabled = false;
            interactableCollider.enabled = true;
        }
    }
}
