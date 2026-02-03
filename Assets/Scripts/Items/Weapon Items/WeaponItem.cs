using UnityEngine;

namespace FG
{
    public class WeaponItem : Item
    {
        [Header("Model")]
        public GameObject ModelPrefub;

        [Header("Type")]
        public WeaponType weaponType = WeaponType.WEAPON;

        [Header("Overrider")]
        public AnimatorOverrideController animatorOverrider;

        [Header("Damage")]
        public int physicalDamage;
        public int magicDamage;
        public int fireDamage;
        public int lightningDamage;
        public int holyDamage;

        [Header("Poise Damage")]
        public int poiseDamage;

        [Header("Damage Absorbtion (0.0 - 1.0)")]
        public float physicalDamageAbsorbtion;
        public float magicDamageAbsorbtion;
        public float fireDamageAbsorbtion;
        public float lightningDamageAbsorbtion;
        public float holyDamageAbsorbtion;

        [Header("Damage Absorbtion Stability (0.0 - 1.0)")]
        public float stability = 0.5f;                          // HOW MUCH STAMINA TO DEDUCT AFTER BLOCKING

        [Header("Damage Modifiers - Light Attacks")]
        public float lightAttack01DamageModifier = 1.0f;
        public float lightAttack02DamageModifier = 1.2f;

        [Header("Damage Modifiers - Heavy Attacks")]
        public float heavyAttack01DamageModifier = 1.4f;
        public float heavyAttack02DamageModifier = 1.6f;

        [Header("Damage Modifiers - Charged Attacks")]
        public float chargedAttack01DamageModifier = 2.0f;
        public float chargedAttack02DamageModifier = 2.2f;

        [Header("Damage Modifiers - Locomotion Attacks")]
        public float runAttack01DamageModifier = 1.2f;
        public float rollAttack01DamageModifier = 1.1f;
        public float backstepAttack01DamageModifier = 1.1f;

        [Header("Restrictions")]
        public int strengthReq;
        public int intelligenceReq;
        public int dexteriteReq;

        [Header("Stamina Cost")]
        public int baseStaminaCost;
        public float lightAttackStaminaCostModifier = 1.0f;
        public float heavyAttackStaminaCostModifier = 1.3f;
        public float runAttackStaminaCostModifier = 0.9f;
        public float rollAttackStaminaCostModifier = 0.8f;
        public float backstepAttackStaminaCostModifier = 0.8f;

        [Header("Actions - Bumpers")]
        public WeaponAction OH_RB_Action; // ONE HANDED RIGHT BUMPER ACTION
        public WeaponAction OH_LB_Action; // ONE HANDED LEFT BUMPER ACTION

        [Header("Actions - Triggers")]
        public WeaponAction OH_RT_Action; // ONE HANDED RIGHT TRIGGER ACTION

        [Header("Sounds")]
        public AudioClip[] whooshesSoundFX;
        public AudioClip[] blocksSoundFX;
    }
}
