using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace FG
{
    public class DurkCombatManager : AICharacterCombatManager
    {
        private DurkCharacterManager aiDurk;

        [Header("Damage")]
        public float baseDamage = 30.0f;

        [Header("Multipliers")]
        [SerializeField] private float attack_01_modifier = 1.0f;
        [SerializeField] private float attack_02_modifier = 1.3f;
        [SerializeField] private float attack_03_modifier = 1.5f;

        [Header("Stomp")]
        public float stompRadius = 2.5f;
        public float attack_stomp_modifier = 1.1f;
        public int stompPoiseDamage = 15;
        public GameObject stompVFX;

        [Header("Weapon Collider")]
        [SerializeField] private DummyWeaponDamageCollider damageCollider;
        [SerializeField] private DurkStompDamageCollider stompCollider;

        protected override void Awake()
        {
            base.Awake();

            aiDurk = GetComponent<DurkCharacterManager>();
            damageCollider = GetComponentInChildren<DummyWeaponDamageCollider>();
            stompCollider = GetComponentInChildren<DurkStompDamageCollider>();
        }

        protected override void Start()
        {
            base.Start();

            damageCollider.damageDealer = aiDurk;
        }

        public override void DisableAllDamageColliders()
        {
            base.DisableAllDamageColliders();

            damageCollider.DisableCollider();
        }

        // ----------------
        // ANIMATION EVENTS - COLLIDERS
        public void ActivateDamageCollider()
        {
            damageCollider.EnableCollider();
        }

        public void DeactivateDamageCollider()
        {
            damageCollider.DisableCollider();
        }

        public void TriggerStumpCollider()
        {
            stompCollider.Trigger();
        }

        // ANIMATION EVENTS - ATTACK MODIFIERS
        public void ApplyAttack01Modifier()
        {
            aiDurk.durkSFXManager.PlayAttackGruntSoundFX();
            damageCollider.physicalDamage = (int)(baseDamage * attack_01_modifier);
        }

        public void ApplyAttack02Modifier()
        {
            aiDurk.durkSFXManager.PlayAttackGruntSoundFX();
            damageCollider.physicalDamage = (int)(baseDamage * attack_02_modifier);
        }

        public void ApplyAttack03Modifier()
        {
            aiDurk.durkSFXManager.PlayAttackGruntSoundFX();
            damageCollider.physicalDamage = (int)(baseDamage * attack_03_modifier);
        }
    }
}
