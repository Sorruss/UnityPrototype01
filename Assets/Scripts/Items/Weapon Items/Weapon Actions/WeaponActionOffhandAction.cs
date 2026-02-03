using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Offhand Action")]
    public class WeaponActionOffhandAction : WeaponAction
    {
        public override void TryToPerformAction(PlayerManager player, WeaponItem weapon)
        {
            base.TryToPerformAction(player, weapon);

            // STOPPERS
            if (!player.IsOwner)
                return;

            if (!player.playerCombatManager.CanBlock)
                return;

            if (player.playerNetwork.networkIsBlocking.Value)
                return;

            if (player.playerNetwork.networkIsAttacking.Value)
                return;

            player.playerNetwork.networkIsBlocking.Value = true;
        }
    }
}
