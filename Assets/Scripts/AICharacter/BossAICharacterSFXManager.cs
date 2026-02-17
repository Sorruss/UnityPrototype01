using UnityEngine;

namespace FG
{
    public class BossAICharacterSFXManager : AICharacterSFXManager
    {
        [Header("Boss - Phase Shift SFX")]
        [SerializeField] private AudioClip phaseShift01SFX;

        [Header("Boss - Music")]
        [SerializeField] private AudioClip musicIntro;
        [SerializeField] private AudioClip musicPhase01;

        public void PlayShiftPhase01SoundFX()
        {
            PlayAudioClip(phaseShift01SFX);
        }

        public void PlayBossMusic()
        {
            SFXManager.instance.PlayBossFightMusic(musicIntro, musicPhase01);
        }
    }
}
