using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Health Damage")]
    public class TakeHealthDamageEffect : InstantEffect
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
        [SerializeField] public bool poiseIsBroken = false;

        [Header("Hit Info")]
        [SerializeField] public CharacterManager damageCauser;
        [SerializeField] public Vector3 contactPoint;
        [SerializeField] public float hitAngle;

        [Header("Animation")]
        [SerializeField] private bool playDamageAnimation = true;
        [SerializeField] private bool manuallySelectDamageAnimation = false;
        [SerializeField] private string damageAnimationName;

        [Header("Sound FX")]
        [SerializeField] private bool toPlaySFX = true;
        [SerializeField] private AudioClip elementalDamageSFX;

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

            if (playDamageAnimation)        // DAMAGE ANIMATION
            {
                // Apply default damage animation.
                if (manuallySelectDamageAnimation)
                {
                    // Apply custom damage animation.
                }

                PlayDamageAnimation(ref character);
            }
        }

        private void DecreaseHealth(ref CharacterManager character)
        {
            if (damageCauser != null)
            {
                // Consider all damageCauser's damage boosters;
            }

            // Consider character's flat damage reducers and reduce them from damage types;
            // Consider all character's other damage reducers (armor, potions, tokens etc);
            
            totalDamage = physicalDamage + holyDamage + fireDamage + magicDamage + lightningDamage;
            if (totalDamage <= 0.0f)
            {
                totalDamage = 1.0f;
            }

            DebugManager.instance.DamageReceiveLog(totalDamage);
            character.characterNetwork.networkCurrentHealth.Value -= totalDamage;
        }

        private void PlayVFX(ref CharacterManager character)
        {
            character.characterEffectsManager.PlayBloodSplashVFX(contactPoint, hitAngle);
        }

        private void PlaySFX(ref CharacterManager character)
        {
            // PHYSICAL SFX DAMAGE
            character.characterSFXManager.PlayAudioClip(SFXManager.instance.GetRandomSFX(ref SFXManager.instance.physicalDamageSFX), 0.5f);

            // DAMAGE GRUNT
            character.characterSFXManager.PlayDamageGruntSoundFX();

            // PLAY OTHER ELEMENTAL DAMAGE SFX IF SUCH PROPERTIES ON EFFECT
        }

        private void PlayDamageAnimation(ref CharacterManager character)
        {
            poiseIsBroken = true;

            if (hitAngle >= 145 && hitAngle <= 180 ||
                hitAngle <= -145 && hitAngle >= -180)    // HIT FROM FRONT
            {
                damageAnimationName = 
                    character.characterAnimatorManager.GetNextRandomDamageAnimationFromList(
                    ref character.characterAnimatorManager.HitFrontMedium);
            }
            else if (hitAngle <= 45 && hitAngle >= -45) // HIT FROM BEHIND
            {
                damageAnimationName =
                    character.characterAnimatorManager.GetNextRandomDamageAnimationFromList(
                    ref character.characterAnimatorManager.HitBackMedium);
            }
            else if (hitAngle > 45 && hitAngle < 145)   // HIT FROM LEFT SIDE
            {
                damageAnimationName =
                    character.characterAnimatorManager.GetNextRandomDamageAnimationFromList(
                    ref character.characterAnimatorManager.HitLeftMedium);
            }
            else if (hitAngle < -45 && hitAngle > -145) // HIT FROM RIGHT SIDE
            {
                damageAnimationName =
                    character.characterAnimatorManager.GetNextRandomDamageAnimationFromList(
                    ref character.characterAnimatorManager.HitRightMedium);
            }

            if (poiseIsBroken)
            {
                character.characterAnimatorManager.PerformAnimationAction(damageAnimationName, true);
                character.characterAnimatorManager.lastUsedDamageAnimation = damageAnimationName;
                poiseIsBroken = false;
            }
        }
    }
}
