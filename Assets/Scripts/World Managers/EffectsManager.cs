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

        [Header("Static Effects")]
        [SerializeField] public TwoHandingStaticEffect twoHandingEffect;
        [Space]
        [SerializeField] private List<StaticEffect> staticEffects;

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
            GenerateStaticEffectsIDs();
        }

        // --------------
        // AUTO GENERATING IDs
        private void GenerateInstantEffectsIDs()
        {
            for (int i = 0;  i < instantEffects.Count; ++i)
            {
                instantEffects[i].instantEffectID = i;
            }
        }

        private void GenerateStaticEffectsIDs()
        {
            for (int i = 0; i < staticEffects.Count; ++i)
            {
                staticEffects[i].staticEffectID = i;
            }
        }
    }
}
