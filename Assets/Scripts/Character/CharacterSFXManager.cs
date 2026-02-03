using System.Collections;
using UnityEngine;

namespace FG
{
    public class CharacterSFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        [Header("Sounds - Grunts")]
        public AudioClip[] damageGrunts;
        public AudioClip[] attackGrunts;
        public AudioClip[] deathGrunts;

        [Header("Sounds - Steps")]
        public float stepHeight = 0.05f;
        public AudioClip[] stepSoundsDirt;
        public AudioClip[] stepSoundsTiles;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // --------------
        // MAIN FUNCTIONS
        public void PlayAudioClip(AudioClip audioClip, float volume = 1.0f, bool loop = false, bool randomPitch = true, float pitchDiff = 0.1f)
        {
            if (audioSource == null)
                return;

            if (randomPitch)
                audioSource.pitch += Random.Range(-pitchDiff, pitchDiff);

            audioSource.loop = loop;
            PlayClip(audioClip, volume);
            audioSource.pitch = 1.0f;
        }

        // -------------
        // ANIMATION SFX
        protected virtual void PlayRollSoundFX()
        {
            PlayClip(SFXManager.instance.rollActionSFX);
        }

        protected virtual void PlayBackstepSoundFX()
        {
            PlayClip(SFXManager.instance.backstepActionSFX);
        }

        // ------
        // GRUNTS
        public virtual void PlayDamageGruntSoundFX()
        {
            PlayAudioClip(SFXManager.instance.GetRandomSFX(ref damageGrunts));
        }

        public virtual void PlayAttackGruntSoundFX()
        {
            PlayAudioClip(SFXManager.instance.GetRandomSFX(ref attackGrunts));
        }

        // --------------
        // WEAPON RELATED
        public virtual void PlayWeaponBlockFX()
        {

        }

        // -----
        // STEPS
        public virtual void PlayStepSoundFX()
        {
            PlayAudioClip(SFXManager.instance.GetRandomSFX(ref stepSoundsTiles), 0.7f);
        }

        // -----------------
        // METHODS - HELPERS
        protected virtual void PlayClip(AudioClip clip, float volume = 1.0f)
        {
            if (clip == null)
                return;

            audioSource.PlayOneShot(clip, volume);
        }
    }
}
