using UnityEngine;

namespace FG
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        private PlayerManager playerManager;

        [Header("Weapon Prefubs Instances (Auto)")]
        [SerializeField] private GameObject LeftHandWeaponInstance;
        [SerializeField] private GameObject RightHandWeaponInstance;

        [Header("Weapon Prefubs Weapon Managers (Auto)")]
        [SerializeField] public WeaponManager LeftHandWeaponManager;
        [SerializeField] public WeaponManager RightHandWeaponManager;

        [Header("Sockets (Auto)")]
        [SerializeField] private WeaponSocket leftHandSocket;
        [SerializeField] private WeaponSocket leftHandShieldSocket;
        [SerializeField] private WeaponSocket rightHandSocket;
        [SerializeField] private WeaponSocket backSocket;

        protected override void Awake()
        {
            base.Awake();

            playerManager = GetComponent<PlayerManager>();
            FindSockets();
        }

        protected override void Start()
        {
            base.Start();

            LoadBothHandsWeapons();
        }

        private void FindSockets()
        {
            WeaponSocket[] sockets = GetComponentsInChildren<WeaponSocket>();
            foreach (WeaponSocket socket in sockets)
            {
                if (socket.socket == CharacterWeaponSocket.LEFT_HAND)
                    leftHandSocket = socket;
                else if (socket.socket == CharacterWeaponSocket.RIGHT_HAND)
                    rightHandSocket = socket;
                else if (socket.socket == CharacterWeaponSocket.LEFT_HAND_SHIELD)
                    leftHandShieldSocket = socket;
                else if (socket.socket == CharacterWeaponSocket.BACK)
                    backSocket = socket;
            }
        }

        // ----------------
        // LEFT HAND WEAPON.
        public void SwitchLeftWeapon()
        {
            if (!playerManager.IsOwner)
                return;

            // DO ANIMATION WITHOUT MOVEMENT RESTRICTIONS.
            playerManager.playerAnimatorManager.PerformAnimationAction("swap_left_weapon_01", false, false, true, true);

            // LOOK IN FRONT OF CURRENT WEAPON SLOT IF IT'S ALREADY NOT A LAST SLOT.
            int anotherWeaponIndex = 0;
            WeaponItem anotherWeapon = null;
            if (playerManager.playerInventoryManager.LeftHandWeaponIndex != 2)
            {
                for (int i = playerManager.playerInventoryManager.LeftHandWeaponIndex + 1; i < 3; ++i)
                {
                    if (playerManager.playerInventoryManager.LeftHandWeaponSciptables[i].ID != ItemDatabase.instance.unarmedWeapon.ID)
                    {
                        anotherWeapon = playerManager.playerInventoryManager.LeftHandWeaponSciptables[i];
                        anotherWeaponIndex = i;
                        break;
                    }
                }
            }

            // IF NOTHING WAS FOUND, LOOK FROM THE START OF WEAPON SLOTS.
            if (anotherWeapon == null)
            {
                for (int i = 0; i < playerManager.playerInventoryManager.LeftHandWeaponIndex; ++i)
                {
                    if (playerManager.playerInventoryManager.LeftHandWeaponSciptables[i].ID != ItemDatabase.instance.unarmedWeapon.ID)
                    {
                        anotherWeapon = playerManager.playerInventoryManager.LeftHandWeaponSciptables[i];
                        anotherWeaponIndex = i;
                        break;
                    }
                }
            }

            // ASSIGN FOUND WEAPON OR UNARMED IF NOTHING WAS FOUND.
            if (anotherWeapon != null)
            {
                playerManager.playerInventoryManager.LeftHandWeaponIndex = anotherWeaponIndex;
                playerManager.playerInventoryManager.LeftHandWeaponScriptable = anotherWeapon;

                // NOTIFY NETWORK SO IT CHANGES WIELDING WEAPON FOR EVERYONE INCLUDING THIS CLIENT.
                playerManager.playerNetwork.networkLeftHandWeaponID.Value = anotherWeapon.ID;
            }
            else
            {
                playerManager.playerInventoryManager.LeftHandWeaponIndex = 0;
                playerManager.playerInventoryManager.LeftHandWeaponScriptable = ItemDatabase.instance.unarmedWeapon;

                // NOTIFY NETWORK SO IT CHANGES WIELDING WEAPON FOR EVERYONE INCLUDING THIS CLIENT.
                playerManager.playerNetwork.networkLeftHandWeaponID.Value = ItemDatabase.instance.unarmedWeapon.ID;
            }

            // OVERRIDE ANIMATIONS FOR NEWLY EQUIPPED WEAPON
            playerManager.playerAnimatorManager.UpdateAnimatorOverrider(
                playerManager.playerInventoryManager.LeftHandWeaponScriptable.animatorOverrider);
        }

        public void LoadLeftWeapon()
        {
            if (playerManager.playerInventoryManager.LeftHandWeaponScriptable != null)
            {
                // INSTANTIATE WEAPON PREFUB AND LOAD IT INTO HAND
                LeftHandWeaponInstance = Instantiate(playerManager.playerInventoryManager.LeftHandWeaponScriptable.ModelPrefub);

                // CHOOSE WHICH SOCKET TO LOAD TO DEPENDING ON THE WEAPON TYPE WE LOADING
                WeaponType leftWeaponType = playerManager.playerInventoryManager.LeftHandWeaponScriptable.weaponType;
                if (leftWeaponType == WeaponType.WEAPON)
                    leftHandSocket.LoadModel(LeftHandWeaponInstance);
                else if (leftWeaponType == WeaponType.SHIELD)
                    leftHandShieldSocket.LoadModel(LeftHandWeaponInstance);

                // SET WEAPON DAMAGE VALUES TO THE DAMAGE COLLIDER WHICH IS ON ITS PREFUB
                LeftHandWeaponManager = LeftHandWeaponInstance.GetComponent<WeaponManager>();
                LeftHandWeaponManager.TransferWeaponValuesToCollider(playerManager, ref playerManager.playerInventoryManager.LeftHandWeaponScriptable);
            }
        }

        public void UnloadLeftWeapon()
        {
            leftHandSocket.UnloadModel();
            leftHandShieldSocket.UnloadModel();
        }

        // ----------------
        // RIGT HAND WEAPON.
        public void SwitchRightWeapon()
        {
            if (!playerManager.IsOwner)
                return;

            // DO ANIMATION WITHOUT MOVEMENT RESTRICTIONS.
            playerManager.playerAnimatorManager.PerformAnimationAction("swap_right_weapon_01", false, false, true, true);

            // LOOK IN FRONT OF CURRENT WEAPON SLOT IF IT'S ALREADY NOT A LAST SLOT.
            int anotherWeaponIndex = 0;
            WeaponItem anotherWeapon = null;
            if (playerManager.playerInventoryManager.RightHandWeaponIndex != 2)
            {
                for (int i = playerManager.playerInventoryManager.RightHandWeaponIndex + 1; i < 3; ++i)
                {
                    if (playerManager.playerInventoryManager.RightHandWeaponSciptables[i].ID != ItemDatabase.instance.unarmedWeapon.ID)
                    {
                        anotherWeapon = playerManager.playerInventoryManager.RightHandWeaponSciptables[i];
                        anotherWeaponIndex = i;
                        break;
                    }
                }
            }

            // IF NOTHING WAS FOUND, LOOK FROM THE START OF WEAPON SLOTS.
            if (anotherWeapon == null)
            {
                for (int i = 0; i < playerManager.playerInventoryManager.RightHandWeaponIndex; ++i)
                {
                    if (playerManager.playerInventoryManager.RightHandWeaponSciptables[i].ID != ItemDatabase.instance.unarmedWeapon.ID)
                    {
                        anotherWeapon = playerManager.playerInventoryManager.RightHandWeaponSciptables[i];
                        anotherWeaponIndex = i;
                        break;
                    }
                }
            }

            // ASSIGN FOUND WEAPON OR UNARMED IF NOTHING WAS FOUND.
            if (anotherWeapon != null)
            {
                playerManager.playerInventoryManager.RightHandWeaponIndex = anotherWeaponIndex;
                playerManager.playerInventoryManager.RightHandWeaponScriptable = anotherWeapon;

                // NOTIFY NETWORK SO IT CHANGES WIELDING WEAPON FOR EVERYONE INCLUDING THIS CLIENT.
                playerManager.playerNetwork.networkRightHandWeaponID.Value = anotherWeapon.ID;
            }
            else
            {
                playerManager.playerInventoryManager.RightHandWeaponIndex = 0;
                playerManager.playerInventoryManager.RightHandWeaponScriptable = ItemDatabase.instance.unarmedWeapon;

                // NOTIFY NETWORK SO IT CHANGES WIELDING WEAPON FOR EVERYONE INCLUDING THIS CLIENT.
                playerManager.playerNetwork.networkRightHandWeaponID.Value = ItemDatabase.instance.unarmedWeapon.ID;
            }

            // OVERRIDE ANIMATIONS FOR NEWLY EQUIPPED WEAPON
            playerManager.playerAnimatorManager.UpdateAnimatorOverrider(
                playerManager.playerInventoryManager.RightHandWeaponScriptable.animatorOverrider);
        }

        public void LoadRightWeapon()
        {
            if (playerManager.playerInventoryManager.RightHandWeaponScriptable != null)
            {
                RightHandWeaponInstance = Instantiate(playerManager.playerInventoryManager.RightHandWeaponScriptable.ModelPrefub);
                rightHandSocket.LoadModel(RightHandWeaponInstance);

                RightHandWeaponManager = RightHandWeaponInstance.GetComponent<WeaponManager>();
                RightHandWeaponManager.TransferWeaponValuesToCollider(playerManager, ref playerManager.playerInventoryManager.RightHandWeaponScriptable);
            }
        }

        public void UnloadRightWeapon()
        {
            rightHandSocket.UnloadModel();
        }

        // -----------
        // TWO HANDING
        public void UnTwoHandWeapon()
        {
            // 1. REMOVE STRENGTH BUFF FROM COLLIDER
            // 2. RETURN WEAPON IN ITS SOCKET
            // 3. RETURN WEAPON FROM BACK IN ITS SOCKET
            // 4. APPLY 1H ANIMSET

            playerManager.animator.SetBool("IsTwoHanding", false);
        }

        public void TwoHandLeftWeapon()
        {
            // 1. APPLY STRENGTH BOOST TO COLLIDER
            // 2. SEND RIGHT WEAPON TO BACK SOCKET
            // 3. FIT LEFT WEAPON IN RIGHT HAND SOCKET
            // 4. APPLY 2H ANIMSET

            playerManager.animator.SetBool("IsTwoHanding", true);
        }

        public void TwoHandRightWeapon()
        {
            // 1. APPLY STRENGTH BOOST TO COLLIDER
            // 2. SEND LEFT WEAPON TO BACK SOCKET
            // 3. FIT LEFT WEAPON IN RIGHT HAND SOCKET
            // 4. APPLY 2H ANIMSET

            playerManager.animator.SetBool("IsTwoHanding", true);
        }

        // --------------
        // COMMON METHODS.
        private void LoadBothHandsWeapons()
        {
            LoadLeftWeapon();
            LoadRightWeapon();
        }

        // ----------------------------------------------
        // ATTACK ANIMATION EVENTS - COLLIDERS MANAGEMENT
        public void ActivateDamageCollider()
        {
            if (playerManager.playerNetwork.networkIsUsingLeftHand.Value)
            {
                LeftHandWeaponManager.ActivateDamageCollider(true);
                playerManager.playerSFXManager.PlayAudioClip(
                    SFXManager.instance.GetRandomSFX(ref playerManager.playerInventoryManager.LeftHandWeaponScriptable.whooshesSoundFX));
            }

            if (playerManager.playerNetwork.networkIsUsingRightHand.Value)
            {
                RightHandWeaponManager.ActivateDamageCollider(true);
                playerManager.playerSFXManager.PlayAudioClip(
                    SFXManager.instance.GetRandomSFX(ref playerManager.playerInventoryManager.RightHandWeaponScriptable.whooshesSoundFX));
            }
        }

        public void DeactivateDamageCollider()
        {
            if (playerManager.playerNetwork.networkIsUsingLeftHand.Value)
            {
                LeftHandWeaponManager.ActivateDamageCollider(false);
            }

            if (playerManager.playerNetwork.networkIsUsingRightHand.Value)
            {
                RightHandWeaponManager.ActivateDamageCollider(false);
            }
        }
    }
}
