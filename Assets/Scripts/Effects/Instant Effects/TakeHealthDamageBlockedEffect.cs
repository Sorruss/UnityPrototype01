using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Health Damage Blocked")]
    public class TakeHealthDamageBlockedEffect : InstantEffect
    {
        [Header("Total Damage")]
        [SerializeField] private float totalDamage;

        [Header("Damage")]
        [SerializeField] public float physicalDamage;
        [SerializeField] public float holyDamage;
        [SerializeField] public float fireDamage;
        [SerializeField] public float magicDamage;
        [SerializeField] public float lightningDamage;

        [Header("Poise Damage")]
        [SerializeField] public float poiseDamage;

        [Header("Hit Info")]
        [SerializeField] public CharacterManager damageCauser;
        [SerializeField] public Vector3 contactPoint;
        [SerializeField] public float hitAngle;

        [Header("Animation")]
        [SerializeField] private bool playDamageBlockedAnimation = true;

        [Header("Sound FX")]
        [SerializeField] private bool toPlaySFX = true;

        public override void ApplyEffect(ref CharacterManager character)
        {
            // Check if character is dead.
            if (character.characterNetwork.networkIsDead.Value)
                return;

            // Check if character is invalnurable.
            if (character.characterNetwork.networkIsInvincible.Value)
                return;

            base.ApplyEffect(ref character);

            // -------------------------------------------------------------
            // PART WHERE ALL CLIENTS SHOULD SEE THE ACTIONS (SFX, VFX, ETC)
            PlayVFX(ref character);         // VFX
            if (toPlaySFX)
                PlaySFX(ref character);     // SFX

            // ----------------------------------------------------
            // SAFEGUARD IF THIS CLIENT IS NOT AN OWNER
            if (!character.IsOwner)
                return;

            DecreaseHealth(ref character);  // Calculate all damages;

            if (character.characterNetwork.networkIsDead.Value) // IF DIED -> NO NEED TO DO ANY OTHER STUFF BELOW
                return;

            if (playDamageBlockedAnimation)        // DAMAGE ANIMATION
                PlayDamageBlockedAnimation(ref character);
        }

        private void DecreaseHealth(ref CharacterManager character)
        {
            if (damageCauser != null)
            {
                // Consider all damageCauser's damage boosters;
            }

            // Consider character's flat damage reducers and reduce them from damage types;
            physicalDamage *= 1.0f - character.characterStatsManager.damageAbsorbtionPhysical;
            magicDamage *= 1.0f - character.characterStatsManager.damageAbsorbtionMagic;
            fireDamage *= 1.0f - character.characterStatsManager.damageAbsorbtionFire;
            lightningDamage *= 1.0f - character.characterStatsManager.damageAbsorbtionLightning;
            holyDamage *= 1.0f - character.characterStatsManager.damageAbsorbtionHoly;

            // Consider all character's other damage reducers (armor, potions, tokens etc);

            totalDamage = physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage;
            if (totalDamage <= 0.0f)
                totalDamage = 1.0f;

            DebugManager.instance.DamageBlockedReceiveLog(totalDamage);
            character.characterNetwork.networkCurrentHealth.Value -= totalDamage;
        }

        private void PlayVFX(ref CharacterManager character)
        {

        }

        private void PlaySFX(ref CharacterManager character)
        {

        }

        private void PlayDamageBlockedAnimation(ref CharacterManager character)
        {
            DamageIntensity damageIntensity = UtilityManager.instance.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);

            string damageBlockAnimation = "hit_block_ping_01";
            switch (damageIntensity)
            {
                case DamageIntensity.Light:
                    damageBlockAnimation = "hit_block_light_01";
                    break;
                case DamageIntensity.Medium:
                    damageBlockAnimation = "hit_block_medium_01";
                    break;
                case DamageIntensity.Heavy:
                    damageBlockAnimation = "hit_block_heavy_01";
                    break;
                case DamageIntensity.Colossal:
                    damageBlockAnimation = "hit_block_colossal_01";
                    break;
            }

            //character.characterAnimatorManager.PerformAnimationAction(damageBlockAnimation, true);
        }
    }
}
