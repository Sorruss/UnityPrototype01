using Unity.Netcode;
using UnityEngine;

namespace FG
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        private PlayerManager player;

        [Header("Debug Only")]
        public bool CanDoComboMainHand = false;
        public bool CanDoComboOffHand = false;
        public bool CanBlock = true;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public override void DisableAllDamageColliders()
        {
            base.DisableAllDamageColliders();

            if (player.playerEquipmentManager.LeftHandWeaponManager != null)
                player.playerEquipmentManager.LeftHandWeaponManager.ActivateDamageCollider(false);

            if (player.playerEquipmentManager.RightHandWeaponManager != null)
                player.playerEquipmentManager.RightHandWeaponManager.ActivateDamageCollider(false);
        }

        // -------------
        // LOCK ON STUFF
        public override void SetCurrentTarget(CharacterManager target)
        {
            base.SetCurrentTarget(target);

            if (player.IsOwner)
                PlayerCamera.instance.AdjustCameraHeight();
        }

        // --------------
        // WEAPON ACTIONS
        public void TryToPerformWeaponAction(WeaponAction weaponAction, WeaponItem weapon)
        {
            // SET CORRECT WEAPON ANIMATOR OVERRIDER CONTROLLER
            if (player.playerNetwork.networkIsTwoHanding.Value)
                player.playerAnimatorManager.UpdateAnimatorOverrider(weapon.animatorOverriderTH);
            else
                player.playerAnimatorManager.UpdateAnimatorOverrider(weapon.animatorOverriderOH);

            // PERFORM ACTION ON THIS CLIENT.
            weaponAction.TryToPerformAction(player, weapon);

            if (player.IsOwner)
            { // NOTIFY ALL OTHER CLIENTS TO MIMIC THE SAME ACTION ON THIS CLIENT'S INSTANCE.
                player.playerNetwork.NotifyServerOfWeaponActionServerRpc(
                    NetworkManager.Singleton.LocalClientId, 
                    weaponAction.ActionID, 
                    weapon.ID);
            }
        }

        // ------------------
        // RIPOSTE & BACKSTAB
        protected override void PerformRiposte(CharacterManager target, WeaponClass weaponClass)
        {
            // 1. CHECK IF PLAYER CAN DO RIPOSTE
            if (player.isPerfomingAction)
                return;

            if (player.characterNetwork.networkIsDead.Value)
                return;

            if (player.characterNetwork.networkCurrentStamina.Value <= 0.0f)
                return;

            WeaponMeleeItem weaponUsed = currentWeaponItemBeingUsed as WeaponMeleeItem;
            if (weaponUsed == null)
                return;

            // 2. CREATE GAMEOBJECT WITH CORRECT POSITION RELATIVE TO WEAPON WE USING
            if (riposteTargetPosition == null)
            {
                GameObject gameObject = new GameObject("Riposte Victim Position");
                gameObject.transform.parent = transform;
                gameObject.transform.position = Vector3.zero;
                riposteTargetPosition = gameObject.transform;
            }

            riposteTargetPosition.localPosition = UtilityManager.instance.GetRipostePositionBasedOnWeaponClass(weaponClass);

            // 3. PLACE TARGET ONTO THE CREATED GAMEOBJECT
            target.transform.position = riposteTargetPosition.position;

            // 4. ROTATE TARGET SO IT LOOKS AT THE CHARACTER
            target.transform.rotation = Quaternion.LookRotation(-player.transform.forward);

            // 5. CREATE CRITICAL DAMAGE EFFECT
            TakeCriticalDamageEffect criticalDamageEffect = Instantiate(EffectsManager.instance.criticalDamageEffect);
            WeaponManager weaponManager = player.playerEquipmentManager.RightHandWeaponManager;

            if (player.playerNetwork.networkIsTwoHandingLeftWeapon.Value)
                weaponManager = player.playerEquipmentManager.LeftHandWeaponManager;

            criticalDamageEffect.physicalDamage = weaponManager.meleeDamageCollider.physicalDamage;
            criticalDamageEffect.magicDamage = weaponManager.meleeDamageCollider.magicDamage;
            criticalDamageEffect.fireDamage = weaponManager.meleeDamageCollider.fireDamage;
            criticalDamageEffect.lightningDamage = weaponManager.meleeDamageCollider.lightningDamage;
            criticalDamageEffect.holyDamage = weaponManager.meleeDamageCollider.holyDamage;

            criticalDamageEffect.physicalDamage *= weaponUsed.riposteMultiplier;
            criticalDamageEffect.magicDamage *= weaponUsed.riposteMultiplier;
            criticalDamageEffect.fireDamage *= weaponUsed.riposteMultiplier;
            criticalDamageEffect.lightningDamage *= weaponUsed.riposteMultiplier;
            criticalDamageEffect.holyDamage *= weaponUsed.riposteMultiplier;

            // 6. APPLY CRITICAL DAMAGE EFFECT TO THE TARGET (WILL ASSIGN PENDING_CRITICAL_DAMAGE VARIABLE)
            target.characterNetwork.NotifyClientOfRiposteServerRpc(
                player.NetworkObjectId, target.NetworkObjectId, weaponUsed.ID,
                criticalDamageEffect.physicalDamage, criticalDamageEffect.magicDamage,
                criticalDamageEffect.fireDamage, criticalDamageEffect.lightningDamage,
                criticalDamageEffect.holyDamage);

            // 7. PERFORM THE RIPOSTE ANIMATION ON CHARACTER (RPC)
            if (player.IsOwner)
                player.playerNetwork.networkIsInvincible.Value = true;

            player.playerAnimatorManager.PerformInstantAnimationAction("Riposte_01", true);
        }

        protected override void PerformBackstab(CharacterManager target, WeaponClass weaponClass)
        {
            // 1. CHECK IF PLAYER CAN DO RIPOSTE
            if (player.isPerfomingAction)
                return;

            if (player.characterNetwork.networkIsDead.Value)
                return;

            if (player.characterNetwork.networkCurrentStamina.Value <= 0.0f)
                return;

            WeaponMeleeItem weaponUsed = currentWeaponItemBeingUsed as WeaponMeleeItem;
            if (weaponUsed == null)
                return;

            // 2. CREATE GAMEOBJECT WITH CORRECT POSITION RELATIVE TO WEAPON WE USING
            if (backstabTargetPosition == null)
            {
                GameObject gameObject = new GameObject("Backstab Victim Position");
                gameObject.transform.parent = transform;
                gameObject.transform.position = Vector3.zero;
                backstabTargetPosition = gameObject.transform;
            }

            backstabTargetPosition.localPosition = UtilityManager.instance.GetBackstabPositionBasedOnWeaponClass(weaponClass);

            // 3. PLACE TARGET ONTO THE CREATED GAMEOBJECT
            target.transform.position = backstabTargetPosition.position;

            // 4. ROTATE TARGET SO IT LOOKS AWAY FROM THE CHARACTER
            target.transform.rotation = Quaternion.LookRotation(player.transform.forward);

            // 5. CREATE CRITICAL DAMAGE EFFECT
            TakeCriticalDamageEffect criticalDamageEffect = Instantiate(EffectsManager.instance.criticalDamageEffect);
            WeaponManager weaponManager = player.playerEquipmentManager.RightHandWeaponManager;

            if (player.playerNetwork.networkIsTwoHandingLeftWeapon.Value)
                weaponManager = player.playerEquipmentManager.LeftHandWeaponManager;

            criticalDamageEffect.physicalDamage = weaponManager.meleeDamageCollider.physicalDamage;
            criticalDamageEffect.magicDamage = weaponManager.meleeDamageCollider.magicDamage;
            criticalDamageEffect.fireDamage = weaponManager.meleeDamageCollider.fireDamage;
            criticalDamageEffect.lightningDamage = weaponManager.meleeDamageCollider.lightningDamage;
            criticalDamageEffect.holyDamage = weaponManager.meleeDamageCollider.holyDamage;

            criticalDamageEffect.physicalDamage *= weaponUsed.backstabMultiplier;
            criticalDamageEffect.magicDamage *= weaponUsed.backstabMultiplier;
            criticalDamageEffect.fireDamage *= weaponUsed.backstabMultiplier;
            criticalDamageEffect.lightningDamage *= weaponUsed.backstabMultiplier;
            criticalDamageEffect.holyDamage *= weaponUsed.backstabMultiplier;

            // 6. APPLY CRITICAL DAMAGE EFFECT TO THE TARGET (WILL ASSIGN PENDING_CRITICAL_DAMAGE VARIABLE)
            target.characterNetwork.NotifyClientOfBackstabServerRpc(
                player.NetworkObjectId, target.NetworkObjectId, weaponUsed.ID,
                criticalDamageEffect.physicalDamage, criticalDamageEffect.magicDamage,
                criticalDamageEffect.fireDamage, criticalDamageEffect.lightningDamage,
                criticalDamageEffect.holyDamage);

            // 7. PERFORM THE BACKSTAB ANIMATION ON CHARACTER (RPC)
            if (player.IsOwner)
                player.playerNetwork.networkIsInvincible.Value = true;

            player.playerAnimatorManager.PerformInstantAnimationAction("Backstab_01", true);
        }

        // ----------
        // WEAPON ART
        public WeaponItem GetWeaponToUseWeaponArt()
        {
            // IF WE ARE TWO HANDING A WEAPON AND IT DOES HAVE AN ART -> IT'S GREAT
            // IF IT DOESN'T -> RETURN NULL
            if (player.playerNetwork.networkIsTwoHanding.Value)
            {
                // IDK WHAT TO DO YET HERE SO LEAVE IT BE FOR NOW
                return null;
            }

            // THE IDEA OF BELOW MADNESS -> LEFT WEAPON WEAPON ART TAKES PRIORITY IF IT'S PRESENT
            // OTHERWISE WE LOOK FOR RIGHT ONE
            if (player.playerInventoryManager.LeftHandWeaponScriptable == null)
            {
                // NO LEFT WEAPON
                if (player.playerInventoryManager.RightHandWeaponScriptable == null)
                {
                    // NO RIGHT WEAPON
                    return null; // NO WEAPON ART
                }
                else
                {
                    // YES RIGHT WEAPON
                    if (player.playerInventoryManager.RightHandWeaponScriptable.OH_LT_WeaponArt == null)
                    {
                        // NO WEAPON ART
                        return null;
                    }

                    // YES WEAPON ART
                    player.playerNetwork.SetCurrentActiveHand(true);
                    player.playerNetwork.networkCurrentWeaponInUseID.Value = player.playerInventoryManager.RightHandWeaponScriptable.ID;
                    return player.playerInventoryManager.RightHandWeaponScriptable;
                }
            }
            else
            {
                // YES LEFT WEAPON
                if (player.playerInventoryManager.LeftHandWeaponScriptable.OH_LT_WeaponArt == null)
                {
                    // NO WEAPON ART -> CHECK RIGHT HAND
                    if (player.playerInventoryManager.RightHandWeaponScriptable == null)
                    {
                        // NO RIGHT WEAPON
                        return null;
                    }

                    // YES RIGHT WEAPON
                    if (player.playerInventoryManager.RightHandWeaponScriptable.OH_LT_WeaponArt == null)
                    {
                        // NO WEAPON ART
                        return null;
                    }

                    // YES WEAPON ART
                    player.playerNetwork.SetCurrentActiveHand(true);
                    player.playerNetwork.networkCurrentWeaponInUseID.Value = player.playerInventoryManager.RightHandWeaponScriptable.ID;
                    return player.playerInventoryManager.RightHandWeaponScriptable;
                }

                // YES WEAPON ART
                player.playerNetwork.SetCurrentActiveHand(false);
                player.playerNetwork.networkCurrentWeaponInUseID.Value = player.playerInventoryManager.LeftHandWeaponScriptable.ID;
                return player.playerInventoryManager.LeftHandWeaponScriptable;
            }
        }

        // ---------------------------------
        // ACTION ANIMATIONS - STAMINA DRAIN
        public void TryToDrainStaminaBasedOnAttackType()
        {
            // SINCE THIS METHOD IS RUN AS AN ANIMATION EVENT, IT'S GONNA BE RUN BY EVERYONE
            // BUT IT'S NEEDED TO DRAIN STAMINA ONLY ON ONE PLAYER INSTANCE SO THIS CHECK IS HERE
            if (!player.IsOwner)
                return;

            // AT THIS POINT THE VARIABLE 'currentWeaponItemBeingUsed' WAS SET FROM INSIDE WEAPONACTION
            // BUT IF NOT THEN THERE IS THIS CHECK BELOW
            if (currentWeaponItemBeingUsed == null)
                return;

            UpdateStaminaNeededForCurrentMove();
            player.playerStatsManager.TryDecreaseStamina(staminaNeededForCurrentAction);
        }

        // ACTION ANIMATIONS - COMBO EVENTS
        public override void EnableCanDoCombo()
        {
            if (player.playerNetwork.networkIsUsingLeftHand.Value)
            {
                player.playerCombatManager.CanDoComboOffHand = true;
            }
            else if (player.playerNetwork.networkIsUsingRightHand.Value)
            {
                player.playerCombatManager.CanDoComboMainHand = true;
            }
        }

        public override void DisableCanDoCombo()
        {
            player.playerCombatManager.CanDoComboMainHand = false;
            player.playerCombatManager.CanDoComboOffHand = false;
        }
    }
}
