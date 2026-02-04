using UnityEngine;
using System.Collections.Generic;

namespace FG
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("Active Static Effects")]
        [SerializeField] private List<StaticEffect> activeStaticEffects;

        [Header("VFX")]
        [SerializeField] private GameObject bloodSplashVFX;

        private void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        // ---------------
        // INSTANT EFFECTS
        public virtual void ApplyInstantEffect(InstantEffect effect)
        {
            effect.ApplyInstantEffect(ref character);
        }
        
        // --------------
        // STATIC EFFECTS
        public virtual void ApplyStaticEffect(StaticEffect effect)
        {
            activeStaticEffects.Add(effect);
            effect.ApplyStaticEffect(character);
        }

        public virtual void RemoveStaticEffect(int staticEffectID)
        {
            Helpers.CleanUpListFromNULLs(ref activeStaticEffects);

            for (int i = activeStaticEffects.Count - 1; i >= 0; --i)
            {
                if (activeStaticEffects[i].staticEffectID != staticEffectID)
                    continue;

                activeStaticEffects[i].RemoveStaticEffect(character);
                activeStaticEffects.RemoveAt(i);
                break;
            }
        }

        // --------
        // VISUALFX
        public void PlayBloodSplashVFX(Vector3 spawnPoint, float angleDirection)
        {
            if (bloodSplashVFX == null)
            {
                bloodSplashVFX = EffectsManager.instance.bloodSplashVFX;
            }

            GameObject bloodSplash = Instantiate(bloodSplashVFX, spawnPoint, Quaternion.identity);
        }
    }
}
