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

        // -------
        // GETTERS
        public LayerMask GetCharacterMasks() => characterMasks;

        public LayerMask GetEnvironmentMasks() => environmentMasks;

        // ----------------
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

        public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(int poiseDamage)
        {
            DamageIntensity damageIntensity = DamageIntensity.Ping;

            if (poiseDamage > 10)
                damageIntensity = DamageIntensity.Light;

            if (poiseDamage > 30)
                damageIntensity = DamageIntensity.Medium;

            if (poiseDamage > 70)
                damageIntensity = DamageIntensity.Heavy;

            if (poiseDamage > 120)
                damageIntensity = DamageIntensity.Colossal;

            return damageIntensity;
        }
    
        // RIPOSTE
        public Vector3 GetRipostePositionBasedOnWeaponClass(WeaponClass weaponClass)
        {
            Vector3 position = new Vector3(0.11f, 0.0f, 0.7f);

            // ADJUST IT FOR WEAPONS IF NEEDED
            switch (weaponClass)
            {
                case WeaponClass.STRAIGHT_SWORD:
                    break;
                case WeaponClass.SHIELD:
                    break;
                case WeaponClass.FIST:
                    break;
            }

            return position;
        }

        public string GetRiposteVictimAnimationBasedOnWeaponClass(WeaponClass weaponClass)
        {
            string riposteVictimAnimation = null;

            switch (weaponClass)
            {
                case WeaponClass.STRAIGHT_SWORD:
                    riposteVictimAnimation = "Riposte_Sword_Victim_01";
                    break;
                case WeaponClass.SHIELD:
                    break;
                case WeaponClass.FIST:
                    break;
            }

            return riposteVictimAnimation;
        }

        // BACKSTAB
        public Vector3 GetBackstabPositionBasedOnWeaponClass(WeaponClass weaponClass)
        {
            Vector3 position = new Vector3(0.12f, 0.0f, 0.74f);

            // ADJUST IT FOR WEAPONS IF NEEDED
            switch (weaponClass)
            {
                case WeaponClass.STRAIGHT_SWORD:
                    break;
                case WeaponClass.SHIELD:
                    break;
                case WeaponClass.FIST:
                    break;
            }

            return position;
        }

        public string GetBackstabVictimAnimationBasedOnWeaponClass(WeaponClass weaponClass)
        {
            string riposteVictimAnimation = null;

            switch (weaponClass)
            {
                case WeaponClass.STRAIGHT_SWORD:
                    riposteVictimAnimation = "Backstab_Sword_Victim_01";
                    break;
                case WeaponClass.SHIELD:
                    break;
                case WeaponClass.FIST:
                    break;
            }

            return riposteVictimAnimation;
        }
    }
}
