using UnityEngine;

namespace FG
{
    public class DummyWeaponDamageCollider : MeleeWeaponDamageCollider
    {
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

            // Here is target.IsOwner instead of damageDealer cause it just feels better if 
            // it's more in sync from the receiver side if AI hits him. Like AI can hit enemy from server side
            // but not to be as visible on client's side so we do it in vice-versa.
            if (target.IsOwner)
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
