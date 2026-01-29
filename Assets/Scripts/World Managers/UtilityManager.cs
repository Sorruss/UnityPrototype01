using UnityEngine;

namespace FG
{
    public class UtilityManager : MonoBehaviour
    {
        [HideInInspector] public static UtilityManager instance;

        [Header("Layers")]
        [SerializeField] private LayerMask characterMasks;
        [SerializeField] private LayerMask environmentMasks;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        // GETTERS
        public LayerMask GetCharacterMasks() => characterMasks;

        public LayerMask GetEnvironmentMasks() => environmentMasks;

        // HELPER FUNCTIONS
        public bool CanCharacterAttackThisTargetTeam(CharacterTeam attacker, CharacterTeam target)
        {
            if (attacker == CharacterTeam.Team01)
            {
                switch (target)
                {
                    case CharacterTeam.Team01: return false;
                    case CharacterTeam.Team02: return true;
                }
            }
            else if (attacker == CharacterTeam.Team02)
            {
                switch (target)
                {
                    case CharacterTeam.Team01: return true;
                    case CharacterTeam.Team02: return false;
                }
            }

            return false;
        }

        public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(float poiseDamage)
        {
            DamageIntensity damageIntensity = DamageIntensity.Ping;

            if (poiseDamage > 10.0f)
                damageIntensity = DamageIntensity.Light;

            if (poiseDamage > 30.0f)
                damageIntensity = DamageIntensity.Medium;

            if (poiseDamage > 70.0f)
                damageIntensity = DamageIntensity.Heavy;

            if (poiseDamage > 120.0f)
                damageIntensity = DamageIntensity.Colossal;

            return damageIntensity;
        }
    }
}
