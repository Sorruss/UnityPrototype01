using TMPro;
using UnityEngine;

namespace FG
{
    public class UI_Character_HeathBar : UI_ProgressBar
    {
        [Header("Components (Auto)")]
        [SerializeField] private CharacterManager characterManager;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private AICharacterManager aiCharacterManager;

        [Header("Config - Main")]
        [SerializeField] private float stayOnScreenTime = 15.0f;
        [SerializeField] private float damageNumberStayOnScreenTime = 2.0f;

        [Header("Config - Needed GameObjects")]
        [SerializeField] private GameObject parentMainObject;
        [SerializeField] private TextMeshProUGUI textCharacterName;
        [SerializeField] private TextMeshProUGUI textDamageNumber;

        [Header("Debug")]
        [SerializeField] private bool shouldBeShown = false;
        [SerializeField] private bool damageNumberShouldBeShown = false;
        [SerializeField] private int totalDamage;
        [SerializeField] public float previousHealth;

        private float stayOnScreenTimer = 0.0f;
        private float damageNumberStayOnScreenTimer = 0.0f;

        protected override void Awake()
        {
            base.Awake();

            characterManager = GetComponentInParent<CharacterManager>();

            // CASTING
            if (characterManager == null)
                return;

            playerManager = characterManager as PlayerManager;
            aiCharacterManager = characterManager as AICharacterManager;
        }

        protected override void Start()
        {
            base.Start();

            // SETTING UP HEALTH ITSELF
            UpdateHealthBarValues();

            // MAKE DAMAGE NUMBER TEXT EMPTY
            textDamageNumber.text = "";

            // SETUP CHARACTER NAME BASED ON THE CHARACTER WE HAVE RN
            if (playerManager != null)
                textCharacterName.text = playerManager.playerNetwork.networkPlayerName.Value.ToString();

            if (aiCharacterManager != null)
                textCharacterName.text = aiCharacterManager.aiCharacterName;

            // DISABLING
            parentMainObject.SetActive(false);
        }

        protected override void Update()
        {
            base.Update();

            // HEALTH BAR VISIBILITY LOGIC
            if (!shouldBeShown)
                return;

            if (characterManager.characterNetwork.networkIsDead.Value)
            {
                shouldBeShown = false;
                parentMainObject.SetActive(false);
                return;
            }

            parentMainObject.transform.LookAt(parentMainObject.transform.position + Camera.main.transform.forward);

            stayOnScreenTimer += Time.deltaTime;
            if (stayOnScreenTimer >= stayOnScreenTime)
            {
                shouldBeShown = false;
                parentMainObject.SetActive(false);
            }

            // DAMAGE NUMBER VISIBILITY LOGIC
            if (!damageNumberShouldBeShown)
                return;

            damageNumberStayOnScreenTimer += Time.deltaTime;
            if (damageNumberStayOnScreenTimer >= damageNumberStayOnScreenTime)
            {
                damageNumberShouldBeShown = false;
                textDamageNumber.text = "";
                totalDamage = 0;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            totalDamage = 0;
            stayOnScreenTimer = 0.0f;
            damageNumberStayOnScreenTimer = 0.0f;
        }

        private void UpdateHealthBarValues()
        {
            if (characterManager == null)
                return;

            SetMaxValue(characterManager.characterNetwork.networkMaxHealth.Value);
            SetNewValue(characterManager.characterNetwork.networkCurrentHealth.Value);
        }

        public void ActivateHealthBar()
        {
            UpdateHealthBarValues();
            parentMainObject.SetActive(true);
            stayOnScreenTimer = 0.0f;
            shouldBeShown = true;
        }

        public void DisplayDamageNumber(int damage)
        {
            if (damage == 0)
                return;

            totalDamage += damage;

            if (previousHealth > characterManager.characterNetwork.networkCurrentHealth.Value)
                textDamageNumber.text = $"- {Mathf.Abs(totalDamage)}";
            else
                textDamageNumber.text = $"+ {Mathf.Abs(totalDamage)}";

            damageNumberShouldBeShown = true;
            damageNumberStayOnScreenTimer = 0.0f;
        }
    }
}
