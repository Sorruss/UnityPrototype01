using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG
{
    public class ItemDatabase : MonoBehaviour
    {
        [HideInInspector] public static ItemDatabase instance;

        [Header("Special Items")]
        public WeaponMeleeItem unarmedWeapon;

        [Header("Weapons")]
        [SerializeField] private List<WeaponItem> weaponItems = new List<WeaponItem>();
        private List<Item> items = new List<Item>();

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }

            AssignIDs();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void AssignIDs()
        {
            // ADD ALL WEAPONS TO ALL ITEMS LIST
            foreach (var weapon in weaponItems)
            {
                items.Add(weapon);
            }

            // ASSIGN ID TO EVERY ITEM IN ITEMS LIST
            for (int i = 0; i < items.Count; ++i)
            {
                items[i].ID = i;
            }
        }

        public WeaponItem GetWeaponItemByID(int id)
        {
            return weaponItems.FirstOrDefault(weapon => weapon.ID == id);
        }
    }
}
