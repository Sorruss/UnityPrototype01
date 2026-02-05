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
                player.characterNetwork.networkMaxHealth.Value = GetMaxHealthOfVitalityLevel(player.characterNetwork.networkVitality.Value);
                player.characterNetwork.networkCurrentHealth.Value = player.characterNetwork.networkMaxHealth.Value;

                // Updates stamina values if it's New Game. Otherwise, those values would be overwritten whem importing save data.
                player.characterNetwork.networkMaxStamina.Value = GetMaxStaminaOfEnduranceLevel(player.characterNetwork.networkEndurance.Value);
                player.characterNetwork.networkCurrentStamina.Value = player.characterNetwork.networkMaxStamina.Value;
            }
        }

        // -------------
        // ARMOR RELATED
        public void RecalculateTotalArmorStatsImpact()
        {
            #region RESET_ALL_VALUES
            // DAMAGE ABSORBTION
            armorDamageAbsorbtionPhysical = 0.0f;
            armorDamageAbsorbtionMagic = 0.0f;
            armorDamageAbsorbtionFire = 0.0f;
            armorDamageAbsorbtionLightning = 0.0f;
            armorDamageAbsorbtionHoly = 0.0f;
            
            // RESISTANCES
            armorPoisonResistance = 0;
            armorRotResistance = 0;
            armorBleedResistance = 0;
            armorFrostResistance = 0;
            armorMadnessResistance = 0;
            armorSleepResistance = 0;
            armorCurseResistance = 0;

            // POISE
            basePoiseOffense = 0;
            #endregion

            // ADD HEAD PIECE IMPACT
            if (player.playerInventoryManager.HeadArmorScriptable != null)
            {
                // DAMAGE ABSORBTION
                armorDamageAbsorbtionPhysical += player.playerInventoryManager.HeadArmorScriptable.physicalDamageAbsorbtion;
                armorDamageAbsorbtionMagic += player.playerInventoryManager.HeadArmorScriptable.magicDamageAbsorbtion;
                armorDamageAbsorbtionFire += player.playerInventoryManager.HeadArmorScriptable.fireDamageAbsorbtion;
                armorDamageAbsorbtionLightning += player.playerInventoryManager.HeadArmorScriptable.lightningDamageAbsorbtion;
                armorDamageAbsorbtionHoly += player.playerInventoryManager.HeadArmorScriptable.holyDamageAbsorbtion;

                // RESISTANCES
                armorPoisonResistance += player.playerInventoryManager.HeadArmorScriptable.poisonResistance;
                armorRotResistance += player.playerInventoryManager.HeadArmorScriptable.rotResistance;
                armorBleedResistance += player.playerInventoryManager.HeadArmorScriptable.bleedResistance;
                armorFrostResistance += player.playerInventoryManager.HeadArmorScriptable.frostResistance;
                armorMadnessResistance += player.playerInventoryManager.HeadArmorScriptable.madnessResistance;
                armorSleepResistance += player.playerInventoryManager.HeadArmorScriptable.sleepResistance;
                armorCurseResistance += player.playerInventoryManager.HeadArmorScriptable.curseResistance;

                // POISE
                basePoiseOffense += player.playerInventoryManager.HeadArmorScriptable.poise;
            }

            // ADD CHEST PIECE IMPACT
            if (player.playerInventoryManager.ChestArmorScriptable != null)
            {
                // DAMAGE ABSORBTION
                armorDamageAbsorbtionPhysical += player.playerInventoryManager.ChestArmorScriptable.physicalDamageAbsorbtion;
                armorDamageAbsorbtionMagic += player.playerInventoryManager.ChestArmorScriptable.magicDamageAbsorbtion;
                armorDamageAbsorbtionFire += player.playerInventoryManager.ChestArmorScriptable.fireDamageAbsorbtion;
                armorDamageAbsorbtionLightning += player.playerInventoryManager.ChestArmorScriptable.lightningDamageAbsorbtion;
                armorDamageAbsorbtionHoly += player.playerInventoryManager.ChestArmorScriptable.holyDamageAbsorbtion;

                // RESISTANCES
                armorPoisonResistance += player.playerInventoryManager.ChestArmorScriptable.poisonResistance;
                armorRotResistance += player.playerInventoryManager.ChestArmorScriptable.rotResistance;
                armorBleedResistance += player.playerInventoryManager.ChestArmorScriptable.bleedResistance;
                armorFrostResistance += player.playerInventoryManager.ChestArmorScriptable.frostResistance;
                armorMadnessResistance += player.playerInventoryManager.ChestArmorScriptable.madnessResistance;
                armorSleepResistance += player.playerInventoryManager.ChestArmorScriptable.sleepResistance;
                armorCurseResistance += player.playerInventoryManager.ChestArmorScriptable.curseResistance;

                // POISE
                basePoiseOffense += player.playerInventoryManager.ChestArmorScriptable.poise;
            }

            // ADD HAND PIECE IMPACT
            if (player.playerInventoryManager.HandArmorScriptable != null)
            {
                // DAMAGE ABSORBTION
                armorDamageAbsorbtionPhysical += player.playerInventoryManager.HandArmorScriptable.physicalDamageAbsorbtion;
                armorDamageAbsorbtionMagic += player.playerInventoryManager.HandArmorScriptable.magicDamageAbsorbtion;
                armorDamageAbsorbtionFire += player.playerInventoryManager.HandArmorScriptable.fireDamageAbsorbtion;
                armorDamageAbsorbtionLightning += player.playerInventoryManager.HandArmorScriptable.lightningDamageAbsorbtion;
                armorDamageAbsorbtionHoly += player.playerInventoryManager.HandArmorScriptable.holyDamageAbsorbtion;

                // RESISTANCES
                armorPoisonResistance += player.playerInventoryManager.HandArmorScriptable.poisonResistance;
                armorRotResistance += player.playerInventoryManager.HandArmorScriptable.rotResistance;
                armorBleedResistance += player.playerInventoryManager.HandArmorScriptable.bleedResistance;
                armorFrostResistance += player.playerInventoryManager.HandArmorScriptable.frostResistance;
                armorMadnessResistance += player.playerInventoryManager.HandArmorScriptable.madnessResistance;
                armorSleepResistance += player.playerInventoryManager.HandArmorScriptable.sleepResistance;
                armorCurseResistance += player.playerInventoryManager.HandArmorScriptable.curseResistance;

                // POISE
                basePoiseOffense += player.playerInventoryManager.HandArmorScriptable.poise;
            }

            // ADD LEG PIECE IMPACT
            if (player.playerInventoryManager.LegArmorScriptable != null)
            {
                // DAMAGE ABSORBTION
                armorDamageAbsorbtionPhysical += player.playerInventoryManager.LegArmorScriptable.physicalDamageAbsorbtion;
                armorDamageAbsorbtionMagic += player.playerInventoryManager.LegArmorScriptable.magicDamageAbsorbtion;
                armorDamageAbsorbtionFire += player.playerInventoryManager.LegArmorScriptable.fireDamageAbsorbtion;
                armorDamageAbsorbtionLightning += player.playerInventoryManager.LegArmorScriptable.lightningDamageAbsorbtion;
                armorDamageAbsorbtionHoly += player.playerInventoryManager.LegArmorScriptable.holyDamageAbsorbtion;

                // RESISTANCES
                armorPoisonResistance += player.playerInventoryManager.LegArmorScriptable.poisonResistance;
                armorRotResistance += player.playerInventoryManager.LegArmorScriptable.rotResistance;
                armorBleedResistance += player.playerInventoryManager.LegArmorScriptable.bleedResistance;
                armorFrostResistance += player.playerInventoryManager.LegArmorScriptable.frostResistance;
                armorMadnessResistance += player.playerInventoryManager.LegArmorScriptable.madnessResistance;
                armorSleepResistance += player.playerInventoryManager.LegArmorScriptable.sleepResistance;
                armorCurseResistance += player.playerInventoryManager.LegArmorScriptable.curseResistance;

                // POISE
                basePoiseOffense += player.playerInventoryManager.LegArmorScriptable.poise;
            }
        }
    }
}
