using UnityEngine;
using System.Collections.Generic;

namespace FG
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        // -------------
        // WEAPON SECTION
        [Header("Weapon's scriptables")]
        public WeaponItem LeftHandWeaponScriptable;
        public WeaponItem RightHandWeaponScriptable;
        public WeaponItem TwoHandedWeaponScriptable;

        [Header("List of weapons in quick slots")]
        public WeaponItem[] LeftHandWeaponScriptables = new WeaponItem[3];
        public WeaponItem[] RightHandWeaponScriptables = new WeaponItem[3];

        [Header("Currently active weapons indexes in quick slots")]
        public int LeftHandWeaponIndex = 0;
        public int RightHandWeaponIndex = 0;

        // -------------
        // ARMOR SECTION
        [Header("Equipped Armor Scriptables")]
        public HeadArmorItem HeadArmorScriptable;
        public ChestArmorItem ChestArmorScriptable;
        public HandArmorItem HandArmorScriptable;
        public LegArmorItem LegArmorScriptable;

        // ---------
        // INVENTORY
        [Header("Inventory")]
        public List<Item> itemsInInventory = new();

        // -------------------------
        // INVENTORY RELATED METHODS
        public void AddItemToInventory(Item item)
        {
            itemsInInventory.Add(item);
        }

        public void RemoveItemFromInventory(Item item)
        {
            itemsInInventory.Remove(item);
        }
    }
}
