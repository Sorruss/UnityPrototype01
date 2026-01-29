using UnityEngine;

namespace FG
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public WeaponItem LeftHandWeaponSciptable;
        public WeaponItem RightHandWeaponSciptable;

        public WeaponItem[] LeftHandWeaponSciptables = new WeaponItem[3];
        public int LeftHandWeaponIndex = 0;
        public WeaponItem[] RightHandWeaponSciptables = new WeaponItem[3];
        public int RightHandWeaponIndex = 0;
    }
}
