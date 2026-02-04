using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Character Effects/Static Effects/Two Handing Effect")]
    public class TwoHandingStaticEffect : StaticEffect
    {
        [Header("Config")]
        [SerializeField] private float strengthMultiplier = 1.5f;

        [Header("Debug Info")]
        [SerializeField] private int strengthModifier;

        public override void ApplyStaticEffect(CharacterManager character)
        {
            base.ApplyStaticEffect(character);

            if (!character.IsOwner)
                return;

            strengthModifier = Mathf.RoundToInt(character.characterNetwork.networkStrength.Value * Mathf.Abs(strengthMultiplier - 1.0f));
            character.characterNetwork.networkStrengthModifier.Value += strengthModifier;
        }

        public override void RemoveStaticEffect(CharacterManager character)
        {
            base.RemoveStaticEffect(character);

            if (!character.IsOwner)
                return;

            character.characterNetwork.networkStrengthModifier.Value -= strengthModifier;
        }
    }
}
