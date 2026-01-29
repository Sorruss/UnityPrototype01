using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class CharacterCombatManager : MonoBehaviour
    {
        // OBJECTS TO GET ON AWAKE
        private CharacterManager character;
        [HideInInspector] public Transform lockOnTransform;

        // ALL THE COMBAT RELATED STUFF
        [HideInInspector] public WeaponMeleeAttackType currentAttackTypeBeingUsed;
        [HideInInspector] public WeaponItem currentWeaponItemBeingUsed;
        [HideInInspector] public float staminaNeededForCurrentAction = 0.0f;
        [HideInInspector] public string lastAttackAnimationPerfomed;

        [Header("Current Target")]
        public CharacterManager currentTarget;

        [Header("Flags")]
        public bool isAllowedToDoRollAttack = false;
        public bool isAllowedToDoBackstepAttack = false;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
        }

        protected virtual void Start()
        {

        }

        public virtual void SetCurrentTarget(CharacterManager target)
        {
            if (!character.IsOwner)
                return;

            currentTarget = target;
            if (target != null)
                character.characterNetwork.networkTargetNetworkObejectID.Value = target.GetComponent<NetworkObject>().NetworkObjectId;
        }

        public void UpdateStaminaNeededForCurrentMove()
        {
            staminaNeededForCurrentAction = currentWeaponItemBeingUsed.baseStaminaCost;
            switch (currentAttackTypeBeingUsed)     // This variable is set from within animatorManager
            {
                case WeaponMeleeAttackType.LIGHT_ATTACK_01:
                case WeaponMeleeAttackType.LIGHT_ATTACK_02:
                    staminaNeededForCurrentAction *= currentWeaponItemBeingUsed.lightAttackStaminaCostModifier;
                    break;

                case WeaponMeleeAttackType.RUN_ATTACK_01:
                    staminaNeededForCurrentAction *= currentWeaponItemBeingUsed.runAttackStaminaCostModifier;
                    break;
                case WeaponMeleeAttackType.ROLL_ATTACK_01:
                    staminaNeededForCurrentAction *= currentWeaponItemBeingUsed.rollAttackStaminaCostModifier;
                    break;
                case WeaponMeleeAttackType.BACKSTEP_ATTACK_01:
                    staminaNeededForCurrentAction *= currentWeaponItemBeingUsed.backstepAttackStaminaCostModifier;
                    break;

                case WeaponMeleeAttackType.HEAVY_ATTACK_01:
                case WeaponMeleeAttackType.HEAVY_ATTACK_02:
                    staminaNeededForCurrentAction *= currentWeaponItemBeingUsed.heavyAttackStaminaCostModifier;
                    break;

                case WeaponMeleeAttackType.CHARGED_ATTACK_01:
                case WeaponMeleeAttackType.CHARGED_ATTACK_02:
                    staminaNeededForCurrentAction *= currentWeaponItemBeingUsed.heavyAttackStaminaCostModifier;
                    break;
            }
        }

        public virtual void DisableAllDamageColliders()
        {

        }

        // --------------------------------
        // ANIMATION EVENTS - ROTATION
        public void EnableRotation()
        {
            character.characterLocomotionManager.canRotate = true;
        }

        public void DisableRotation()
        {
            character.characterLocomotionManager.canRotate = false;
        }

        // ANIMATION EVENTS - INVINCIBILITY
        public void EnableInvincibility()
        {
            if (!character.IsOwner)
                return;

            character.characterNetwork.networkIsInvincible.Value = true;
        }

        public void DisableInvincibility()
        {
            if (!character.IsOwner)
                return;

            character.characterNetwork.networkIsInvincible.Value = false;
        }

        // ANIMATION EVENTS - ROLL ATTACK
        public void AllowToDoRollAttack()
        {
            isAllowedToDoRollAttack = true;
        }

        public void DontAllowToDoRollAttack()
        {
            isAllowedToDoRollAttack = false;
        }

        // ANIMATION EVENTS - BACKSTEP ATTACK
        public void AllowToDoBackstepAttack()
        {
            isAllowedToDoBackstepAttack = true;
        }

        public void DontAllowToDoBackstepAttack()
        {
            isAllowedToDoBackstepAttack = false;
        }

        // ANIMATION EVENTS - COMBO
        public virtual void EnableCanDoCombo()
        {

        }

        public virtual void DisableCanDoCombo()
        {

        }
    }
}
