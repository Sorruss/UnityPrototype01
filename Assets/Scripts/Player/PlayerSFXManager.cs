using UnityEngine;

namespace FG
{
    public class PlayerSFXManager : CharacterSFXManager
    {
        private PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public override void PlayWeaponBlockFX()
        {
            base.PlayWeaponBlockFX();

            if (player.playerInventoryManager.LeftHandWeaponScriptable == null)
                return;

            PlayAudioClip(SFXManager.instance.GetRandomSFX(ref player.playerInventoryManager.LeftHandWeaponScriptable.blocksSoundFX));
        }
    }
}
