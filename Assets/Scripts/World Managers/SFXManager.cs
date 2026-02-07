using System.Collections;
using UnityEngine;

namespace FG
{
    public class SFXManager : MonoBehaviour
    {
        [HideInInspector] public static SFXManager instance;

        [Header("SFX - Actions")]
        public AudioClip rollActionSFX;
        public AudioClip backstepActionSFX;

        [Header("SFX - Attack Actions")]
        public AudioClip[] lightSwingSFX;
        public AudioClip[] heavySwingSFX;

        [Header("SFX - Damage")]
        public AudioClip[] physicalDamageSFX;

        [Header("SFX - Guard Broken")]
        public AudioClip[] guardBrokenSFX;

        [Header("SFX - Item Pick Up")]
        public AudioClip itemPickUpSoundFX;

        [Header("Music - Boss Fight")]
        [SerializeField] private AudioSource audioSourceBossFightIntro;
        [SerializeField] private AudioSource audioSourceBossFightLoop;
        [SerializeField] private float bossFightMusicFadeOut = 0.75f;
        [SerializeField] private float bossFightMusicVolume = 0.75f;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        // ----------------
        // BOSS FIGHT MUSIC
        public void PlayBossFightMusic(AudioClip musicIntro, AudioClip musicLoop)
        {
            audioSourceBossFightIntro.volume = bossFightMusicVolume;
            audioSourceBossFightIntro.clip = musicIntro;
            audioSourceBossFightIntro.Play();

            audioSourceBossFightLoop.volume = bossFightMusicVolume;
            audioSourceBossFightLoop.clip = musicLoop;
            audioSourceBossFightLoop.PlayDelayed(audioSourceBossFightIntro.clip.length);
        }

        public void StopBossFighMusic()
        {
            StartCoroutine(StopBossFightMusicCoroutine());
        }

        private IEnumerator StopBossFightMusicCoroutine()
        {
            while (audioSourceBossFightIntro.volume > 0.0f)
            {
                audioSourceBossFightIntro.volume -= Time.deltaTime * bossFightMusicFadeOut;
                audioSourceBossFightLoop.volume -= Time.deltaTime * bossFightMusicFadeOut;

                yield return null;
            }

            audioSourceBossFightIntro.Stop();
            audioSourceBossFightLoop.Stop();

            yield return null;
        }

        // -----------------------
        // SUPPLEMENTARY FUNCTIONS
        public AudioClip GetRandomSFX(ref AudioClip[] audioClips)
        {
            if (audioClips.Length <= 0)
                return null;

            int randomIndex = Random.Range(0, audioClips.Length);
            return audioClips[randomIndex];
        }
    }
}
