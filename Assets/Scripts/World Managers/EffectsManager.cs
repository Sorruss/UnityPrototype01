using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class EffectsManager : MonoBehaviour
    {
        [HideInInspector] public static EffectsManager instance;

        [Header("VFX")]
        public GameObject bloodSplashVFX;
        public GameObject criticalBloodSplashVFX;

        [Header("Instant Effects")]
        public TakeHealthDamageEffect healthDamageEffect;
        public TakeHealthDamageBlockedEffect healthDamageBlockedEffect;
        public TakeStaminaDamageEffect staminaDamageEffect;
        public TakeCriticalDamageEffect criticalDamageEffect;
        [Space]
        [SerializeField] private List<InstantEffect> instantEffects;

        [Header("Static Effects")]
        public TwoHandingStaticEffect twoHandingEffect;
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
