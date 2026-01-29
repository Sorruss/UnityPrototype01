using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class EffectsManager : MonoBehaviour
    {
        [HideInInspector] public static EffectsManager instance;

        [Header("VFX")]
        public GameObject bloodSplashVFX;

        [Header("Instant Effects")]
        [SerializeField] public TakeHealthDamageEffect healthDamageEffect;
        [SerializeField] public TakeHealthDamageBlockedEffect healthDamageBlockedEffect;
        [SerializeField] public TakeStaminaDamageEffect staminaDamageEffect;
        [Space]
        [SerializeField] private List<InstantEffect> instantEffects;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
            }

            GenerateInstantEffectsIDs();
        }

        private void GenerateInstantEffectsIDs()
        {
            for (int i = 0;  i < instantEffects.Count; ++i)
            {
                instantEffects[i].instantEffectID = i;
            }
        }
    }
}
