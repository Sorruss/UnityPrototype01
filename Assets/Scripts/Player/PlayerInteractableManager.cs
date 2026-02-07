using UnityEngine;
using System.Collections.Generic;

namespace FG
{
    public class PlayerInteractableManager : MonoBehaviour
    {
        private PlayerManager player;

        [Header("Debug - Interactables")]
        [SerializeField] private List<Interactable> interactables;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();

            interactables = new List<Interactable>();
        }

        private void FixedUpdate()
        {
            if (!player.IsOwner)
                return;

            if (PlayerUIManager.instance.isPopUpOpened)
                return;

            if (interactables.Count <= 0)
                return;

            if (interactables[0] == null)
            {
                CleanupInteractables();
                return;
            }

            PlayerUIManager.instance.popUpManager.SendInteractablePopUp(interactables[0].GetInteractableText());
            PlayerUIManager.instance.isPopUpOpened = true;
        }

        public void Interact()
        {
            if (!player.IsOwner)
                return;

            PlayerUIManager.instance.popUpManager.CloseAllPopUps();

            if (interactables.Count <= 0)
                return;

            if (interactables[0] == null)
                return;

            interactables[0].Interact(player);
            RemoveInteractable(interactables[0]);
        }

        // -----------------------------
        // INTERACTABLES LIST MANAGEMENT
        private void CleanupInteractables()
        {
            for (int i = interactables.Count - 1; i >= 0; --i)
            {
                if (interactables[i] != null)
                    continue;

                interactables.RemoveAt(i);
            }
        }

        public void AddInteractable(Interactable interactable)
        {
            CleanupInteractables();

            if (interactables.Contains(interactable))
                return;

            interactables.Add(interactable);
            PlayerUIManager.instance.popUpManager.UpdateInteractablePopUpQuantityText(interactables.Count);
        }

        public void RemoveInteractable(Interactable interactable)
        {
            if (!interactables.Contains(interactable))
                return;

            interactables.Remove(interactable);
            CleanupInteractables();

            PlayerUIManager.instance.popUpManager.UpdateInteractablePopUpQuantityText(interactables.Count);
        }
    }
}
