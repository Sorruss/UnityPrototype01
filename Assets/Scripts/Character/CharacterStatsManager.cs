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

        [Header("Damage Absorbtion (0.0f - 1.0f)")]
        public float damageAbsorbtionPhysical = 0.0f;
        public float damageAbsorbtionMagic = 0.0f;
        public float damageAbsorbtionFire = 0.0f;
        public float damageAbsorbtionLightning = 0.0f;
        public float damageAbsorbtionHoly = 0.0f;

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
            {
                return false;
            }

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
