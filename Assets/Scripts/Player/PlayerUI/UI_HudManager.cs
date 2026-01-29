using UnityEngine;
using UnityEngine.UI;

namespace FG
{
    public class UI_HudManager : MonoBehaviour
    {
        [Header("Status Bars")]
        private UI_StaminaBar staminaBar;
        private UI_HealthBar healthBar;

        [Header("Boss Health Bar")]
        public Transform bossHealthBarOrigin;
        public GameObject bossHealthBar;

        [Header("Quick Item Slots")]
        [SerializeField] private Image quickItemSlotMagicTop;
        [SerializeField] private Image quickItemSlotMagicBottom;
        [SerializeField] private Image quickItemSlotWeaponLeft;
        [SerializeField] private Image quickItemSlotWeaponRight;

        private void Awake()
        {
            staminaBar = GetComponentInChildren<UI_StaminaBar>();
            healthBar = GetComponentInChildren<UI_HealthBar>();
        }

        // TOOLS.
        public void RefreshUI()
        {
            // STATUS BARS
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);

            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
        }

        // STAMINA RELATED FUNCTIONALITY.
        public void SetMaxStamina(int maxStamina)
        {
            staminaBar.SetMaxValue(maxStamina);
        }

        public void SetStamina(float staminaValue)
        {
            staminaBar.SetNewValue(staminaValue);
        }

        public void OnStaminaChanged(float prevValue, float newValue)
        {
            SetStamina(newValue);
        }

        public void OnMaxStaminaChanged(int prevValue, int newValue)
        {
            SetMaxStamina(newValue);
        }

        // HEALTH RELATED FUNCTIONALITY.
        public void SetMaxHealth(int maxHealth)
        {
            healthBar.SetMaxValue(maxHealth);
        }

        public void SetHealth(float healthValue)
        {
            healthBar.SetNewValue(healthValue);
        }

        public void OnHealthChanged(float prevValue, float newValue)
        {
            SetHealth(newValue);
        }

        public void OnMaxHealthChanged(int prevValue, int newValue)
        {
            SetMaxHealth(newValue);
        }

        // QUICK ITEM SLOTS RELATED FUNCTIONALITY.
        public void SetQuickSlotLeftWeaponSprite(int weaponID)
        {
            WeaponItem weapon = ItemDatabase.instance.GetWeaponItemByID(weaponID);

            if (weapon == null || weapon.Icon == null)
            {
                quickItemSlotWeaponLeft.enabled = false;
                quickItemSlotWeaponLeft.sprite = null;
                return;
            }

            quickItemSlotWeaponLeft.enabled = true;
            quickItemSlotWeaponLeft.sprite = weapon.Icon;
        }

        public void SetQuickSlotRightWeaponSprite(int weaponID)
        {
            WeaponItem weapon = ItemDatabase.instance.GetWeaponItemByID(weaponID);

            if (weapon == null || weapon.Icon == null)
            {
                quickItemSlotWeaponRight.enabled = false;
                quickItemSlotWeaponRight.sprite = null;
                return;
            }
            
            quickItemSlotWeaponRight.enabled = true;
            quickItemSlotWeaponRight.sprite = weapon.Icon;
        }
    }
}
