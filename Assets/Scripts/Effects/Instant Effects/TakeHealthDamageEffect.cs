using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Health Damage")]
    public class TakeHealthDamageEffect : InstantEffect
    {
        [Header("Total Damage")]
        [SerializeField] protected float totalDamage;

        [Header("Damage")]
        public float physicalDamage;
        public float holyDamage;
        public float fireDamage;
        public float magicDamage;
        public float lightningDamage;

        [Header("Poise Damage")]
        public int poiseDamage;
        public bool poiseIsBroken = false;

        [Header("Hit Info")]
        public CharacterManager damageCauser;
        public Vector3 contactPoint;
        public float hitAngle;

        [Header("Animation")]
        [SerializeField] protected bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimationName;

        [Header("Sound FX")]
        [SerializeField] protected bool toPlaySFX = true;

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

            // ----------------------------------------------------
            // SAFEGUARD IF THIS CLIENT IS NOT AN OWNER
            if (!character.IsOwner)
                return;

            DecreaseHealth(ref character);  // DEDUCT HEALTH

            if (character.characterNetwork.networkIsDead.Value) // IF DIED -> NO NEED TO DO ANY OTHER STUFF BELOW
                return;

            DecreasePoise(ref character);

            if (playDamageAnimation)        // DAMAGE ANIMATION
            {
                // Apply default damage animation.
                if (manuallySelectDamageAnimation)
                {
                    // Apply custom damage animation.
                }

                PlayDamageAnimation(ref character);
            }

            // IT'S AT THE BOTTOM SO THE ANIMATION OF IT TAKES THE PRIORITY OVER OTHERS
            DecreasePosture(ref character);
        }

        protected virtual void DecreaseHealth(ref CharacterManager character)
        {
            if (damageCauser != null)
            {
                // Consider all damageCauser's damage boosters;
            }

            // Consider character's flat damage reducers and reduce them from damage types;
            // Consider all character's other damage reducers (armor, potions, tokens etc);
            
            totalDamage = physicalDamage + holyDamage + fireDamage + magicDamage + lightningDamage;
            if (totalDamage <= 0.0f)
                totalDamage = 1.0f;

            DebugManager.instance.DamageReceiveLog(totalDamage);
            character.characterStatsManager.DamageHelth(totalDamage);
        }

        protected virtual void DecreasePoise(ref CharacterManager character)
        {
            // DEDUCT POISE
            character.characterStatsManager.DeductPoise(poiseDamage);

            // DETERMINE IF POISE WAS BROKEN
            if (character.characterStatsManager.GetPoiseLeft() <= 0)
                poiseIsBroken = true;
        }

        protected virtual void DecreasePosture(ref CharacterManager character)
        {
            AICharacterManager aiCharacter = character as AICharacterManager;

            if (aiCharacter == null)
                return;

            aiCharacter.aiCharacterStatsManager.DamagePosture(poiseDamage);

            if (aiCharacter.aiCharacterStatsManager.currentPosture > 0)
                return;

            // RPC INSTANT ANIMATION & SFX
            aiCharacter.aiCharacterAnimatorManager.PerformInstantAnimationAction("Posture_Break_01", true);
            aiCharacter.characterSFXManager.PlayPostureBreakSoundFX();
        }

        protected virtual void PlayVFX(ref CharacterManager character)
        {
            character.characterEffectsManager.PlayBloodSplashVFX(contactPoint);
        }

        protected virtual void PlaySFX(ref CharacterManager character)
        {
            // PHYSICAL SFX DAMAGE
            character.characterSFXManager.PlayAudioClip(SFXManager.instance.GetRandomSFX(ref SFXManager.instance.physicalDamageSFX), 0.5f);

            // DAMAGE GRUNT
            character.characterSFXManager.PlayDamageGruntSoundFX();

            // PLAY OTHER ELEMENTAL DAMAGE SFX IF SUCH PROPERTIES ON EFFECT
        }

        protected virtual void PlayDamageAnimation(ref CharacterManager character)
        {
            HitDirection hitDirection = HitDirection.FRONT;

            #region DETERMINE THE SIDE HIT WAS TAKEN FROM
            if (hitAngle >= 145 && hitAngle <= 180 ||
                hitAngle <= -145 && hitAngle >= -180)
            {
                hitDirection = HitDirection.BEHIND;
            }
            else if (hitAngle <= 45 && hitAngle >= -45)
            {
                hitDirection = HitDirection.FRONT;
            }
            else if (hitAngle < -45 && hitAngle > -145)
            {
                hitDirection = HitDirection.LEFT;
            }
            else if (hitAngle > 45 && hitAngle < 145)
            {
                hitDirection = HitDirection.RIGHT;
            }
            #endregion

            #region TO PLAY STUN ANIMATION OR JUST A FLINCH
            if (poiseIsBroken)  // POISE WAS BROKEN SO WE ARE STUNNED
            {
                switch (hitDirection)
                {
                    case HitDirection.LEFT:
                        damageAnimationName =
                            character.characterAnimatorManager.GetNextRandomDamageAnimationFromList(
                            ref character.characterAnimatorManager.HitLeftMedium);
                        break;
                    case HitDirection.RIGHT:
                        damageAnimationName =
                            character.characterAnimatorManager.GetNextRandomDamageAnimationFromList(
                            ref character.characterAnimatorManager.HitRightMedium);
                        break;
                    case HitDirection.FRONT:
                        damageAnimationName =
                            character.characterAnimatorManager.GetNextRandomDamageAnimationFromList(
                            ref character.characterAnimatorManager.HitFrontMedium);
                        break;
                    case HitDirection.BEHIND:
                        damageAnimationName =
                            character.characterAnimatorManager.GetNextRandomDamageAnimationFromList(
                            ref character.characterAnimatorManager.HitBackMedium);
                        break;
                    default:
                        break;
                }

                character.characterAnimatorManager.PerformAnimationAction(damageAnimationName, true);
                poiseIsBroken = false;
            }
            else   // POISE WAS NOT BROKEN SO WE JUST PLAY FLINCH ANIMATION
            {
                switch (hitDirection)
                {
                    case HitDirection.LEFT:
                        damageAnimationName =
                            character.characterAnimatorManager.GetNextRandomDamageAnimationFromList(
                            ref character.characterAnimatorManager.HitLeftPing);
                        break;
                    case HitDirection.RIGHT:
                        damageAnimationName =
                            character.characterAnimatorManager.GetNextRandomDamageAnimationFromList(
                            ref character.characterAnimatorManager.HitRightPing);
                        break;
                    case HitDirection.FRONT:
                        damageAnimationName =
                            character.characterAnimatorManager.GetNextRandomDamageAnimationFromList(
                            ref character.characterAnimatorManager.HitFrontPing);
                        break;
                    case HitDirection.BEHIND:
                        damageAnimationName =
                            character.characterAnimatorManager.GetNextRandomDamageAnimationFromList(
                            ref character.characterAnimatorManager.HitBackPing);
                        break;
                    default:
                        break;
                }

                character.characterAnimatorManager.PerformAnimationAction(damageAnimationName, false, false, true, true);
            }
            #endregion

            character.characterAnimatorManager.lastUsedDamageAnimation = damageAnimationName;
        }
    }
}
