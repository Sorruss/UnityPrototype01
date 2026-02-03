using UnityEngine;

namespace FG
{
    public class WeaponManager : MonoBehaviour
    {
        private MeleeWeaponDamageCollider meleeDamageCollider;

        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void TransferWeaponValuesToCollider(CharacterManager damageDealer, ref WeaponItem weaponItem)
        {
            meleeDamageCollider.damageDealer = damageDealer;

            meleeDamageCollider.physicalDamage = weaponItem.physicalDamage;
            meleeDamageCollider.magicDamage = weaponItem.magicDamage;
            meleeDamageCollider.fireDamage = weaponItem.fireDamage;
            meleeDamageCollider.lightningDamage = weaponItem.lightningDamage;
            meleeDamageCollider.holyDamage = weaponItem.holyDamage;
            meleeDamageCollider.poiseDamage = weaponItem.poiseDamage;

            meleeDamageCollider.lightAttack01DamageModifier = weaponItem.lightAttack01DamageModifier;
            meleeDamageCollider.lightAttack02DamageModifier = weaponItem.lightAttack02DamageModifier;
            meleeDamageCollider.heavyAttack01DamageModifier = weaponItem.heavyAttack01DamageModifier;
            meleeDamageCollider.heavyAttack02DamageModifier = weaponItem.heavyAttack02DamageModifier;
            meleeDamageCollider.chargedAttack01DamageModifier = weaponItem.chargedAttack01DamageModifier;
            meleeDamageCollider.chargedAttack02DamageModifier = weaponItem.chargedAttack02DamageModifier;
            meleeDamageCollider.runAttack01DamageModifier = weaponItem.runAttack01DamageModifier;
            meleeDamageCollider.rollAttack01DamageModifier = weaponItem.rollAttack01DamageModifier;
            meleeDamageCollider.backstepAttack01DamageModifier = weaponItem.backstepAttack01DamageModifier;
        }

        public void ApplyTwoHandingBoost()
        {
            meleeDamageCollider.physicalDamage = (int)(meleeDamageCollider.physicalDamage * 1.5f);
        }

        public void ActivateDamageCollider(bool activate)
        {
            if (activate)
            {
                meleeDamageCollider.EnableCollider();
            }
            else
            {
                meleeDamageCollider.DisableCollider();
            }
        }
    }
}
