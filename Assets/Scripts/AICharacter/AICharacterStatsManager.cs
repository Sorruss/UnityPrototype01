using UnityEngine;

namespace FG
{
    public class AICharacterStatsManager : CharacterStatsManager
    {
        private AICharacterManager aiCharacter;

        [Header("Posture")]
        [SerializeField] private bool ignorePosture = false;
        [SerializeField] private int maxPosture = 150;
        public int currentPosture;
        [SerializeField] private int postureRegenPerSecond = 15;
        [SerializeField] private float postureRegenTime = 8.0f;
        [SerializeField] private float postureRegenTimer;
        private float postureTickTimer;

        // ------------
        // UNITY EVENTS
        protected override void Awake()
        {
            base.Awake();

            aiCharacter = GetComponent<AICharacterManager>();
        }
        
        protected override void Start()
        {
            base.Start();

            // POSTURE
            currentPosture = maxPosture;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            HandlePosture();
        }

        // -------
        // POSTURE
        private void HandlePosture()
        {
            if (character.characterNetwork.networkIsDead.Value)
                return;

            if (postureRegenTimer > 0.0f)
            {
                postureRegenTimer -= Time.deltaTime;
            }
            else
            {
                if (currentPosture < maxPosture)
                {
                    postureTickTimer += Time.deltaTime;

                    if (postureTickTimer >= 1.0f)
                    {
                        currentPosture += postureRegenPerSecond;
                        postureTickTimer = 0.0f;
                    }
                }
                else
                {
                    currentPosture = maxPosture;
                    postureTickTimer = 0.0f;
                }
            }
        }

        public void DamagePosture(int postureDamage)
        {
            if (!character.IsOwner)
                return;

            if (character.characterNetwork.networkIsDead.Value)
                return;

            if (ignorePosture)
                return;

            postureRegenTimer = postureRegenTime;
            currentPosture -= postureDamage;

            if (currentPosture <= 0)
            {
                DamageIntensity lastDamageIntensity = 
                    UtilityManager.instance.GetDamageIntensityBasedOnPoiseDamage(
                        character.characterCombatManager.lastPoiseDamageTaken);

                // THE TIMES WHEN WE DON'T NEED TO MAKE CHARACTER RIPOSTABLE
                // WHEN IT'S A HUGE ASS ATTACK WHICH THROWS CHARACTER INTO THE AIR
                // SO IT PLAYS THE NICE ANIMATION INSTEAD OF BEING STUGGERED
                if (lastDamageIntensity == DamageIntensity.Colossal)
                    currentPosture = 1;

                // WHEN CHARACTER IS ALREADY BEING RIPOSTED AND TAKES DAMAGE
                if (character.characterNetwork.networkIsBeingCriticallyDamaged.Value)
                    currentPosture = 1;

                // WHEN CHARACTER IS RIPOSTABLE AND TAKES DAMAGE
                if (character.characterNetwork.networkIsRipostable.Value)
                    currentPosture = 1;
            }
        }
    }
}
