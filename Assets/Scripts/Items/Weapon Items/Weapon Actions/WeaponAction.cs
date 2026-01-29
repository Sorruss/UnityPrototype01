using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Test Action")]
    public class WeaponAction : ScriptableObject
    {
        [Header("Parent Config")]
        public int ActionID;

        public virtual void TryToPerformAction(PlayerManager player, WeaponItem weapon)
        {
            if (player.IsOwner)
            {
                player.playerNetwork.networkCurrentWeaponInUseID.Value = weapon.ID;
            }
        }
    }
}
