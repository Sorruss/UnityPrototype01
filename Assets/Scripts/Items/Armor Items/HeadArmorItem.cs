using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Item/Armor/Head Piece")]
    public class HeadArmorItem : ArmorItem
    {
        [Header("Head Piece Type")]
        public HeadEquipmentType headEquipmentType;
    }
}
