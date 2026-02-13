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
        [HideInInspector] public float staminaNeededForCurrentAction;

        [Header("Current Target")]
        public CharacterManager currentTarget;
        
        [Header("Riposte")]
        public float pendingCriticalDamage;
        [SerializeField] private float riposteDistance = 0.7f;
        [SerializeField] protected Transform riposteTargetPosition;

        [Header("Backstab")]
        [SerializeField] private bool isBackstabable = true;
        [SerializeField] private float backstabDistance = 0.7f;
        [SerializeField] protected Transform backstabTargetPosition;

        [Header("Flags")]
        public bool isAllowedToDoRollAttack = false;
        public bool isAllowedToDoBackstepAttack = false;

        [Header("Saved values")]
        public string lastAttackAnimationPerfomed;
        public WeaponMeleeAttackType currentAttackTypeBeingUsed;
        public WeaponItem currentWeaponItemBeingUsed;
        public int lastPoiseDamageTaken;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
        }

        protected virtual void Start()
        {

        }

        //-------------
        // COMBAT STUFF
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

        // -------
        // RIPOSTE
        public void AttemptRiposte(WeaponClass weaponClass)
        {
            // 1. RAYCASTING TO FIND THE TARGET
            RaycastHit[] potentialTargets = Physics.RaycastAll(
                lockOnTransform.position,
                character.transform.TransformDirection(Vector3.forward),
                riposteDistance,
                UtilityManager.instance.GetCharacterMasks());

            // 2. CHECK IF TARGET IS SUITABLE
            CharacterManager target = null;
            foreach (RaycastHit potentialTarget in potentialTargets)
            {
                CharacterManager potentialTargetCharacter = potentialTarget.transform.gameObject.GetComponent<CharacterManager>();

                // NULL CHECK
                if (potentialTargetCharacter == null)
                    continue;

                // IF IT'S PLAYER ITSELF -> RETURN
                if (potentialTargetCharacter.NetworkObjectId == character.NetworkObjectId)
                    continue;

                // IF CHARACTERS ARE ON THE SAME TEAM -> RETURN
                if (!UtilityManager.instance.CanCharacterAttackThisTargetTeam(character.characterTeam, potentialTargetCharacter.characterTeam))
                    continue;

                // IF DEAD -> RETURN
                if (potentialTargetCharacter.characterNetwork.networkIsDead.Value)
                    continue;

                // IF NON RIPOSTABLE -> RETURN
                if (!potentialTargetCharacter.characterNetwork.networkIsRipostable.Value)
                    continue;

                // IF ALREADY BEEING CRITICALLY DAMAGED -> RETURN
                if (potentialTargetCharacter.characterNetwork.networkIsBeingCriticallyDamaged.Value)
                    continue;

                // IF INVINCIBLE -> RETURN
                if (potentialTargetCharacter.characterNetwork.networkIsInvincible.Value)
                    continue;

                // IF THE ANGLE IS NOT RIGHT -> RETURN
                Vector3 distanceToCharacter = character.transform.position - potentialTargetCharacter.transform.position;
                float angleToTarget = Vector3.SignedAngle(potentialTargetCharacter.transform.forward, distanceToCharacter, Vector3.up);
                if (angleToTarget > 60 || angleToTarget < -60)
                    return;

                // IF ALL THE CHECKS WERE PASSED -> IT'S OUR TARGET TO RIPOSTE
                target = potentialTargetCharacter;
                break;
            }

            if (target == null)
                return;
            
            // 3. DO RIPOSTE
            PerformRiposte(target, weaponClass);
        }

        protected virtual void PerformRiposte(CharacterManager target, WeaponClass weaponClass)
        {
            
        }

        // ANIMATION EVENT
        public void ApplyPendingCriticalDamage()
        {
            // SFX
            character.characterSFXManager.PlayCriticalStrikeSoundFX();

            // VFX
            character.characterEffectsManager.PlayCriticalBloodSplashVFX(character.characterCombatManager.lockOnTransform.position);

            // DAMAGE HEALTH
            character.characterStatsManager.DamageHelth(pendingCriticalDamage);
        }

        // --------
        // BACKSTAB
        public void AttemptBackstab(WeaponClass weaponClass)
        {
            // 1. RAYCASTING TO FIND THE TARGET
            RaycastHit[] potentialTargets = Physics.RaycastAll(
                lockOnTransform.position,
                character.transform.TransformDirection(Vector3.forward),
                backstabDistance,
                UtilityManager.instance.GetCharacterMasks());

            // 2. CHECK IF TARGET IS SUITABLE
            CharacterManager target = null;
            foreach (RaycastHit potentialTarget in potentialTargets)
            {
                CharacterManager potentialTargetCharacter = potentialTarget.transform.gameObject.GetComponent<CharacterManager>();

                // NULL CHECK
                if (potentialTargetCharacter == null)
                    continue;

                // IF IT'S PLAYER ITSELF -> RETURN
                if (potentialTargetCharacter.NetworkObjectId == character.NetworkObjectId)
                    continue;

                // IF CHARACTERS ARE ON THE SAME TEAM -> RETURN
                if (!UtilityManager.instance.CanCharacterAttackThisTargetTeam(character.characterTeam, potentialTargetCharacter.characterTeam))
                    continue;

                // IF DEAD -> RETURN
                if (potentialTargetCharacter.characterNetwork.networkIsDead.Value)
                    continue;

                // IF NON BACKSTABABLE -> RETURN
                if (!isBackstabable)
                    continue;

                // IF ALREADY BEEING CRITICALLY DAMAGED -> RETURN
                if (potentialTargetCharacter.characterNetwork.networkIsBeingCriticallyDamaged.Value)
                    continue;

                // IF INVINCIBLE -> RETURN
                if (potentialTargetCharacter.characterNetwork.networkIsInvincible.Value)
                    continue;

                // IF THE ANGLE IS NOT RIGHT -> RETURN
                Vector3 distanceToCharacter = character.transform.position - potentialTargetCharacter.transform.position;
                float angleToTarget = Vector3.SignedAngle(potentialTargetCharacter.transform.forward, distanceToCharacter, Vector3.up);
                Debug.Log(angleToTarget);
                if (angleToTarget >= 0 && angleToTarget < 145 ||     // RIGHT SIDE
                    angleToTarget <= 0 && angleToTarget > -145)      // LEFT SIDE
                    return;

                // IF ALL THE CHECKS WERE PASSED -> IT'S OUR TARGET TO RIPOSTE
                target = potentialTargetCharacter;
                break;
            }

            if (target == null)
                return;

            // 3. DO RIPOSTE
            PerformBackstab(target, weaponClass);
        }

        protected virtual void PerformBackstab(CharacterManager target, WeaponClass weaponClass)
        {

        }

        // ---------------------------
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

        // ANIMATION EVENTS - IS RIPOSTABLE
        public void EnableIsRipostable()
        {
            if (!character.IsOwner)
                return;

            character.characterNetwork.networkIsRipostable.Value = true;
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
