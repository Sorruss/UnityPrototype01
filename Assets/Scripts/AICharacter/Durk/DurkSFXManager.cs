using UnityEngine;

namespace FG
{
    public class DurkSFXManager : CharacterSFXManager
    {
        [Header("Special Sounds")]
        [SerializeField] private AudioClip[] stompHit;
        [SerializeField] private AudioClip[] clubWhoosh;
        [SerializeField] private AudioClip[] clubHitGround;

        public void PlayClubWhooshSoundFX()
        {
            if (clubWhoosh.Length <= 0)
                return;

            PlayAudioClip(SFXManager.instance.GetRandomSFX(ref clubWhoosh), 0.5f);
        }

        public void PlayClubHitGroundSoundFX()
        {
            if (clubWhoosh.Length <= 0)
                return;

            PlayAudioClip(SFXManager.instance.GetRandomSFX(ref clubHitGround));
        }

        public void PlayStompSoundFX()
        {
            if (clubWhoosh.Length <= 0)
                return;

            PlayAudioClip(SFXManager.instance.GetRandomSFX(ref stompHit));
        }
    }
}
