using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Item/Weapon/Weapon Melee")]
    public class WeaponMeleeItem : WeaponItem
    {
        [Header("Critical Attack Multipliers")]
        public float riposteMultiplier = 2.0f;
        public float backstabMultiplier = 2.0f;
    }
}
