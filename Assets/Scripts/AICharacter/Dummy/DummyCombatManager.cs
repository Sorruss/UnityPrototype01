using UnityEngine;

namespace FG
{
    public class DummyCombatManager : AICharacterCombatManager
    {
        private AICharacterManager aiCharacter;

        // WEAPON SCRIPTABLES
        [Header("Weapon's Scriptables")]
        [SerializeField] private WeaponItem LeftHandWeaponScriptable;
        [SerializeField] private WeaponItem RightHandWeaponScriptable;

        [Header("Attack Damage")]
        public int baseDamage = 20;
        public int basePoiseDamage = 40;

        [Header("Attack Modifiers")]
        public float Attack01DamageModifier = 1.0f;
        public float Attack02DamageModifier = 1.2f;

        // WEAPON PREFUBS INSTANCES
        private GameObject LeftHandWeaponInstance;
        private GameObject RightHandWeaponInstance;

        // WEAPON PREFUBS WEAPON MANAGERS
        private WeaponManager LeftHandWeaponManager;
        private WeaponManager RightHandWeaponManager;

        // SOCKETS
        private WeaponSocket leftHandSocket;
        private WeaponSocket rightHandSocket;

        protected override void Awake()
        {
            base.Awake();

            // INSTANTIATE SCRIPTABLES
            if (LeftHandWeaponInstance != null)
            {
                LeftHandWeaponScriptable = Instantiate(LeftHandWeaponScriptable);
                LeftHandWeaponScriptable.physicalDamage = baseDamage;
            }

            if (RightHandWeaponScriptable != null)
            {
                RightHandWeaponScriptable = Instantiate(RightHandWeaponScriptable);
                RightHandWeaponScriptable.physicalDamage = baseDamage;
            }

            // GET CHARACTER AND SOCKETS
            aiCharacter = GetComponent<AICharacterManager>();
            FindSockets();
        }

        protected override void Start()
        {
            base.Start();

            LoadBothHandsWeapons();
        }

        public override void DisableAllDamageColliders()
        {
            base.DisableAllDamageColliders();

            if (LeftHandWeaponManager != null)
                LeftHandWeaponManager.ActivateDamageCollider(false);

            if (RightHandWeaponManager != null)
                RightHandWeaponManager.ActivateDamageCollider(false);
        }

        // ----------------------------------------------------
        // EQUIPMENT FUNCTIONALITY (FIND SOCKETS, LOAD WEAPONS)
        private void FindSockets()
        {
            WeaponSocket[] sockets = GetComponentsInChildren<WeaponSocket>();
            foreach (WeaponSocket socket in sockets)
            {
                if (socket.socket == CharacterWeaponSocket.LEFT_HAND)
                {
                    leftHandSocket = socket;
                }
                else if (socket.socket == CharacterWeaponSocket.RIGHT_HAND)
                {
                    rightHandSocket = socket;
                }
            }
        }

        public void LoadLeftWeapon()
        {
            if (leftHandSocket == null)
                return;

            if (LeftHandWeaponScriptable == null)
                return;

            // INSTANTIATE WEAPON PREFUB AND LOAD IT INTO HAND.
            LeftHandWeaponInstance = Instantiate(LeftHandWeaponScriptable.ModelPrefub);
            leftHandSocket.LoadModel(LeftHandWeaponInstance);

            // SET WEAPON DAMAGE VALUES TO THE DAMAGE COLLIDER WHICH IS ON ITS PREFUB.
            LeftHandWeaponManager = LeftHandWeaponInstance.GetComponent<WeaponManager>();
            LeftHandWeaponManager.TransferWeaponValuesToCollider(aiCharacter, ref LeftHandWeaponScriptable);
        }

        public void LoadRightWeapon()
        {
            if (rightHandSocket == null)
                return;

            if (RightHandWeaponScriptable == null)
                return;

            RightHandWeaponInstance = Instantiate(RightHandWeaponScriptable.ModelPrefub);
            rightHandSocket.LoadModel(RightHandWeaponInstance);

            RightHandWeaponManager = RightHandWeaponInstance.GetComponent<WeaponManager>();
            RightHandWeaponManager.TransferWeaponValuesToCollider(aiCharacter, ref RightHandWeaponScriptable);
        }

        private void LoadBothHandsWeapons()
        {
            LoadLeftWeapon();
            LoadRightWeapon();
        }

        // ----------------------------------------------
        // ATTACK ANIMATION EVENTS - COLLIDERS MANAGEMENT
        public void ActivateLeftDamageCollider()
        {
            LeftHandWeaponManager.ActivateDamageCollider(true);
            aiCharacter.characterSFXManager.PlayAudioClip(
                    SFXManager.instance.GetRandomSFX(ref LeftHandWeaponScriptable.whooshesSoundFX));
        }

        public void DeactivateLeftDamageCollider()
        {
            LeftHandWeaponManager.ActivateDamageCollider(false);
        }

        public void ActivateRightDamageCollider()
        {
            RightHandWeaponManager.ActivateDamageCollider(true);
            aiCharacter.characterSFXManager.PlayAudioClip(
                    SFXManager.instance.GetRandomSFX(ref RightHandWeaponScriptable.whooshesSoundFX));
        }

        public void DeactivateRightDamageCollider()
        {
            RightHandWeaponManager.ActivateDamageCollider(false);
        }

        // ATTACK ANIMATION EVENTS - APPLY DAMAGE MODIFIERS
        public void ApplyAttack01DamageModifier()
        {
            RightHandWeaponScriptable.physicalDamage = (int)(baseDamage * Attack01DamageModifier);
            RightHandWeaponScriptable.poiseDamage = (int)(basePoiseDamage * Attack01DamageModifier);
            RightHandWeaponManager.TransferWeaponValuesToCollider(aiCharacter, ref RightHandWeaponScriptable);
        }

        public void ApplyAttack02DamageModifier()
        {
            RightHandWeaponScriptable.physicalDamage = (int)(baseDamage * Attack02DamageModifier);
            RightHandWeaponScriptable.poiseDamage = (int)(basePoiseDamage * Attack02DamageModifier);
            RightHandWeaponManager.TransferWeaponValuesToCollider(aiCharacter, ref RightHandWeaponScriptable);
        }
    }
}
