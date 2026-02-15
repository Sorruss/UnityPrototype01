using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace FG
{
    public class UI_EquipmentMenuManager : MonoBehaviour
    {
        private PlayerManager player;
        [HideInInspector] public bool isEquipmentMenuOpen = false;
        [HideInInspector] public bool isInventoryMenuOpen = false;

        [Header("For Inventory Logic")]
        public EquipmentSlotType selectedSlotType;
        [SerializeField] private GameObject inventorySlotPrefab;
        [SerializeField] private Transform inventoryContent;
        public Button lastSelectedEquipmentSlotButton;

        [Header("Main Windows")]
        [SerializeField] private GameObject equipmentMenu;
        [SerializeField] private GameObject inventoryMenu;

        [Header("Left Weapon Quick Slots")]
        [SerializeField] private Image leftWeaponQuickSlot_01;
        [SerializeField] private Image leftWeaponQuickSlot_02;
        [SerializeField] private Image leftWeaponQuickSlot_03;

        [Header("Right Weapon Quick Slots")]
        [SerializeField] private Image rightWeaponQuickSlot_01;
        [SerializeField] private Image rightWeaponQuickSlot_02;
        [SerializeField] private Image rightWeaponQuickSlot_03;

        [Header("Armor Slots")]
        [SerializeField] private Image headArmorSlot;
        [SerializeField] private Image chestArmorSlot;
        [SerializeField] private Image handArmorSlot;
        [SerializeField] private Image legArmorSlot;

        // ----------
        // REFRESHERS
        private void RefreshAllIcons()
        {
            RefreshWeaponQuickSlotIcons();
            RefreshArmorIcons();
        }

        private void RefreshWeaponQuickSlotIcons()
        {
            if (player == null)
                return;

            Sprite weaponSprite = player.playerInventoryManager.LeftHandWeaponScriptables[0].Icon;
            if (weaponSprite != null)
            {
                leftWeaponQuickSlot_01.enabled = true;
                leftWeaponQuickSlot_01.sprite = weaponSprite;
            }
            else
            {
                leftWeaponQuickSlot_01.enabled = false;
            }

            weaponSprite = player.playerInventoryManager.LeftHandWeaponScriptables[1].Icon;
            if (weaponSprite != null)
            {
                leftWeaponQuickSlot_02.enabled = true;
                leftWeaponQuickSlot_02.sprite = weaponSprite;
            }
            else
            {
                leftWeaponQuickSlot_02.enabled = false;
            }

            weaponSprite = player.playerInventoryManager.LeftHandWeaponScriptables[2].Icon;
            if (weaponSprite != null)
            {
                leftWeaponQuickSlot_03.enabled = true;
                leftWeaponQuickSlot_03.sprite = weaponSprite;
            }
            else
            {
                leftWeaponQuickSlot_03.enabled = false;
            }

            weaponSprite = player.playerInventoryManager.RightHandWeaponScriptables[0].Icon;
            if (weaponSprite != null)
            {
                rightWeaponQuickSlot_01.enabled = true;
                rightWeaponQuickSlot_01.sprite = weaponSprite;
            }
            else
            {
                rightWeaponQuickSlot_01.enabled = false;
            }

            weaponSprite = player.playerInventoryManager.RightHandWeaponScriptables[1].Icon;
            if (weaponSprite != null)
            {
                rightWeaponQuickSlot_02.enabled = true;
                rightWeaponQuickSlot_02.sprite = weaponSprite;
            }
            else
            {
                rightWeaponQuickSlot_02.enabled = false;
            }

            weaponSprite = player.playerInventoryManager.RightHandWeaponScriptables[2].Icon;
            if (weaponSprite != null)
            {
                rightWeaponQuickSlot_03.enabled = true;
                rightWeaponQuickSlot_03.sprite = weaponSprite;
            }
            else
            {
                rightWeaponQuickSlot_03.enabled = false;
            }
        }

        private void RefreshArmorIcons()
        {
            if (player == null)
                return;

            // HEAD
            if (player.playerInventoryManager.HeadArmorScriptable != null)
            {
                headArmorSlot.enabled = true;
                headArmorSlot.sprite = player.playerInventoryManager.HeadArmorScriptable.Icon;
            }
            else
            {
                headArmorSlot.enabled = false;
            }

            // CHEST
            if (player.playerInventoryManager.ChestArmorScriptable != null)
            {
                chestArmorSlot.enabled = true;
                chestArmorSlot.sprite = player.playerInventoryManager.ChestArmorScriptable.Icon;
            }
            else
            {
                chestArmorSlot.enabled = false;
            }

            // HAND
            if (player.playerInventoryManager.HandArmorScriptable != null)
            {
                handArmorSlot.enabled = true;
                handArmorSlot.sprite = player.playerInventoryManager.HandArmorScriptable.Icon;
            }
            else
            {
                handArmorSlot.enabled = false;
            }

            // LEG
            if (player.playerInventoryManager.LegArmorScriptable != null)
            {
                legArmorSlot.enabled = true;
                legArmorSlot.sprite = player.playerInventoryManager.LegArmorScriptable.Icon;
            }
            else
            {
                legArmorSlot.enabled = false;
            }
        }

        // --------------
        // COMMON METHODS
        public void OpenEquipmentMenu()
        {
            // CLOSE EVERYTHING ELSE
            PlayerUIManager.instance.popUpManager.CloseAllPopUps();
            PlayerUIManager.instance.CloseAllMenus();
            PlayerUIManager.instance.hudManager.HideHUD();

            // UPDATE THIS
            if (player == null)
                player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerManager>();
            
            RefreshAllIcons();

            // ACTIVATE THIS
            equipmentMenu.SetActive(true);
            isEquipmentMenuOpen = true;
            PlayerUIManager.instance.EnableCursor(true);
            PlayerUIManager.instance.isMenuOpened = true;
        }

        public void CloseEquipmentMenu()
        {
            equipmentMenu.SetActive(false);
            isEquipmentMenuOpen = false;
            CloseInventoryMenu();
            UnselectLastEquipmentSlotButton();
            PlayerUIManager.instance.EnableCursor(false);
        }

        public void OpenInventoryMenu()
        {
            inventoryMenu.SetActive(true);
            isInventoryMenuOpen = true;
        }

        public void CloseInventoryMenu()
        {
            inventoryMenu.SetActive(false);
            isInventoryMenuOpen = false;

            SelectLastEquipmentSlotButton();
        }

        public void SelectLastEquipmentSlotButton()
        {
            if (lastSelectedEquipmentSlotButton == null)
                return;

            lastSelectedEquipmentSlotButton.Select();
            lastSelectedEquipmentSlotButton.OnSelect(null);
        }

        public void UnselectLastEquipmentSlotButton()
        {
            if (lastSelectedEquipmentSlotButton == null)
                return;

            lastSelectedEquipmentSlotButton.OnDeselect(null);
        }

        // ----------------------
        // INVENTORY MENU METHODS
        public void CleanInventoryUI()
        {
            foreach (Transform child in inventoryContent)
                Destroy(child.gameObject);
        }

        public void LoadWeaponsToInventoryUI()
        {
            // CLEAN PREVIOUSLY POPULATED INVENTORY UI
            CleanInventoryUI();
            OpenInventoryMenu();

            // FIND ALL THE WEAPON ITEMS IN PLAYER'S INVENTORY
            List<WeaponItem> weapons = new List<WeaponItem>();
            foreach (Item item in player.playerInventoryManager.itemsInInventory)
            {
                WeaponItem weapon = item as WeaponItem;
                if (weapon == null)
                    continue;

                weapons.Add(weapon);
            }

            // POPULATE INVENTORY UI
            bool firstElement = true;
            foreach (WeaponItem weapon in weapons)
            {
                GameObject inventorySlotInstance = Instantiate(inventorySlotPrefab, inventoryContent);
                inventorySlotInstance.GetComponent<UI_InventorySlot>().SetItem(weapon);

                if (firstElement)
                {
                    firstElement = false;
                    inventorySlotInstance.GetComponent<Button>().Select();
                    inventorySlotInstance.GetComponent<Button>().OnSelect(null);
                }
            }
        }

        public void LoadHeadArmorToInventoryUI()
        {
            // CLEAN PREVIOUSLY POPULATED INVENTORY UI
            CleanInventoryUI();
            OpenInventoryMenu();

            // FIND ALL THE HEAD ARMOR ITEMS IN PLAYER'S INVENTORY
            List<HeadArmorItem> helmets = new List<HeadArmorItem>();
            foreach (Item item in player.playerInventoryManager.itemsInInventory)
            {
                HeadArmorItem helmet = item as HeadArmorItem;
                if (helmet == null)
                    continue;

                helmets.Add(helmet);
            }

            // POPULATE INVENTORY UI
            bool firstElement = true;
            foreach (HeadArmorItem helmet in helmets)
            {
                GameObject inventorySlotInstance = Instantiate(inventorySlotPrefab, inventoryContent);
                inventorySlotInstance.GetComponent<UI_InventorySlot>().SetItem(helmet);

                if (firstElement)
                {
                    firstElement = false;
                    inventorySlotInstance.GetComponent<Button>().Select();
                    inventorySlotInstance.GetComponent<Button>().OnSelect(null);
                }
            }
        }

        public void LoadChestArmorToInventoryUI()
        {
            // CLEAN PREVIOUSLY POPULATED INVENTORY UI
            CleanInventoryUI();
            OpenInventoryMenu();

            // FIND ALL THE CHEST ARMOR ITEMS IN PLAYER'S INVENTORY
            List<ChestArmorItem> chestplates = new List<ChestArmorItem>();
            foreach (Item item in player.playerInventoryManager.itemsInInventory)
            {
                ChestArmorItem chestplaet = item as ChestArmorItem;
                if (chestplaet == null)
                    continue;

                chestplates.Add(chestplaet);
            }

            // POPULATE INVENTORY UI
            bool firstElement = true;
            foreach (ChestArmorItem chestplate in chestplates)
            {
                GameObject inventorySlotInstance = Instantiate(inventorySlotPrefab, inventoryContent);
                inventorySlotInstance.GetComponent<UI_InventorySlot>().SetItem(chestplate);

                if (firstElement)
                {
                    firstElement = false;
                    inventorySlotInstance.GetComponent<Button>().Select();
                    inventorySlotInstance.GetComponent<Button>().OnSelect(null);
                }
            }
        }

        public void LoadHandArmorToInventoryUI()
        {
            // CLEAN PREVIOUSLY POPULATED INVENTORY UI
            CleanInventoryUI();
            OpenInventoryMenu();

            // FIND ALL THE HAND ARMOR ITEMS IN PLAYER'S INVENTORY
            List<HandArmorItem> gauntlets = new List<HandArmorItem>();
            foreach (Item item in player.playerInventoryManager.itemsInInventory)
            {
                HandArmorItem gauntlet = item as HandArmorItem;
                if (gauntlet == null)
                    continue;

                gauntlets.Add(gauntlet);
            }

            // POPULATE INVENTORY UI
            bool firstElement = true;
            foreach (HandArmorItem gauntlet in gauntlets)
            {
                GameObject inventorySlotInstance = Instantiate(inventorySlotPrefab, inventoryContent);
                inventorySlotInstance.GetComponent<UI_InventorySlot>().SetItem(gauntlet);

                if (firstElement)
                {
                    firstElement = false;
                    inventorySlotInstance.GetComponent<Button>().Select();
                    inventorySlotInstance.GetComponent<Button>().OnSelect(null);
                }
            }
        }

        public void LoadLegArmorToInventoryUI()
        {
            // CLEAN PREVIOUSLY POPULATED INVENTORY UI
            CleanInventoryUI();
            OpenInventoryMenu();

            // FIND ALL THE LEG  ARMOR ITEMS IN PLAYER'S INVENTORY
            List<LegArmorItem> leggins = new List<LegArmorItem>();
            foreach (Item item in player.playerInventoryManager.itemsInInventory)
            {
                LegArmorItem leggin = item as LegArmorItem;
                if (leggin == null)
                    continue;

                leggins.Add(leggin);
            }

            // POPULATE INVENTORY UI
            bool firstElement = true;
            foreach (LegArmorItem leggin in leggins)
            {
                GameObject inventorySlotInstance = Instantiate(inventorySlotPrefab, inventoryContent);
                inventorySlotInstance.GetComponent<UI_InventorySlot>().SetItem(leggin);

                if (firstElement)
                {
                    firstElement = false;
                    inventorySlotInstance.GetComponent<Button>().Select();
                    inventorySlotInstance.GetComponent<Button>().OnSelect(null);
                }
            }
        }

        // ------------
        // MAIN METHODS
        public void EquipItem(Item item)
        {
            switch (selectedSlotType)
            {
                case EquipmentSlotType.WEAPON_QUICKSLOT_LEFT_01:
                    // 1. REMOVE ITEM WE WANT TO EQUIP FROM INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(item);

                    // 2. ADD ITEM THAT WAS EQUIPPED TO INVENTORY
                    if (player.playerInventoryManager.LeftHandWeaponScriptables[0].ID != ItemDatabase.instance.unarmedWeapon.ID)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.LeftHandWeaponScriptables[0]);

                    // 3. WHAT'S BELOW
                    // WE NEED TO EXCHANGE HIS CURRENT WEAPON W/THIS ONE
                    player.playerInventoryManager.LeftHandWeaponScriptables[0] = item as WeaponItem;

                    // IF PLAYER CURRENTLY USING THIS SLOT, WE NEED TO LOAD MODEL IN
                    if (player.playerInventoryManager.LeftHandWeaponIndex == 0)
                        player.playerNetwork.networkLeftHandWeaponID.Value = item.ID;

                    // REFRESH EQUIPMENT ICONS
                    RefreshWeaponQuickSlotIcons();
                    break;
                case EquipmentSlotType.WEAPON_QUICKSLOT_LEFT_02:
                    // 1. REMOVE ITEM WE WANT TO EQUIP FROM INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(item);

                    // 2. ADD ITEM THAT WAS EQUIPPED TO INVENTORY
                    if (player.playerInventoryManager.LeftHandWeaponScriptables[1].ID != ItemDatabase.instance.unarmedWeapon.ID)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.LeftHandWeaponScriptables[1]);

                    // 3. WHAT'S BELOW
                    // WE NEED TO EXCHANGE HIS CURRENT WEAPON W/THIS ONE
                    player.playerInventoryManager.LeftHandWeaponScriptables[1] = item as WeaponItem;

                    // IF PLAYER CURRENTLY USING THIS SLOT, WE NEED TO LOAD MODEL IN
                    if (player.playerInventoryManager.LeftHandWeaponIndex == 1)
                        player.playerNetwork.networkLeftHandWeaponID.Value = item.ID;

                    // REFRESH EQUIPMENT ICONS
                    RefreshWeaponQuickSlotIcons();
                    break;
                case EquipmentSlotType.WEAPON_QUICKSLOT_LEFT_03:
                    // 1. REMOVE ITEM WE WANT TO EQUIP FROM INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(item);

                    // 2. ADD ITEM THAT WAS EQUIPPED TO INVENTORY
                    if (player.playerInventoryManager.LeftHandWeaponScriptables[2].ID != ItemDatabase.instance.unarmedWeapon.ID)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.LeftHandWeaponScriptables[2]);

                    // 3. WHAT'S BELOW
                    // WE NEED TO EXCHANGE HIS CURRENT WEAPON W/THIS ONE
                    player.playerInventoryManager.LeftHandWeaponScriptables[2] = item as WeaponItem;

                    // IF PLAYER CURRENTLY USING THIS SLOT, WE NEED TO LOAD MODEL IN
                    if (player.playerInventoryManager.LeftHandWeaponIndex == 2)
                        player.playerNetwork.networkLeftHandWeaponID.Value = item.ID;

                    // REFRESH EQUIPMENT ICONS
                    RefreshWeaponQuickSlotIcons();
                    break;
                case EquipmentSlotType.WEAPON_QUICKSLOT_RIGHT_01:
                    // 1. REMOVE ITEM WE WANT TO EQUIP FROM INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(item);

                    // 2. ADD ITEM THAT WAS EQUIPPED TO INVENTORY
                    if (player.playerInventoryManager.RightHandWeaponScriptables[0].ID != ItemDatabase.instance.unarmedWeapon.ID)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.RightHandWeaponScriptables[0]);

                    // 3. WHAT'S BELOW
                    // WE NEED TO EXCHANGE HIS CURRENT WEAPON W/THIS ONE
                    player.playerInventoryManager.RightHandWeaponScriptables[0] = item as WeaponItem;

                    // IF PLAYER CURRENTLY USING THIS SLOT, WE NEED TO LOAD MODEL IN
                    if (player.playerInventoryManager.RightHandWeaponIndex == 0)
                        player.playerNetwork.networkRightHandWeaponID.Value = item.ID;

                    // REFRESH EQUIPMENT ICONS
                    RefreshWeaponQuickSlotIcons();
                    break;
                case EquipmentSlotType.WEAPON_QUICKSLOT_RIGHT_02:
                    // 1. REMOVE ITEM WE WANT TO EQUIP FROM INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(item);

                    // 2. ADD ITEM THAT WAS EQUIPPED TO INVENTORY
                    if (player.playerInventoryManager.RightHandWeaponScriptables[1].ID != ItemDatabase.instance.unarmedWeapon.ID)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.RightHandWeaponScriptables[1]);

                    // 3. WHAT'S BELOW
                    // WE NEED TO EXCHANGE HIS CURRENT WEAPON W/THIS ONE
                    player.playerInventoryManager.RightHandWeaponScriptables[1] = item as WeaponItem;

                    // IF PLAYER CURRENTLY USING THIS SLOT, WE NEED TO LOAD MODEL IN
                    if (player.playerInventoryManager.RightHandWeaponIndex == 1)
                        player.playerNetwork.networkRightHandWeaponID.Value = item.ID;

                    // REFRESH EQUIPMENT ICONS
                    RefreshWeaponQuickSlotIcons();
                    break;
                case EquipmentSlotType.WEAPON_QUICKSLOT_RIGHT_03:
                    // 1. REMOVE ITEM WE WANT TO EQUIP FROM INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(item);

                    // 2. ADD ITEM THAT WAS EQUIPPED TO INVENTORY
                    if (player.playerInventoryManager.RightHandWeaponScriptables[2].ID != ItemDatabase.instance.unarmedWeapon.ID)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.RightHandWeaponScriptables[2]);

                    // 3. WHAT'S BELOW
                    // WE NEED TO EXCHANGE HIS CURRENT WEAPON W/THIS ONE
                    player.playerInventoryManager.RightHandWeaponScriptables[2] = item as WeaponItem;

                    // IF PLAYER CURRENTLY USING THIS SLOT, WE NEED TO LOAD MODEL IN
                    if (player.playerInventoryManager.RightHandWeaponIndex == 2)
                        player.playerNetwork.networkRightHandWeaponID.Value = item.ID;

                    // REFRESH EQUIPMENT ICONS
                    RefreshWeaponQuickSlotIcons();
                    break;
                case EquipmentSlotType.HEAD_ARMOR:
                    // 1. REMOVE ITEM WE WANT TO EQUIP FROM INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(item);

                    // 2. ADD ITEM THAT WAS EQUIPPED TO INVENTORY
                    if (player.playerInventoryManager.HeadArmorScriptable != null)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.HeadArmorScriptable);

                    // 3. WHAT'S BELOW
                    // WE NEED TO EXCHANGE HIS CURRENT ARMOR W/THIS ONE
                    player.playerInventoryManager.HeadArmorScriptable = item as HeadArmorItem;

                    // WE NEED TO LOAD MODEL IN
                    player.playerEquipmentManager.EquipHeadArmor(player.playerInventoryManager.HeadArmorScriptable);
                    player.playerNetwork.networkArmorHelmetID.Value = item.ID;

                    // REFRESH EQUIPMENT ICONS
                    RefreshArmorIcons();
                    break;
                case EquipmentSlotType.CHEST_ARMOR:
                    // 1. REMOVE ITEM WE WANT TO EQUIP FROM INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(item);

                    // 2. ADD ITEM THAT WAS EQUIPPED TO INVENTORY
                    if (player.playerInventoryManager.ChestArmorScriptable != null)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.ChestArmorScriptable);

                    // 3. WHAT'S BELOW
                    // WE NEED TO EXCHANGE HIS CURRENT ARMOR W/THIS ONE
                    player.playerInventoryManager.ChestArmorScriptable = item as ChestArmorItem;

                    // WE NEED TO LOAD MODEL IN
                    player.playerEquipmentManager.EquipChestArmor(player.playerInventoryManager.ChestArmorScriptable);
                    player.playerNetwork.networkArmorChestplateID.Value = item.ID;

                    // REFRESH EQUIPMENT ICONS
                    RefreshArmorIcons();
                    break;
                case EquipmentSlotType.HAND_ARMOR:
                    // 1. REMOVE ITEM WE WANT TO EQUIP FROM INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(item);

                    // 2. ADD ITEM THAT WAS EQUIPPED TO INVENTORY
                    if (player.playerInventoryManager.HandArmorScriptable != null)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.HandArmorScriptable);

                    // 3. WHAT'S BELOW
                    // WE NEED TO EXCHANGE HIS CURRENT ARMOR W/THIS ONE
                    player.playerInventoryManager.HandArmorScriptable = item as HandArmorItem;

                    // WE NEED TO LOAD MODEL IN
                    player.playerEquipmentManager.EquipHandArmor(player.playerInventoryManager.HandArmorScriptable);
                    player.playerNetwork.networkArmorGauntletsID.Value = item.ID;

                    // REFRESH EQUIPMENT ICONS
                    RefreshArmorIcons();
                    break;
                case EquipmentSlotType.LEG_ARMOR:
                    // 1. REMOVE ITEM WE WANT TO EQUIP FROM INVENTORY
                    player.playerInventoryManager.RemoveItemFromInventory(item);

                    // 2. ADD ITEM THAT WAS EQUIPPED TO INVENTORY
                    if (player.playerInventoryManager.LegArmorScriptable != null)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.LegArmorScriptable);

                    // 3. WHAT'S BELOW
                    // WE NEED TO EXCHANGE HIS CURRENT ARMOR W/THIS ONE
                    player.playerInventoryManager.LegArmorScriptable = item as LegArmorItem;

                    // WE NEED TO LOAD MODEL IN
                    player.playerEquipmentManager.EquipLegArmor(player.playerInventoryManager.LegArmorScriptable);
                    player.playerNetwork.networkArmorLegginsID.Value = item.ID;

                    // REFRESH EQUIPMENT ICONS
                    RefreshArmorIcons();
                    break;
                default:
                    break;
            }

            CloseInventoryMenu();
        }

        public void UnequipItem()
        {
            // IT MEANS WE DON'T HAVE SELECTED SLOT RIGHT NOW, SO CAN'T UNEQUIP
            if (selectedSlotType == EquipmentSlotType.NONE)
                return;

            switch (selectedSlotType)
            {
                case EquipmentSlotType.WEAPON_QUICKSLOT_LEFT_01:
                    // NEED TO ADD THE ITEM BACK TO THE INVENTORY
                    if (player.playerInventoryManager.LeftHandWeaponScriptables[0].ID != ItemDatabase.instance.unarmedWeapon.ID)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.LeftHandWeaponScriptables[0]);

                    // WE NEED TO SET UNARMED AS IS PER TRADITION
                    player.playerInventoryManager.LeftHandWeaponScriptables[0] = Instantiate(ItemDatabase.instance.unarmedWeapon);

                    if (player.playerInventoryManager.LeftHandWeaponIndex == 0)
                    {
                        // IF PLAYER CURRENTLY USING THIS SLOT, WE NEED TO LOAD MODEL IN
                        player.playerNetwork.networkLeftHandWeaponID.Value = 
                            player.playerInventoryManager.LeftHandWeaponScriptables[0].ID;
                    }

                    // REFRESH EQUIPMENT ICONS
                    RefreshWeaponQuickSlotIcons();

                    break;
                case EquipmentSlotType.WEAPON_QUICKSLOT_LEFT_02:
                    // NEED TO ADD THE ITEM BACK TO THE INVENTORY
                    if (player.playerInventoryManager.LeftHandWeaponScriptables[1].ID != ItemDatabase.instance.unarmedWeapon.ID)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.LeftHandWeaponScriptables[1]);

                    // WE NEED TO SET UNARMED AS IS PER TRADITION
                    player.playerInventoryManager.LeftHandWeaponScriptables[1] = Instantiate(ItemDatabase.instance.unarmedWeapon);

                    if (player.playerInventoryManager.LeftHandWeaponIndex == 1)
                    {
                        // IF PLAYER CURRENTLY USING THIS SLOT, WE NEED TO LOAD MODEL IN
                        player.playerNetwork.networkLeftHandWeaponID.Value =
                            player.playerInventoryManager.LeftHandWeaponScriptables[0].ID;
                    }

                    // REFRESH EQUIPMENT ICONS
                    RefreshWeaponQuickSlotIcons();

                    break;
                case EquipmentSlotType.WEAPON_QUICKSLOT_LEFT_03:
                    // NEED TO ADD THE ITEM BACK TO THE INVENTORY
                    if (player.playerInventoryManager.LeftHandWeaponScriptables[2].ID != ItemDatabase.instance.unarmedWeapon.ID)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.LeftHandWeaponScriptables[2]);

                    // WE NEED TO SET UNARMED AS IS PER TRADITION
                    player.playerInventoryManager.LeftHandWeaponScriptables[2] = Instantiate(ItemDatabase.instance.unarmedWeapon);

                    if (player.playerInventoryManager.LeftHandWeaponIndex == 2)
                    {
                        // IF PLAYER CURRENTLY USING THIS SLOT, WE NEED TO LOAD MODEL IN
                        player.playerNetwork.networkLeftHandWeaponID.Value =
                            player.playerInventoryManager.LeftHandWeaponScriptables[0].ID;
                    }

                    // REFRESH EQUIPMENT ICONS
                    RefreshWeaponQuickSlotIcons();

                    break;
                case EquipmentSlotType.WEAPON_QUICKSLOT_RIGHT_01:
                    // NEED TO ADD THE ITEM BACK TO THE INVENTORY
                    if (player.playerInventoryManager.RightHandWeaponScriptables[0].ID != ItemDatabase.instance.unarmedWeapon.ID)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.RightHandWeaponScriptables[0]);

                    // WE NEED TO SET UNARMED AS IS PER TRADITION
                    player.playerInventoryManager.RightHandWeaponScriptables[0] = Instantiate(ItemDatabase.instance.unarmedWeapon);

                    if (player.playerInventoryManager.RightHandWeaponIndex == 0)
                    {
                        // IF PLAYER CURRENTLY USING THIS SLOT, WE NEED TO LOAD MODEL IN
                        player.playerNetwork.networkRightHandWeaponID.Value =
                            player.playerInventoryManager.RightHandWeaponScriptables[0].ID;
                    }

                    // REFRESH EQUIPMENT ICONS
                    RefreshWeaponQuickSlotIcons();

                    break;
                case EquipmentSlotType.WEAPON_QUICKSLOT_RIGHT_02:
                    // NEED TO ADD THE ITEM BACK TO THE INVENTORY
                    if (player.playerInventoryManager.RightHandWeaponScriptables[1].ID != ItemDatabase.instance.unarmedWeapon.ID)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.RightHandWeaponScriptables[1]);

                    // WE NEED TO SET UNARMED AS IS PER TRADITION
                    player.playerInventoryManager.RightHandWeaponScriptables[1] = Instantiate(ItemDatabase.instance.unarmedWeapon);

                    if (player.playerInventoryManager.RightHandWeaponIndex == 1)
                    {
                        // IF PLAYER CURRENTLY USING THIS SLOT, WE NEED TO LOAD MODEL IN
                        player.playerNetwork.networkRightHandWeaponID.Value =
                            player.playerInventoryManager.RightHandWeaponScriptables[0].ID;
                    }

                    // REFRESH EQUIPMENT ICONS
                    RefreshWeaponQuickSlotIcons();

                    break;
                case EquipmentSlotType.WEAPON_QUICKSLOT_RIGHT_03:
                    // NEED TO ADD THE ITEM BACK TO THE INVENTORY
                    if (player.playerInventoryManager.RightHandWeaponScriptables[2].ID != ItemDatabase.instance.unarmedWeapon.ID)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.RightHandWeaponScriptables[2]);

                    // WE NEED TO SET UNARMED AS IS PER TRADITION
                    player.playerInventoryManager.RightHandWeaponScriptables[2] = Instantiate(ItemDatabase.instance.unarmedWeapon);

                    if (player.playerInventoryManager.RightHandWeaponIndex == 2)
                    {
                        // IF PLAYER CURRENTLY USING THIS SLOT, WE NEED TO LOAD MODEL IN
                        player.playerNetwork.networkRightHandWeaponID.Value =
                            player.playerInventoryManager.RightHandWeaponScriptables[0].ID;
                    }

                    // REFRESH EQUIPMENT ICONS
                    RefreshWeaponQuickSlotIcons();

                    break;
                case EquipmentSlotType.HEAD_ARMOR:
                    // NEED TO ADD THE ITEM BACK TO THE INVENTORY
                    if (player.playerInventoryManager.HeadArmorScriptable != null)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.HeadArmorScriptable);

                    // WE NEED TO REMOVE THIS ARMOR
                    player.playerEquipmentManager.EquipHeadArmor(null);

                    // REFRESH EQUIPMENT ICONS
                    RefreshArmorIcons();

                    break;
                case EquipmentSlotType.CHEST_ARMOR:
                    // NEED TO ADD THE ITEM BACK TO THE INVENTORY
                    if (player.playerInventoryManager.ChestArmorScriptable != null)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.ChestArmorScriptable);

                    // WE NEED TO REMOVE THIS ARMOR
                    player.playerEquipmentManager.EquipChestArmor(null);

                    // REFRESH EQUIPMENT ICONS
                    RefreshArmorIcons();

                    break;
                case EquipmentSlotType.HAND_ARMOR:
                    // NEED TO ADD THE ITEM BACK TO THE INVENTORY
                    if (player.playerInventoryManager.HandArmorScriptable != null)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.HandArmorScriptable);

                    // WE NEED TO REMOVE THIS ARMOR
                    player.playerEquipmentManager.EquipHandArmor(null);

                    // REFRESH EQUIPMENT ICONS
                    RefreshArmorIcons();

                    break;
                case EquipmentSlotType.LEG_ARMOR:
                    // NEED TO ADD THE ITEM BACK TO THE INVENTORY
                    if (player.playerInventoryManager.LegArmorScriptable != null)
                        player.playerInventoryManager.AddItemToInventory(player.playerInventoryManager.LegArmorScriptable);

                    // WE NEED TO REMOVE THIS ARMOR
                    player.playerEquipmentManager.EquipLegArmor(null);

                    // REFRESH EQUIPMENT ICONS
                    RefreshArmorIcons();

                    break;
                default:
                    break;
            }
        }
    }
}
