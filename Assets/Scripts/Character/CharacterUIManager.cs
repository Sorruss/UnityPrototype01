using UnityEngine;

namespace FG
{
    public class CharacterUIManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("UI Components (Hand)")]
        [SerializeField] public UI_Character_HeathBar characterHeathBar;

        [Header("Config")]
        [SerializeField] private bool showHealthBarOnDamage = false;

        private void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void UpdateHealthBar(float oldHealth, float newHealth)
        {
            if (!showHealthBarOnDamage)
                return;

            if (newHealth == character.characterNetwork.networkMaxHealth.Value)
                return;

            characterHeathBar.previousHealth = oldHealth;
            int damageDealt = (int)(oldHealth - newHealth);
            
            characterHeathBar.ActivateHealthBar();
            characterHeathBar.DisplayDamageNumber(damageDealt);
        }
    }
}
