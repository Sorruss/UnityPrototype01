using UnityEngine;

namespace FG
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        private PlayerManager player;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            if (player.IsOwner)
            {
                // Updates health values if it's New Game. Otherwise, those values would be overwritten whem importing save data.
                player.characterNetwork.networkMaxHealth.Value = player.playerStatsManager.GetMaxHealthOfVitalityLevel(player.characterNetwork.networkVitality.Value);
                player.characterNetwork.networkCurrentHealth.Value = player.characterNetwork.networkMaxHealth.Value;

                // Updates stamina values if it's New Game. Otherwise, those values would be overwritten whem importing save data.
                player.characterNetwork.networkMaxStamina.Value = player.playerStatsManager.GetMaxStaminaOfEnduranceLevel(player.characterNetwork.networkEndurance.Value);
                player.characterNetwork.networkCurrentStamina.Value = player.characterNetwork.networkMaxStamina.Value;
            }
        }
    }
}
