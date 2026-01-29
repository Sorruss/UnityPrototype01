using UnityEngine;

namespace FG
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("For Debug")]
        [SerializeField] private InstantEffect instantEffect;
        [SerializeField] private bool takeEffect = false;

        private void Update()
        {
            if (takeEffect)
            {
                takeEffect = false;
                InstantEffect staminaEffect = Instantiate(instantEffect);
                ApplyInstantEffect(staminaEffect);
            }
        }
    }
}
