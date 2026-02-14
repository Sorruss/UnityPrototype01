using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Critical Damage")]
    public class TakeCriticalDamageEffect : TakeHealthDamageEffect
    {
        public override void ApplyInstantEffect(ref CharacterManager character)
        {
            // Check if character is dead.
            if (character.characterNetwork.networkIsDead.Value)
                return;

            // Check if character is invalnurable.
            if (character.characterNetwork.networkIsInvincible.Value)
                return;

            // ----------------------------------------------------
            // SAFEGUARD IF THIS CLIENT IS NOT AN OWNER
            if (!character.IsOwner)
                return;

            DecreaseHealth(ref character);  // ASSIGN PENDING CRITICAL DAMAGE VARIABLE

            if (character.characterNetwork.networkIsDead.Value) // IF DIED -> NO NEED TO DO ANY OTHER STUFF BELOW
                return;

            character.characterNetwork.networkIsBeingCriticallyDamaged.Value = true;

            if (playDamageAnimation)        // DAMAGE ANIMATION
                PlayDamageAnimation(ref character);
        }

        protected override void DecreaseHealth(ref CharacterManager character)
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

            character.characterCombatManager.pendingCriticalDamage = totalDamage;
        }

        protected override void PlayDamageAnimation(ref CharacterManager character)
        {
            if (character.IsOwner)
                character.characterNetwork.networkIsBeingCriticallyDamaged.Value = true;

            string animationToPlay = "Riposte_Sword_Victim_01";
            if (manuallySelectDamageAnimation)
                animationToPlay = damageAnimationName;

            character.characterAnimatorManager.PerformInstantAnimationAction(animationToPlay, true);
        }
    }
}
