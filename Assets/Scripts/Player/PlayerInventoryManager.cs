using UnityEngine;

namespace FG
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        [Header("Weapon's scriptables")]
        public WeaponItem LeftHandWeaponScriptable;
        public WeaponItem RightHandWeaponScriptable;
        public WeaponItem TwoHandedWeaponScriptable;

        [Header("List of weapons in quick slots")]
        public WeaponItem[] LeftHandWeaponSciptables = new WeaponItem[3];
        public WeaponItem[] RightHandWeaponSciptables = new WeaponItem[3];

        [Header("Currently active weapons indexes in quick slots")]
        public int LeftHandWeaponIndex = 0;
        public int RightHandWeaponIndex = 0;
    }
}
