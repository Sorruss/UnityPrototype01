using UnityEngine;

namespace FG
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("VFX")]
        [SerializeField] private GameObject bloodSplashVFX;

        private void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ApplyInstantEffect(InstantEffect effect)
        {
            effect.ApplyEffect(ref character);
        }

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
