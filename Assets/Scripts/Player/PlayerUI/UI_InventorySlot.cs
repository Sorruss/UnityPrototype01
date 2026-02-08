using UnityEngine;
using UnityEngine.UI;

namespace FG
{
    public class UI_InventorySlot : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private Image itemIcon;
        [SerializeField] private Image highlightedIcon;

        [Header("Debug")]
        [SerializeField] private Item item;

        public void SetItem(Item item)
        {
            this.item = item;
            if (item.Icon != null)
            {
                itemIcon.enabled = true;
                itemIcon.sprite = item.Icon;
            }
            else
            {
                itemIcon.enabled = false;
            }
        }

        public void EquipItem()
        {
            PlayerUIManager.instance.equipmentMenuManager.EquipItem(item);
        }
    }
}
