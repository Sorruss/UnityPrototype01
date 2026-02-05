using UnityEngine;

namespace FG
{
    public class ArmorItem : EquipmentItem
    {
        [Header("Damage Absorbtion (0.0 - 1.0)")]
        public float physicalDamageAbsorbtion;
        public float magicDamageAbsorbtion;
        public float fireDamageAbsorbtion;
        public float lightningDamageAbsorbtion;
        public float holyDamageAbsorbtion;

        [Header("Resistance")]
        public int poisonResistance;      // IMMUNITY
        public int rotResistance;         // IMMUNITY
        public int bleedResistance;       // ROBUSTNESS
        public int frostResistance;       // ROBUSTNESS
        public int madnessResistance;     // FOCUS
        public int sleepResistance;       // FOCUS
        public int curseResistance;       // VITALITY

        [Header("Poise")]
        public int poise;

        [Header("Models")]
        public ArmorModel[] armorModels;
    }
}
