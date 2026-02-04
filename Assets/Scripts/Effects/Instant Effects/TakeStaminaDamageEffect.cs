using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
    public class TakeStaminaDamageEffect : InstantEffect
    {
        public float staminaDamage = 0.0f;

        public override void ApplyInstantEffect(ref CharacterManager character)
        {
            base.ApplyInstantEffect(ref character);
            DecreaseStamina(ref character);
        }

        private void DecreaseStamina(ref CharacterManager character)
        {
            if (character.IsOwner)
            {
                character.characterNetwork.networkCurrentStamina.Value -= staminaDamage;
            }
        }
    }
}
