using UnityEngine;
using TMPro;

namespace FG
{
    public class UI_Boss_HealthBar : UI_ProgressBar
    {
        [Header("Config")]
        [SerializeField] private GameObject parentObject;
        [SerializeField] private TextMeshProUGUI bossNameText;

        private BossAICharacterManager relatedBoss;

        public void EnableHealthBar(BossAICharacterManager boss)
        {
            relatedBoss = boss;

            // TEXT SETUP
            bossNameText.text = boss.aiCharacterName;

            // HEALTH SETUP
            SetMaxValue(boss.characterNetwork.networkMaxHealth.Value);
            SetNewValue(boss.characterNetwork.networkCurrentHealth.Value);

            boss.characterNetwork.networkCurrentHealth.OnValueChanged += UpdateSliderValue;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            relatedBoss.characterNetwork.networkCurrentHealth.OnValueChanged -= UpdateSliderValue;
        }

        private void UpdateSliderValue(float oldValue, float newValue)
        {
            if (newValue <= 0)
                DestroyHealthBar(2.5f);

            SetNewValue(newValue);
        }

        private void DestroyHealthBar(float time)
        {
            Destroy(parentObject, time);
        }
    }
}
