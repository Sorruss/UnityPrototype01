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

        }

        public virtual bool CanPerformThisWeaponArt(PlayerManager player)
        {
            return false;
        }
    }
}
