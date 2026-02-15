using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        private CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        // TRANSFORM.
        [HideInInspector] public NetworkVariable<Vector3> networkPosition = 
            new(Vector3.zero, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        [HideInInspector] public NetworkVariable<Quaternion> networkRotation = 
            new(Quaternion.identity, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        [HideInInspector] public Vector3 networkPositionVelocity;

        // SETTINGS.
        [HideInInspector] public float networkPositionSmoothTime = 0.1f;
        [HideInInspector] public float networkRotationSmoothTime = 0.1f;

        // ANIMATOR LOCOMOTION PARAMETERS.
        public NetworkVariable<float> networkHorizontal = 
            new(0.0f, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> networkVertical = 
            new(0.0f, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> networkMoveAmount = 
            new(0.0f, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsMoving =
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        // ACTION FLAGS.
        public NetworkVariable<bool> networkIsSprinting = 
            new(false, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsWalking =
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsJumping = 
            new(false, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsRolling = 
            new(false, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsBlocking =
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsAttacking =
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        // STATE FLAGS.
        public NetworkVariable<bool> networkIsDead = 
            new(false, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsChargingAttack = 
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsActive =
            new(true,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsInvincible =
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsRipostable =
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsBeingCriticallyDamaged =
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> networkIsParrying =
            new(false,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        // LOCK ON.
        [Header("LOCK ON")]
        public NetworkVariable<bool> networkIsLockedOn = 
            new(false, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<ulong> networkTargetNetworkObejectID = 
            new(0, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);

        // STATS.
        [Header("STATS")]
        public NetworkVariable<int> networkEndurance = 
            new(5, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> networkVitality =
            new(7,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> networkStrength =
            new(4,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        // STATS MODIFIERS
        [Header("STATS MODIFIERS")]
        public NetworkVariable<int> networkStrengthModifier =
            new(0,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        // RESOURCES.
        [Header("RESOURCES")]
        public NetworkVariable<int> networkMaxHealth =
            new(0,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> networkCurrentHealth =
            new(0.0f,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> networkMaxStamina = 
            new(0, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> networkCurrentStamina = 
            new(0.0f, 
                NetworkVariableReadPermission.Everyone, 
                NetworkVariableWritePermission.Owner);

        // FLAGS LISTENERS.
        public void OnIsActiveChanged(bool oldValue, bool newValue)
        {
            gameObject.SetActive(newValue);
        }

        public void OnIsDeadChanged(bool oldValue, bool newValue)
        {
            character.animator.SetBool("IsDead", newValue);
        }

        public virtual void OnIsBlockingChanged(bool oldValue, bool newValue)
        {
            character.animator.SetBool("IsBlocking", newValue);
        }

        // LOCOMOTION STATES LISTENERS.
        public void OnIsMovingChanged(bool oldValue, bool newValue)
        {
            character.animator.SetBool("IsMoving", newValue);
        }

        // LOCK ON LISTENERS.
        public void OnTargetNetworkObjectIDChanged(ulong oldValue, ulong newValue)
        {
            // IF ONE PLAYER LOCKS ON SOMEONE, OTHER PLAYERS 
            if (!IsOwner)
            {
                character.characterCombatManager.currentTarget = 
                    NetworkManager.Singleton.SpawnManager.SpawnedObjects[newValue].gameObject.GetComponent<CharacterManager>();
            }
        }

        public void OnIsLockedOnChanged(bool oldValue, bool newValue)
        {
            if (!newValue)
            {
                character.characterCombatManager.currentTarget = null;
            }
        }

        // STATES LISTENERS.
        public void OnIsChargingAttackChanged(bool oldValue, bool newValue)
        {
            character.animator.SetBool("IsChargingAttack", newValue);
        }

        // ANIMATOR ACTION RPCs.
        [ServerRpc]
        public void NotifyServerOfAnimatorActionServerRpc(ulong clientId, string actionName, bool applyRootMotion = true)
        {
            if (IsServer)
            {
                NotifyClientsOfAnimatorActionClientRpc(clientId, actionName, applyRootMotion);
            }
        }

        [ClientRpc]
        private void NotifyClientsOfAnimatorActionClientRpc(ulong clientId, string actionName, bool applyRootMotion)
        {
            if (NetworkManager.Singleton.LocalClientId != clientId)
            {
                ApplyAnimatorAction(actionName, applyRootMotion);
            }
        }

        private void ApplyAnimatorAction(string actionName, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(actionName, character.characterAnimatorManager.actionSmoothTime);
        }

        // ANIMATOR ACTION RPCs but instant (Play instead of CrossFade).
        [ServerRpc]
        public void NotifyServerOfAnimatorInstantActionServerRpc(ulong clientId, string actionName, bool applyRootMotion = true)
        {
            if (IsServer)
            {
                NotifyClientsOfAnimatorInstantActionClientRpc(clientId, actionName, applyRootMotion);
            }
        }

        [ClientRpc]
        private void NotifyClientsOfAnimatorInstantActionClientRpc(ulong clientId, string actionName, bool applyRootMotion)
        {
            if (NetworkManager.Singleton.LocalClientId != clientId)
            {
                ApplyAnimatorInstantAction(actionName, applyRootMotion);
            }
        }

        private void ApplyAnimatorInstantAction(string actionName, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.Play(actionName);
        }

        // ANIMATOR ATTACK ACTIONS RPCs.
        [ServerRpc]
        public void NotifyServerOfAnimatorAttackActionServerRpc(ulong clientId, string actionName, bool applyRootMotion = true)
        {
            if (IsServer)
            {
                NotifyClientsOfAnimatorAttackActionClientRpc(clientId, actionName, applyRootMotion);
            }
        }

        [ClientRpc]
        private void NotifyClientsOfAnimatorAttackActionClientRpc(ulong clientId, string actionName, bool applyRootMotion)
        {
            if (NetworkManager.Singleton.LocalClientId != clientId)
            {
                ApplyAnimatorAttackAction(actionName, applyRootMotion);
            }
        }

        private void ApplyAnimatorAttackAction(string actionName, bool applyRootMotion)
        {
            character.characterAnimatorManager.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(actionName, character.characterAnimatorManager.actionSmoothTime);
        }

        // NOTIFY DAMAGE RECEIVER ABOUT THE DAMAGE FROM DAMAGE DEALER RPCs.
        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        public void NotifyClientOfDamageTakenServerRpc(
            ulong damageDealerID, 
            ulong damageReceiverID, 
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            int poiseDamage,
            float hitAngle,
            float contactPointX,
            float contantPointY,
            float contactPointZ)
        {
            if (IsServer)
            {
                NotifyClientOfDamageTakenClientRpc(
                    damageDealerID,
                    damageReceiverID,
                    physicalDamage,
                    magicDamage,
                    fireDamage,
                    lightningDamage,
                    holyDamage,
                    poiseDamage,
                    hitAngle,
                    contactPointX,
                    contantPointY,
                    contactPointZ);
                }
        }

        [ClientRpc]
        private void NotifyClientOfDamageTakenClientRpc(
            ulong damageDealerID,
            ulong damageReceiverID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            int poiseDamage,
            float hitAngle,
            float contactPointX,
            float contantPointY,
            float contactPointZ)
        {
            ApplyDamage(
                damageDealerID,
                damageReceiverID,
                physicalDamage,
                magicDamage,
                fireDamage,
                lightningDamage,
                holyDamage,
                poiseDamage,
                hitAngle,
                contactPointX,
                contantPointY,
                contactPointZ);
        }

        private void ApplyDamage(
            ulong damageDealerID,
            ulong damageReceiverID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            int poiseDamage,
            float hitAngle,
            float contactPointX,
            float contantPointY,
            float contactPointZ)
        {
            // FIND NECESSARY ENTITIES.
            CharacterManager damageDealer = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageDealerID]
                .gameObject.GetComponent<CharacterManager>();
            CharacterManager damageReceiver = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageReceiverID]
                .gameObject.GetComponent<CharacterManager>();

            // CREATE TAKEHEALTHDAMAGEEFFECT AND CONFIGURE IT.
            TakeHealthDamageEffect healthDamageEffect = Instantiate(EffectsManager.instance.healthDamageEffect);

            healthDamageEffect.damageCauser = damageDealer;
            healthDamageEffect.hitAngle = hitAngle;
            healthDamageEffect.contactPoint = new Vector3(contactPointX, contantPointY, contactPointZ);

            healthDamageEffect.physicalDamage = physicalDamage;
            healthDamageEffect.magicDamage = magicDamage;
            healthDamageEffect.fireDamage = fireDamage;
            healthDamageEffect.lightningDamage = lightningDamage;
            healthDamageEffect.holyDamage = holyDamage;
            healthDamageEffect.poiseDamage = poiseDamage;

            // APPLY CREATED & CONFIGURED EFFECT ON RECEIVER PLAYER.
            damageReceiver.characterEffectsManager.ApplyInstantEffect(healthDamageEffect);
        }

        // NOTIFY BLOCKED DAMAGE RECEIVER ABOUT THE DAMAGE FROM DAMAGE DEALER RPCs.
        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        public void NotifyClientOfBlockedDamageTakenServerRpc(
            ulong damageDealerID,
            ulong damageReceiverID,

            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            int poiseDamage,

            float hitAngle,
            float contactPointX,
            float contantPointY,
            float contactPointZ)
        {
            if (IsServer)
            {
                NotifyClientOfBlockedDamageTakenClientRpc(
                    damageDealerID,
                    damageReceiverID,

                    physicalDamage,
                    magicDamage,
                    fireDamage,
                    lightningDamage,
                    holyDamage,
                    poiseDamage,

                    hitAngle,
                    contactPointX,
                    contantPointY,
                    contactPointZ);
            }
        }

        [ClientRpc]
        private void NotifyClientOfBlockedDamageTakenClientRpc(
            ulong damageDealerID,
            ulong damageReceiverID,

            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            int poiseDamage,

            float hitAngle,
            float contactPointX,
            float contantPointY,
            float contactPointZ)
        {
            ApplyBlockedDamage(
                damageDealerID,
                damageReceiverID,

                physicalDamage,
                magicDamage,
                fireDamage,
                lightningDamage,
                holyDamage,
                poiseDamage,

                hitAngle,
                contactPointX,
                contantPointY,
                contactPointZ);
        }

        private void ApplyBlockedDamage(
            ulong damageDealerID,
            ulong damageReceiverID,

            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage,
            int poiseDamage,

            float hitAngle,
            float contactPointX,
            float contantPointY,
            float contactPointZ)
        {
            // FIND NECESSARY ENTITIES.
            CharacterManager damageDealer = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageDealerID]
                .gameObject.GetComponent<CharacterManager>();
            CharacterManager damageReceiver = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageReceiverID]
                .gameObject.GetComponent<CharacterManager>();

            // CREATE TAKEHEALTHDAMAGEEFFECT AND CONFIGURE IT.
            TakeHealthDamageBlockedEffect healthDamageEffect = Instantiate(EffectsManager.instance.healthDamageBlockedEffect);

            healthDamageEffect.damageCauser = damageDealer;
            healthDamageEffect.hitAngle = hitAngle;
            healthDamageEffect.contactPoint = new Vector3(contactPointX, contantPointY, contactPointZ);

            healthDamageEffect.physicalDamage = physicalDamage;
            healthDamageEffect.magicDamage = magicDamage;
            healthDamageEffect.fireDamage = fireDamage;
            healthDamageEffect.lightningDamage = lightningDamage;
            healthDamageEffect.holyDamage = holyDamage;

            healthDamageEffect.poiseDamage = poiseDamage;
            healthDamageEffect.staminaDamage = poiseDamage;

            // APPLY CREATED & CONFIGURED EFFECT ON RECEIVER PLAYER.
            damageReceiver.characterEffectsManager.ApplyInstantEffect(healthDamageEffect);
        }

        // NOTIFY RIPOSTE
        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        public void NotifyClientOfRiposteServerRpc(
            ulong damageDealerID,
            ulong damageReceiverID,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage)
        {
            if (IsServer)
            {
                NotifyClientOfRiposteClientRpc(
                    damageDealerID,
                    damageReceiverID,
                    weaponID,
                    physicalDamage,
                    magicDamage,
                    fireDamage,
                    lightningDamage,
                    holyDamage);
            }
        }

        [ClientRpc]
        private void NotifyClientOfRiposteClientRpc(
            ulong damageDealerID,
            ulong damageReceiverID,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage)
        {
            ApplyRiposte(
                damageDealerID,
                damageReceiverID,
                weaponID,
                physicalDamage,
                magicDamage,
                fireDamage,
                lightningDamage,
                holyDamage);
        }

        private void ApplyRiposte(
            ulong damageDealerID,
            ulong damageReceiverID,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage)
        {
            // FIND NECESSARY ENTITIES.
            CharacterManager damageDealer = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageDealerID]
                .gameObject.GetComponent<CharacterManager>();
            CharacterManager damageReceiver = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageReceiverID]
                .gameObject.GetComponent<CharacterManager>();

            // CREATE TAKEHEALTHDAMAGEEFFECT AND CONFIGURE IT.
            TakeCriticalDamageEffect criticalDamageEffect = Instantiate(EffectsManager.instance.criticalDamageEffect);
            WeaponMeleeItem meleeWeapon = ItemDatabase.instance.GetWeaponItemByID(weaponID) as WeaponMeleeItem;

            criticalDamageEffect.damageCauser = damageDealer;

            criticalDamageEffect.physicalDamage = physicalDamage;
            criticalDamageEffect.magicDamage = magicDamage;
            criticalDamageEffect.fireDamage = fireDamage;
            criticalDamageEffect.lightningDamage = lightningDamage;
            criticalDamageEffect.holyDamage = holyDamage;

            criticalDamageEffect.physicalDamage *= meleeWeapon.riposteMultiplier;
            criticalDamageEffect.magicDamage *= meleeWeapon.riposteMultiplier;
            criticalDamageEffect.fireDamage *= meleeWeapon.riposteMultiplier;
            criticalDamageEffect.lightningDamage *= meleeWeapon.riposteMultiplier;
            criticalDamageEffect.holyDamage *= meleeWeapon.riposteMultiplier;

            criticalDamageEffect.manuallySelectDamageAnimation = true;
            criticalDamageEffect.damageAnimationName = UtilityManager.instance.GetRiposteVictimAnimationBasedOnWeaponClass(meleeWeapon.weaponClass);

            // APPLY CREATED & CONFIGURED EFFECT ON RECEIVER PLAYER.
            damageReceiver.characterEffectsManager.ApplyInstantEffect(criticalDamageEffect);

            if (damageReceiver.IsOwner)
                damageReceiver.characterNetwork.networkIsInvincible.Value = true;
        }

        // NOTIFY BACKSTAB
        [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
        public void NotifyClientOfBackstabServerRpc(
            ulong damageDealerID,
            ulong damageReceiverID,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage)
        {
            if (IsServer)
            {
                NotifyClientOfBackstabClientRpc(
                    damageDealerID,
                    damageReceiverID,
                    weaponID,
                    physicalDamage,
                    magicDamage,
                    fireDamage,
                    lightningDamage,
                    holyDamage);
            }
        }

        [ClientRpc]
        private void NotifyClientOfBackstabClientRpc(
            ulong damageDealerID,
            ulong damageReceiverID,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage)
        {
            ApplyBackstab(
                damageDealerID,
                damageReceiverID,
                weaponID,
                physicalDamage,
                magicDamage,
                fireDamage,
                lightningDamage,
                holyDamage);
        }

        private void ApplyBackstab(
            ulong damageDealerID,
            ulong damageReceiverID,
            int weaponID,
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float lightningDamage,
            float holyDamage)
        {
            // FIND NECESSARY ENTITIES.
            CharacterManager damageDealer = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageDealerID]
                .gameObject.GetComponent<CharacterManager>();
            CharacterManager damageReceiver = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damageReceiverID]
                .gameObject.GetComponent<CharacterManager>();

            // CREATE TAKEHEALTHDAMAGEEFFECT AND CONFIGURE IT.
            TakeCriticalDamageEffect criticalDamageEffect = Instantiate(EffectsManager.instance.criticalDamageEffect);
            WeaponMeleeItem meleeWeapon = ItemDatabase.instance.GetWeaponItemByID(weaponID) as WeaponMeleeItem;

            criticalDamageEffect.damageCauser = damageDealer;

            criticalDamageEffect.physicalDamage = physicalDamage;
            criticalDamageEffect.magicDamage = magicDamage;
            criticalDamageEffect.fireDamage = fireDamage;
            criticalDamageEffect.lightningDamage = lightningDamage;
            criticalDamageEffect.holyDamage = holyDamage;

            criticalDamageEffect.physicalDamage *= meleeWeapon.backstabMultiplier;
            criticalDamageEffect.magicDamage *= meleeWeapon.backstabMultiplier;
            criticalDamageEffect.fireDamage *= meleeWeapon.backstabMultiplier;
            criticalDamageEffect.lightningDamage *= meleeWeapon.backstabMultiplier;
            criticalDamageEffect.holyDamage *= meleeWeapon.backstabMultiplier;

            criticalDamageEffect.manuallySelectDamageAnimation = true;
            criticalDamageEffect.damageAnimationName = UtilityManager.instance.GetBackstabVictimAnimationBasedOnWeaponClass(meleeWeapon.weaponClass);

            // APPLY CREATED & CONFIGURED EFFECT ON RECEIVER PLAYER.
            damageReceiver.characterEffectsManager.ApplyInstantEffect(criticalDamageEffect);

            if (damageReceiver.IsOwner)
                damageReceiver.characterNetwork.networkIsInvincible.Value = true;
        }
    }
}
