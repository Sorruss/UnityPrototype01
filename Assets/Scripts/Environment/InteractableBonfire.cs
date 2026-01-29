using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class InteractableBonfire : Interactable
    {
        [Header("Config")]
        [SerializeField] private int bonfireID;
        [SerializeField] private string unactivatedPopUpText = "Activate Bonfire";
        [SerializeField] private GameObject[] objectsToActivateOnActivated;
        [SerializeField] private string activationAnimation = "interact_bonfire_01";

        [Header("Network")]
        [SerializeField] private NetworkVariable<bool> networkIsActivated = 
            new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        // -------------
        // NETWORK STUFF
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsOwner)
                OnIsActivatedChanged(false, networkIsActivated.Value);

            networkIsActivated.OnValueChanged += OnIsActivatedChanged;

            if (!IsOwner)
                return;

            if (SaveGameManager.instance.currentSaveData.bonfireList.ContainsKey(bonfireID))
            {
                if (SaveGameManager.instance.currentSaveData.bonfireList[bonfireID])
                {
                    networkIsActivated.Value = true;
                }
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            networkIsActivated.OnValueChanged -= OnIsActivatedChanged;
        }

        private void OnIsActivatedChanged(bool oldValue, bool newValue)
        {
            if (!newValue)
                return;

            ActivatedBonfireVisuals();
        }

        // -------
        // HELPERS
        private void ActivatedBonfireVisuals()
        {
            foreach (var gameObject in objectsToActivateOnActivated)
            {
                gameObject.SetActive(true);
            }
        }

        private IEnumerator WaitForActivationToEndToPromptToRest()
        {
            yield return new WaitForSeconds(2.5f);

            RetriggerCollider();

            yield return null;
        }

        // -------------------
        // INTERACTION METHODS
        private void ActivateBonfire(PlayerManager player)
        {
            if (!player.IsOwner)
                return;

            // ROTATE CHARACTER TOWARDS THE BONFIRE
            player.transform.rotation = Quaternion.LookRotation(transform.position - player.transform.position);

            // ANIMATION
            player.playerAnimatorManager.PerformAnimationAction(activationAnimation, true);

            // ACTIVATION
            if (!SaveGameManager.instance.currentSaveData.bonfireList.ContainsKey(bonfireID))
            {
                SaveGameManager.instance.currentSaveData.bonfireList.Add(bonfireID, true);
            }
            else
            {
                if (!SaveGameManager.instance.currentSaveData.bonfireList[bonfireID])
                {
                    SaveGameManager.instance.currentSaveData.bonfireList.Remove(bonfireID);
                    SaveGameManager.instance.currentSaveData.bonfireList.Add(bonfireID, true);
                }
            }
            
            networkIsActivated.Value = true;

            // POP UP MESSAGE
            PlayerUIManager.instance.popUpManager.SendBossDefeatedPopUp("BON ON FIRE.");

            // PROMPT TO REST
            StartCoroutine(WaitForActivationToEndToPromptToRest());
        }
        
        private void RestAtBonfire(PlayerManager player)
        {
            if (!player.IsOwner)
                return;

            // RESET ALL STATUS EFFECTS


            // RESET HEALTH/STAMINA/MANA
            player.playerStatsManager.FullHealthRecover();
            player.playerStatsManager.FullStaminaRecover();

            // RESPAWN ALL CHARACTERS
            AIManager.instance.RespawnAICharacters();

            // ROTATE CHARACTER TOWARDS THE BONFIRE
            player.transform.rotation = Quaternion.LookRotation(transform.position - player.transform.position);

            // DO A TRANSITION TO SITTING POSE


            // DO A GAME SAVE
            SaveGameManager.instance.SaveGame();
        }

        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            if (networkIsActivated.Value)
                RestAtBonfire(player);
            else
                ActivateBonfire(player);
        }

        public override string GetInteractableText()
        {
            if (networkIsActivated.Value)
                return base.GetInteractableText();
            else
                return unactivatedPopUpText;
        }
    }
}
