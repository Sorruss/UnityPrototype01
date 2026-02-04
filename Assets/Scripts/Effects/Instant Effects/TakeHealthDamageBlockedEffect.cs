using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Health Damage Blocked")]
    public class TakeHealthDamageBlockedEffect : InstantEffect
    {
        [Header("Total Damage")]
        [SerializeField] private float totalDamage;

        [Header("Damage")]
        public float physicalDamage;
        public float magicDamage;
        public float fireDamage;
        public float lightningDamage;
        public float holyDamage;

        [Header("Poise Damage")]
        public int poiseDamage;

        [Header("Stamina Damage")]
        public float staminaDamage;

        [Header("Target's Damage Absorbtion")]
        public float physicalDamageAbsorbtion;
        public float magicDamageAbsorbtion;
        public float fireDamageAbsorbtion;
        public float lightningDamageAbsorbtion;
        public float holyDamageAbsorbtion;

        [Header("Target's Stability")]
        public float stability;

        [Header("Hit Info")]
        public CharacterManager damageCauser;
        public Vector3 contactPoint;
        public float hitAngle;

        [Header("Animation")]
        [SerializeField] private bool playDamageBlockedAnimation = true;

        [Header("Sound FX")]
        [SerializeField] private bool toPlaySFX = true;

        public override void ApplyInstantEffect(ref CharacterManager character)
        {
            // Check if character is dead.
            if (character.characterNetwork.networkIsDead.Value)
                return;

            // Check if character is invalnurable.
            if (character.characterNetwork.networkIsInvincible.Value)
                return;

            base.ApplyInstantEffect(ref character);

            // -------------------------------------------------------------
            // PART WHERE ALL CLIENTS SHOULD SEE THE ACTIONS (SFX, VFX, ETC)
            PlayVFX(ref character);         // VFX
            if (toPlaySFX)
                PlaySFX(ref character);     // SFX

            // -------------------------------------------------------------
            // SAFEGUARD IF THIS CLIENT IS NOT AN OWNER IS INSIDE OF METHODS
            // -------------------------------------------------------------

            DecreaseHealth(ref character);  // DEDUCT HEALTH
            DecreaseStamina(ref character); // DEDUCT STAMINA

            if (character.characterNetwork.networkIsDead.Value) // IF DIED -> NO NEED TO DO ANY OTHER STUFF BELOW
                return;

            if (playDamageBlockedAnimation)        // DAMAGE ANIMATION
                PlayDamageBlockedAnimation(ref character);

            CheckForGuardBreak(ref character);      // GUARD BROKEN ANIMATION
        }

        private void DecreaseHealth(ref CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (damageCauser != null)
            {
                // Consider all damageCauser's damage boosters;
            }

            // Add up all character's damage absobtions
            physicalDamageAbsorbtion += character.characterStatsManager.damageAbsorbtionPhysical;
            magicDamageAbsorbtion += character.characterStatsManager.damageAbsorbtionMagic;
            fireDamageAbsorbtion += character.characterStatsManager.damageAbsorbtionFire;
            lightningDamageAbsorbtion += character.characterStatsManager.damageAbsorbtionLightning;
            holyDamageAbsorbtion += character.characterStatsManager.damageAbsorbtionHoly;

            // Clip absobrtions so they limit at 1
            physicalDamageAbsorbtion = Mathf.Clamp01(physicalDamageAbsorbtion);
            magicDamageAbsorbtion = Mathf.Clamp01(magicDamageAbsorbtion);
            fireDamageAbsorbtion = Mathf.Clamp01(fireDamageAbsorbtion);
            lightningDamageAbsorbtion = Mathf.Clamp01(lightningDamageAbsorbtion);
            holyDamageAbsorbtion = Mathf.Clamp01(holyDamageAbsorbtion);

            // Consider character's damage reducers and reduce them from damage types;
            physicalDamage *= 1.0f - physicalDamageAbsorbtion;
            magicDamage *= 1.0f - magicDamageAbsorbtion;
            fireDamage *= 1.0f - fireDamageAbsorbtion;
            lightningDamage *= 1.0f - lightningDamageAbsorbtion;
            holyDamage *= 1.0f - holyDamageAbsorbtion;

            // Calculate total damage
            totalDamage = physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage;
            if (totalDamage <= 0.0f)
                totalDamage = 1.0f;

            DebugManager.instance.DamageBlockedReceiveLog(totalDamage);
            character.characterNetwork.networkCurrentHealth.Value -= totalDamage;
        }

        private void DecreaseStamina(ref CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            stability = character.characterStatsManager.stability;
            float staminaToDeduct = staminaDamage * (1.0f - stability);

            if (!character.characterStatsManager.TryDecreaseStamina(staminaToDeduct))
                character.characterNetwork.networkCurrentStamina.Value = 0.0f;
        }

        private void CheckForGuardBreak(ref CharacterManager character)
        {
            if (character.characterNetwork.networkCurrentStamina.Value > 0.0f)
                return;

            // PLAY GUARD BROKEN SOUND FOR EVERY PLAYER
            character.characterSFXManager.PlayAudioClip(SFXManager.instance.GetRandomSFX(ref SFXManager.instance.guardBrokenSFX), 0.5f);

            if (!character.IsOwner)
                return;

            // PLAY GUARD BROKEN ANIMATION
            character.characterAnimatorManager.PerformAnimationAction("off_guard_broken_01", true);

            // DISABLE BLOCKING
            character.characterNetwork.networkIsBlocking.Value = false;
        }

        private void PlayVFX(ref CharacterManager character)
        {

        }

        private void PlaySFX(ref CharacterManager character)
        {
            character.characterSFXManager.PlayWeaponBlockFX();
        }

        private void PlayDamageBlockedAnimation(ref CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            DamageIntensity damageIntensity = UtilityManager.instance.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);

            string damageBlockAnimation = "off_block_ping_01";
            switch (damageIntensity)
            {
                case DamageIntensity.Light:
                    damageBlockAnimation = "off_block_light_01";
                    break;
                case DamageIntensity.Medium:
                    damageBlockAnimation = "off_block_medium_01";
                    break;
                case DamageIntensity.Heavy:
                    damageBlockAnimation = "off_block_heavy_01";
                    break;
                case DamageIntensity.Colossal:
                    damageBlockAnimation = "off_block_colossal_01";
                    break;
            }

            character.characterAnimatorManager.PerformAnimationAction(damageBlockAnimation, true);
        }
    }
}
