using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class BossAICharacterManager : AICharacterManager
    {
        [Header("Boss Config")]
        public BossID bossID;
        private int bossIDint;

        [Header("Common Animations")]
        [SerializeField] private string awakenedAnimation = "Awakened_01";
        [SerializeField] private string sleepingAnimation = "Sleeping_01";

        [Header("Phase Shifting")]
        [SerializeField] private string shiftToPhase01Animation = "ShiftPhase_01";
        [SerializeField] private float shiftToPhase01HealthPercentage = 0.5f;

        [Header("Music")]
        [SerializeField] private AudioClip musicIntro;
        [SerializeField] private AudioClip musicPhase01;

        [Header("State Machine")]
        public AIStateSleep sleepState;
        public AIStateCombatStance combatStanceStatePhase01;

        [Header("Flags")]
        public NetworkVariable<bool> isFightActive =
            new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenAwakened = 
            new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenDefeated = 
            new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Fog Walls")]
        [SerializeField] private List<InteractableFogWall> relatedFogWalls = new List<InteractableFogWall>();

        // ------
        // EVENTS
        protected override void Awake()
        {
            base.Awake();

            bossIDint = Convert.ToInt32(bossID);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // SUBSCRIPTIONS
            OnIsFightActiveChanged(false, isFightActive.Value);
            isFightActive.OnValueChanged += OnIsFightActiveChanged;

            if (IsServer)   // ORDER OF OPERATIONS IS IMPORTANT
            {
                // INSTANTIATING NEW STATES
                sleepState = Instantiate(sleepState);
                combatStanceStatePhase01 = Instantiate(combatStanceStatePhase01);

                // GETTING VALUES FROM SAVE FILE
                FetchValuesFromSaveFile();

                // WAIT AND GET RELATED TO BOSS FOG WALLS
                StartCoroutine(WaitForFogWallsListToBePopulatedCoroutine());

                // DECIDE IF TO ENABLE OR DISABLE FOG WALLS
                if (hasBeenAwakened.Value)
                {
                    EnableFogWalls(true);
                }

                if (hasBeenDefeated.Value)
                {
                    characterNetwork.networkIsActive.Value = false;
                    EnableFogWalls(false);
                }
            }

            // MAKING BOSS SLEEP IF WASN'T AWAKENED YET
            if (!hasBeenAwakened.Value)
            {
                currentState = sleepState;
                characterAnimatorManager.PerformTargetAnimationAction(sleepingAnimation, true);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            isFightActive.OnValueChanged -= OnIsFightActiveChanged;
        }

        // ----------
        // COROUTINES
        private IEnumerator WaitForFogWallsListToBePopulatedCoroutine()
        {
            while (NetworkObjectsManager.instance.FogWalls.Count <= 0)
                yield return new WaitForEndOfFrame();

            foreach (var fogWall in NetworkObjectsManager.instance.FogWalls)
                if (fogWall.relatedBossID == bossID)
                    relatedFogWalls.Add(fogWall);

            yield return null;
        }

        public override IEnumerator ProcessDeath(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                characterNetwork.networkCurrentHealth.Value = 0.0f;
                characterNetwork.networkIsDead.Value = true;

                // Reset all needed flags.
                // Do aerial death animation if in air.
                // Loose souls.

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PerformAnimationAction("Death_01", true);
                }
                else
                {
                    // Play manually selected one.
                }
            }

            BossDefeated();

            // Play SFX death sound.
            characterSFXManager.PlayAudioClip(SFXManager.instance.GetRandomSFX(ref characterSFXManager.deathGrunts));

            yield return null;

            // Award player with souls.

            // Enable ability to respawn.
        }

        // --------------
        // SUPP FUNCTIONS
        private void FetchValuesFromSaveFile()
        {
            if (!SaveGameManager.instance.currentSaveData.bossListAwakened.ContainsKey(bossIDint))
            {
                SaveGameManager.instance.currentSaveData.bossListAwakened.Remove(bossIDint);
                SaveGameManager.instance.currentSaveData.bossListDefeated.Remove(bossIDint);

                SaveGameManager.instance.currentSaveData.bossListAwakened.Add(bossIDint, false);
                SaveGameManager.instance.currentSaveData.bossListDefeated.Add(bossIDint, false);
            }
            else
            {
                hasBeenAwakened.Value = SaveGameManager.instance.currentSaveData.bossListAwakened[bossIDint];
                hasBeenDefeated.Value = SaveGameManager.instance.currentSaveData.bossListDefeated[bossIDint];
            }
        }

        private void EnableFogWalls(bool enable)
        {
            if (!IsServer)
                return;

            foreach (var wall in relatedFogWalls)
                wall.networkIsActive.Value = enable;
        }
        
        // -----------
        // AI HANDLING
        public void AwakeBoss()
        {
            EnableFogWalls(true);
            currentState = idleState;

            if (!IsServer)
                return;

            characterAnimatorManager.PerformAnimationAction(awakenedAnimation, true);

            hasBeenAwakened.Value = true;
            isFightActive.Value = true;

            SaveGameManager.instance.currentSaveData.bossListAwakened.Remove(bossIDint);
            SaveGameManager.instance.currentSaveData.bossListAwakened.Add(bossIDint, hasBeenAwakened.Value);
        }

        private void BossDefeated()
        {
            EnableFogWalls(false);
            PlayerUIManager.instance.popUpManager.SendBossDefeatedPopUp();

            if (!IsServer)
                return;

            hasBeenDefeated.Value = true;
            isFightActive.Value = false;

            SaveGameManager.instance.currentSaveData.bossListDefeated.Remove(bossIDint);
            SaveGameManager.instance.currentSaveData.bossListDefeated.Add(bossIDint, hasBeenDefeated.Value);
        }

        private void Phase01Shift()
        {
            characterAnimatorManager.PerformAnimationAction(shiftToPhase01Animation, true);
            combatStanceState = combatStanceStatePhase01;
            currentState = combatStanceState;
        }

        // -------------
        // SUBSCRIPTIONS
        private void OnIsFightActiveChanged(bool oldValue, bool newValue)
        {
            if (!newValue)
            {
                SFXManager.instance.StopBossFighMusic();
                return;
            }

            // MUSIC LOGIC
            SFXManager.instance.PlayBossFightMusic(musicIntro, musicPhase01);

            // HEALTH BAR LOGIC
            GameObject healthBarInstance = Instantiate(
                PlayerUIManager.instance.hudManager.bossHealthBar, 
                PlayerUIManager.instance.hudManager.bossHealthBarOrigin);
            UI_Boss_HealthBar healthBar = healthBarInstance.GetComponentInChildren<UI_Boss_HealthBar>();
            healthBar.EnableHealthBar(this);
        }

        // ---------
        // OVERRIDES
        protected override void CheckHealth(float prevHealth, float newHealth)
        {
            base.CheckHealth(prevHealth, newHealth);

            if (newHealth <= 0.0f)
                return;

            float neededHealthToShiftToPhase01 = characterNetwork.networkMaxHealth.Value * shiftToPhase01HealthPercentage;
            if (newHealth <= neededHealthToShiftToPhase01)
            {
                Phase01Shift();
            }
        }
    }
}
