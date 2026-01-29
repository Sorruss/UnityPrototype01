using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace FG
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Collider")]
        [SerializeField] protected Collider damageCollider;

        [Header("Damage")]
        [SerializeField] public int physicalDamage;
        [SerializeField] public int holyDamage;
        [SerializeField] public int fireDamage;
        [SerializeField] public int magicDamage;
        [SerializeField] public int lightningDamage;

        [Header("Poise Damage")]
        [SerializeField] public int poiseDamage;

        [Header("Hit Info")]
        [SerializeField] public Vector3 contactPoint;
        [SerializeField] public float hitAngle;

        [Header("ActorsToDamage")]
        [SerializeField] protected List<ulong> collidedIDs;

        protected virtual void Awake()
        {
            if (damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }
        }

        private void Start()
        {
            collidedIDs = new List<ulong>();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            
            // CHECKS
            if (damageTarget == null)
                return;

            // NEEDED VALUES
            contactPoint = damageTarget.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            Vector3 vectorToThisCollider = transform.position - damageTarget.transform.position;
            hitAngle = Vector3.SignedAngle(vectorToThisCollider, damageTarget.transform.forward, Vector3.up);

            // FUNCTIONS
            CheckIfBlocking(ref damageTarget);
            DamageTarget(ref damageTarget);
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            if (damageTarget == null)
            {
                return;
            }

            collidedIDs.Remove(damageTarget.characterNetwork.OwnerClientId);
        }

        protected virtual void CheckIfBlocking(ref CharacterManager target)
        {
            if (!target.characterNetwork.networkIsBlocking.Value)
                return;

            if (hitAngle > 45.0f || hitAngle < -45)
                return;

            if (collidedIDs.Contains(target.characterNetwork.OwnerClientId))
                return;

            TakeHealthDamageBlockedEffect healthDamageBlockedEffect = Instantiate(EffectsManager.instance.healthDamageBlockedEffect);
            healthDamageBlockedEffect.physicalDamage = physicalDamage;
            healthDamageBlockedEffect.holyDamage = holyDamage;
            healthDamageBlockedEffect.fireDamage = fireDamage;
            healthDamageBlockedEffect.magicDamage = magicDamage;
            healthDamageBlockedEffect.lightningDamage = lightningDamage;
            healthDamageBlockedEffect.poiseDamage = poiseDamage;
            target.characterEffectsManager.ApplyInstantEffect(healthDamageBlockedEffect);

            collidedIDs.Add(target.characterNetwork.OwnerClientId);
        }

        protected virtual void DamageTarget(ref CharacterManager target)
        {
            if (collidedIDs.Contains(target.characterNetwork.OwnerClientId))
                return;

            TakeHealthDamageEffect healthDamageEffect = Instantiate(EffectsManager.instance.healthDamageEffect);
            healthDamageEffect.physicalDamage = physicalDamage;
            healthDamageEffect.holyDamage = holyDamage;
            healthDamageEffect.fireDamage = fireDamage;
            healthDamageEffect.magicDamage = magicDamage;
            healthDamageEffect.lightningDamage = lightningDamage;
            healthDamageEffect.poiseDamage = poiseDamage;
            target.characterEffectsManager.ApplyInstantEffect(healthDamageEffect);
        
            collidedIDs.Add(target.characterNetwork.OwnerClientId);
        }

        public void EnableCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableCollider()
        {
            damageCollider.enabled = false;
            collidedIDs.Clear();
        }
    }
}
