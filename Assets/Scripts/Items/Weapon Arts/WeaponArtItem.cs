using UnityEngine;

namespace FG
{
    public class WeaponArtItem : Item
    {
        [Header("Config - Parent")]
        [SerializeField] private WeaponClass[] weaponsUsableOn;
        [SerializeField] private float healthTakes;
        [SerializeField] private float staminaTakes;
        [SerializeField] private float focusTakes;

        public virtual void PerformWeaponArt(PlayerManager player)
        {
            if (!CanPerformThisWeaponArt(player))
                return;
        }

        protected virtual bool CanPerformThisWeaponArt(PlayerManager player)
        {
            return false;
        }

        protected virtual void DeductHealth(PlayerManager player)
        {
            player.playerStatsManager.DamageHealth(healthTakes);
        }

        protected virtual void DeductStamina(PlayerManager player)
        {
            player.playerStatsManager.TryDecreaseStamina(staminaTakes);
        }

        protected virtual void DeductFocus(PlayerManager player)
        {

        }
    }
}
