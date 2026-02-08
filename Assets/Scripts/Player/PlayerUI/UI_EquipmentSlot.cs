using UnityEngine;
using UnityEngine.UI;

namespace FG
{
    public class UI_EquipmentSlot : MonoBehaviour
    {
        [Header("Slot Type")]
        [SerializeField] private EquipmentSlotType slotType;

        public void SetThisSlotType()
        {
            PlayerUIManager.instance.equipmentMenuManager.selectedSlotType = slotType;
            PlayerUIManager.instance.equipmentMenuManager.lastSelectedEquipmentSlotButton = GetComponent<Button>();
        }

        public void ClearSlotType()
        {
            if (PlayerUIManager.instance.equipmentMenuManager.isInventoryMenuOpen)
                return;

            PlayerUIManager.instance.equipmentMenuManager.selectedSlotType = EquipmentSlotType.NONE;
        }
    }
}
