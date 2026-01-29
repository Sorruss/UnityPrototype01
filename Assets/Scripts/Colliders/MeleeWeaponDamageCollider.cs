using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Damage Dealer")]
        public CharacterManager damageDealer;

        [Header("Attack Modifiers")]
        public float lightAttack01DamageModifier;
        public float lightAttack02DamageModifier;

        public float heavyAttack01DamageModifier;
        public float heavyAttack02DamageModifier;

        public float chargedAttack01DamageModifier;
        public float chargedAttack02DamageModifier;

        public float runAttack01DamageModifier;
        public float rollAttack01DamageModifier;
        public float backstepAttack01DamageModifier;

        protected override void Awake()
        {
            base.Awake();

            damageCollider.enabled = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            
            // STOPS.
            if (damageTarget == null)
                return;

            if (damageTarget.NetworkObjectId == damageDealer.NetworkObjectId)
                return;

            // REQUIRED INFORMATION.
            contactPoint = damageTarget.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            Vector3 vectorToDamageDealer = damageDealer.transform.position - damageTarget.transform.position;
            hitAngle = Vector3.SignedAngle(vectorToDamageDealer, damageTarget.transform.forward, Vector3.up);

            // APPLY MODIFIERS AND SEND REQUEST TO DAMAGE TARGET.
            CheckIfBlocking(ref damageTarget);
            DamageTarget(ref damageTarget);
        }

        protected override void CheckIfBlocking(ref CharacterManager target)
        {
            if (!target.characterNetwork.networkIsBlocking.Value)
                return;

            if (hitAngle > 45.0f || hitAngle < -45)
                return;

            if (collidedIDs.Contains(target.characterNetwork.OwnerClientId))
                return;

            // TEMPORARY INSTANTEFFECT INSTANCE JUST TO MODIFY VALUES WITHOUT CHANGING COLLIDER'S ONES
            TakeHealthDamageBlockedEffect damageEffect = Instantiate(EffectsManager.instance.healthDamageBlockedEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;

            // APPLY MODIFIER BASED ON ATTACK TYPE
            switch (damageDealer.characterCombatManager.currentAttackTypeBeingUsed)
            {
                case WeaponMeleeAttackType.LIGHT_ATTACK_01:
                    ApplyAttackModifier(damageEffect, lightAttack01DamageModifier);
                    break;
                case WeaponMeleeAttackType.LIGHT_ATTACK_02:
                    ApplyAttackModifier(damageEffect, lightAttack02DamageModifier);
                    break;

                case WeaponMeleeAttackType.RUN_ATTACK_01:
                    ApplyAttackModifier(damageEffect, runAttack01DamageModifier);
                    break;
                case WeaponMeleeAttackType.ROLL_ATTACK_01:
                    ApplyAttackModifier(damageEffect, rollAttack01DamageModifier);
                    break;
                case WeaponMeleeAttackType.BACKSTEP_ATTACK_01:
                    ApplyAttackModifier(damageEffect, backstepAttack01DamageModifier);
                    break;

                case WeaponMeleeAttackType.HEAVY_ATTACK_01:
                    ApplyAttackModifier(damageEffect, heavyAttack01DamageModifier);
                    break;
                case WeaponMeleeAttackType.HEAVY_ATTACK_02:
                    ApplyAttackModifier(damageEffect, heavyAttack02DamageModifier);
                    break;

                case WeaponMeleeAttackType.CHARGED_ATTACK_01:
                    ApplyAttackModifier(damageEffect, chargedAttack01DamageModifier);
                    break;
                case WeaponMeleeAttackType.CHARGED_ATTACK_02:
                    ApplyAttackModifier(damageEffect, chargedAttack02DamageModifier);
                    break;
            }

            if (damageDealer.IsOwner)
            // WE CHECK THIS TO PREVENT SENDING 2 DAMAGE REQUESTS TO DAMAGERECEIVER
            {
                // SEND REQUEST TO THE TARGET ABOUT THE DAMAGE TO TAKE
                target.characterNetwork.NotifyClientOfBlockedDamageTakenServerRpc(
                    damageDealer.NetworkObjectId,           // PLAYER'S IDs
                    target.NetworkObjectId,
                    damageEffect.physicalDamage,            // DAMAGE VALUES INFO
                    damageEffect.magicDamage,
                    damageEffect.fireDamage,
                    damageEffect.lightningDamage,
                    damageEffect.holyDamage,
                    damageEffect.poiseDamage,
                    hitAngle,                               // DAMAGE HIT ANGLE
                    contactPoint.x,                         // DAMAGE CONTACT POINT
                    contactPoint.y,
                    contactPoint.z);
            }

            // ADD TARGET TO COLLIDED IDs TO PREVENT MULTIPLE HITS IN 1 ATTACK
            collidedIDs.Add(target.characterNetwork.NetworkObjectId);
        }

        private void ApplyAttackModifier(TakeHealthDamageEffect effect, float modifier)
        {
            effect.physicalDamage *= modifier;
            effect.magicDamage *= modifier;
            effect.fireDamage *= modifier;
            effect.lightningDamage *= modifier;
            effect.holyDamage *= modifier;
            effect.poiseDamage *= modifier;
        }

        private void ApplyAttackModifier(TakeHealthDamageBlockedEffect effect, float modifier)
        {
            effect.physicalDamage *= modifier;
            effect.magicDamage *= modifier;
            effect.fireDamage *= modifier;
            effect.lightningDamage *= modifier;
            effect.holyDamage *= modifier;
            effect.poiseDamage *= modifier;
        }

        protected override void DamageTarget(ref CharacterManager target)
        {
            if (collidedIDs.Contains(target.characterNetwork.NetworkObjectId))
                return;

            // TEMPORARY INSTANTEFFECT INSTANCE JUST TO MODIFY VALUES WITHOUT CHANGING COLLIDER'S ONES
            TakeHealthDamageEffect damageEffect = Instantiate(EffectsManager.instance.healthDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;

            // APPLY MODIFIER BASED ON ATTACK TYPE
            switch (damageDealer.characterCombatManager.currentAttackTypeBeingUsed)
            {
                case WeaponMeleeAttackType.LIGHT_ATTACK_01:
                    ApplyAttackModifier(damageEffect, lightAttack01DamageModifier);
                    break;
                case WeaponMeleeAttackType.LIGHT_ATTACK_02:
                    ApplyAttackModifier(damageEffect, lightAttack02DamageModifier);
                    break;

                case WeaponMeleeAttackType.RUN_ATTACK_01:
                    ApplyAttackModifier(damageEffect, runAttack01DamageModifier);
                    break;
                case WeaponMeleeAttackType.ROLL_ATTACK_01:
                    ApplyAttackModifier(damageEffect, rollAttack01DamageModifier);
                    break;
                case WeaponMeleeAttackType.BACKSTEP_ATTACK_01:
                    ApplyAttackModifier(damageEffect, backstepAttack01DamageModifier);
                    break;

                case WeaponMeleeAttackType.HEAVY_ATTACK_01:
                    ApplyAttackModifier(damageEffect, heavyAttack01DamageModifier);
                    break;
                case WeaponMeleeAttackType.HEAVY_ATTACK_02:
                    ApplyAttackModifier(damageEffect, heavyAttack02DamageModifier);
                    break;

                case WeaponMeleeAttackType.CHARGED_ATTACK_01:
                    ApplyAttackModifier(damageEffect, chargedAttack01DamageModifier);
                    break;
                case WeaponMeleeAttackType.CHARGED_ATTACK_02:
                    ApplyAttackModifier(damageEffect, chargedAttack02DamageModifier);
                    break;
            }

            if (damageDealer.IsOwner)
            // WE CHECK THIS TO PREVENT SENDING 2 DAMAGE REQUESTS TO DAMAGERECEIVER
            {
                // SEND REQUEST TO THE TARGET ABOUT THE DAMAGE TO TAKE
                target.characterNetwork.NotifyClientOfDamageTakenServerRpc(
                    damageDealer.NetworkObjectId,           // PLAYER'S IDs
                    target.NetworkObjectId,
                    damageEffect.physicalDamage,            // DAMAGE VALUES INFO
                    damageEffect.magicDamage,
                    damageEffect.fireDamage,
                    damageEffect.lightningDamage,
                    damageEffect.holyDamage,
                    damageEffect.poiseDamage,
                    hitAngle,                               // DAMAGE HIT ANGLE
                    contactPoint.x,                         // DAMAGE CONTACT POINT
                    contactPoint.y,
                    contactPoint.z);
            }

            // ADD TARGET TO COLLIDED IDs TO PREVENT MULTIPLE HITS IN 1 ATTACK
            collidedIDs.Add(target.characterNetwork.NetworkObjectId);
        }
    }
}
