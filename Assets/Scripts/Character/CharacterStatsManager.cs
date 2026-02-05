using UnityEngine;

namespace FG
{
    public class CharacterStatsManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("Stamina - Stamina Recovery")]
        [SerializeField] private float staminaRecoverRate = 0.1f;
        [SerializeField] private float staminaRecoverValue = 2.0f;
        [SerializeField] private float staminaRecoverDelay = 1.0f;

        [Header("Stamina - Actions Cost")]
        public float sprintStaminaCost = 2.0f;
        public float dodgeStaminaCost = 20.0f;
        public float jumpStaminaCost = 10.0f;

        private float staminaRecoverTimer = 0.0f;
        private float staminaDelayTimer = 0.0f;

        [Header("Damage Absorbtion From Blocking (0.0f - 1.0f) (Debug)")]
        public float blockDamageAbsorbtionPhysical = 0.0f;
        public float blockDamageAbsorbtionMagic = 0.0f;
        public float blockDamageAbsorbtionFire = 0.0f;
        public float blockDamageAbsorbtionLightning = 0.0f;
        public float blockDamageAbsorbtionHoly = 0.0f;

        [Header("Damage Absorbtion Stability (0.0 - 1.0) (Debug)")]
        public float stability = 0.0f;

        [Header("Damage Absorbtion From Armor (0.0f - 1.0f) (Debug)")]
        public float armorDamageAbsorbtionPhysical = 0.0f;
        public float armorDamageAbsorbtionMagic = 0.0f;
        public float armorDamageAbsorbtionFire = 0.0f;
        public float armorDamageAbsorbtionLightning = 0.0f;
        public float armorDamageAbsorbtionHoly = 0.0f;

        [Header("Resistance From Armor")]
        public int armorPoisonResistance;
        public int armorRotResistance;
        public int armorBleedResistance;
        public int armorFrostResistance;
        public int armorMadnessResistance;
        public int armorSleepResistance;
        public int armorCurseResistance;

        [Header("Poise")]
        [SerializeField] private float poiseResetTimer;
        [SerializeField] private float poiseResetTime = 8.0f;
        public int totalPoiseDamage;
        public int basePoiseOffense;
        public int bonusAttackingPoiseOffense;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {

        }

        // -----------------------------
        // CONVERTERS FROM LEVEL TO STAT
        public int GetMaxStaminaOfEnduranceLevel(int endurance)
        {
            int maxStamina = endurance * 10;
            return maxStamina;
        }

        public int GetMaxHealthOfVitalityLevel(int vitality)
        {
            int maxVitality = vitality * 10;
            return maxVitality;
        }

        // -------------
        // STAMINA LOGIC
        public void RecoverStamina()
        {
            if (character.isPerfomingAction
                || character.characterNetwork.networkIsJumping.Value
                || character.characterNetwork.networkIsSprinting.Value
                || character.characterNetwork.networkCurrentStamina.Value >= character.characterNetwork.networkMaxStamina.Value)
            {
                staminaDelayTimer = 0.0f;
                return;
            }

            staminaDelayTimer += Time.deltaTime;
            if (staminaDelayTimer >= staminaRecoverDelay)
            {
                staminaRecoverTimer += Time.deltaTime;
                if (staminaRecoverTimer >= staminaRecoverRate)
                {
                    staminaRecoverTimer = 0.0f;
                    IncreaseStamina(staminaRecoverValue);
                }
            }
        }

        public void IncreaseStamina(float stamina)
        {
            float currStamina = character.characterNetwork.networkCurrentStamina.Value;
            currStamina += stamina;
            currStamina = Mathf.Clamp(currStamina, 0.0f, character.characterNetwork.networkMaxStamina.Value);
            character.characterNetwork.networkCurrentStamina.Value = currStamina;
        }

        public bool TryDecreaseStamina(float stamina)
        {
            if (!IsEnoughStamina(stamina))
                return false;

            float currStamina = character.characterNetwork.networkCurrentStamina.Value;
            currStamina -= stamina;
            currStamina = Mathf.Clamp(currStamina, 0.0f, character.characterNetwork.networkMaxStamina.Value);
            character.characterNetwork.networkCurrentStamina.Value = currStamina;

            return true;
        }

        public bool IsEnoughStamina(float stamina)
        {
            return character.characterNetwork.networkCurrentStamina.Value >= stamina;
        }
    
        // -----------
        // POISE LOGIC
        public void HandlePoiseReset()
        {
            if (poiseResetTimer > 0.0f)
            {
                poiseResetTimer -= Time.deltaTime;
            }
            else
            {
                poiseResetTimer = 0.0f;
                totalPoiseDamage = 0;
            }
        }

        public void DeductPoise(int poiseToDeduct)
        {
            totalPoiseDamage -= poiseToDeduct;
            poiseResetTimer = poiseResetTime;
        }

        public int GetPoiseLeft()
        {
            int characterPoise = basePoiseOffense + bonusAttackingPoiseOffense; // how much poise we have
            return characterPoise + totalPoiseDamage; // totalPoiseDamage is negative value
        }

        // --------
        // RECOVERS
        public void FullHealthRecover()
        {
            if (!character.IsOwner)
                return;

            character.characterNetwork.networkCurrentHealth.Value = character.characterNetwork.networkMaxHealth.Value;
        }

        public void FullStaminaRecover()
        {
            if (!character.IsOwner)
                return;

            character.characterNetwork.networkCurrentStamina.Value = character.characterNetwork.networkMaxStamina.Value;
        }
    }
}
