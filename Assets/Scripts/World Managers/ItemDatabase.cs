using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG
{
    public class ItemDatabase : MonoBehaviour
    {
        // -----------------------
        // STATIC INSTANCE SECTION
        [HideInInspector] public static ItemDatabase instance;

        // ----------------------------
        // EDITOR VIEWABLE DATA SECTION
        [Header("Special Items")]
        public WeaponMeleeItem unarmedWeapon;

        [Header("Weapons")]
        [SerializeField] private List<WeaponItem> weaponItems = new();

        [Header("Head Pieces")]
        [SerializeField] private List<HeadArmorItem> headArmorItems = new();

        [Header("Chest Pieces")]
        [SerializeField] private List<ChestArmorItem> chestArmorItems = new();

        [Header("Hand Pieces")]
        [SerializeField] private List<HandArmorItem> handArmorItems = new();

        [Header("Leg Pieces")]
        [SerializeField] private List<LegArmorItem> legArmorItems = new();

        // -----------------------
        // LOCAL VARIABLES SECTION
        // ID KEYS
        private readonly int weaponIDKey = 10000;
        private readonly int headArmorIDKey = 20000;
        private readonly int chestArmorIDKey = 30000;
        private readonly int handArmorIDKey = 40000;
        private readonly int legArmorIDKey = 50000;

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
            // WEAPONS
            for (int i = 0; i < weaponItems.Count; ++i)
                weaponItems[i].ID = weaponIDKey + i;

            // HEAD ARMOR
            for (int i = 0; i < headArmorItems.Count; ++i)
                headArmorItems[i].ID = headArmorIDKey + i;

            // CHEST ARMOR
            for (int i = 0; i < chestArmorItems.Count; ++i)
                chestArmorItems[i].ID = chestArmorIDKey + i;

            // HAND ARMOR
            for (int i = 0; i < handArmorItems.Count; ++i)
                handArmorItems[i].ID = handArmorIDKey + i;

            // LEG ARMOR
            for (int i = 0; i < legArmorItems.Count; ++i)
                legArmorItems[i].ID = legArmorIDKey + i;
        }

        // -------
        // GETTERS
        public Item GetItemByID(int id)
        {
            if (id >= weaponIDKey && id < headArmorIDKey)           // IT'S WEAPON
                return GetWeaponItemByID(id);
            else if (id >= headArmorIDKey && id < chestArmorIDKey)  // IT'S HEAD PIECE
                return GetHeadArmorItemByID(id);
            else if (id >= chestArmorIDKey && id < handArmorIDKey)  // IT'S CHEST PIECE
                return GetChestArmorItemByID(id);
            else if (id >= handArmorIDKey && id < legArmorIDKey)    // IT'S HAND PIECE
                return GetHandArmorItemByID(id);
            else if (id >= legArmorIDKey && id < 60000)             // IT'S A LEG PIECE
                return GetLegArmorItemByID(id);

            return null;
        }

        public WeaponItem GetWeaponItemByID(int id)
        {
            return weaponItems.FirstOrDefault(weapon => weapon.ID == id);
        }

        public HeadArmorItem GetHeadArmorItemByID(int id)
        {
            return headArmorItems.FirstOrDefault(headArmorItem => headArmorItem.ID == id);
        }

        public ChestArmorItem GetChestArmorItemByID(int id)
        {
            return chestArmorItems.FirstOrDefault(chestArmorItem => chestArmorItem.ID == id);
        }

        public HandArmorItem GetHandArmorItemByID(int id)
        {
            return handArmorItems.FirstOrDefault(handArmorItem => handArmorItem.ID == id);
        }

        public LegArmorItem GetLegArmorItemByID(int id)
        {
            return legArmorItems.FirstOrDefault(legArmorItem => legArmorItem.ID == id);
        }
    }
}
