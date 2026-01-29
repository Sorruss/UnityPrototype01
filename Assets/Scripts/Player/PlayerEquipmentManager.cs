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
        [SerializeField] private WeaponSocket rightHandSocket;

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
                {
                    leftHandSocket = socket;
                }
                else if (socket.socket == CharacterWeaponSocket.RIGHT_HAND)
                {
                    rightHandSocket = socket;
                }
            }
        }

        // ----------------
        // LEFT HAND WEAPON.
        public void SwitchLeftWeapon()
        {
            if (!playerManager.IsOwner)
            {
                return;
            }

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
                playerManager.playerInventoryManager.LeftHandWeaponSciptable = anotherWeapon;

                // NOTIFY NETWORK SO IT CHANGES WIELDING WEAPON FOR EVERYONE INCLUDING THIS CLIENT.
                playerManager.playerNetwork.networkLeftHandWeaponID.Value = anotherWeapon.ID;
            }
            else
            {
                playerManager.playerInventoryManager.LeftHandWeaponIndex = 0;
                playerManager.playerInventoryManager.LeftHandWeaponSciptable = ItemDatabase.instance.unarmedWeapon;

                // NOTIFY NETWORK SO IT CHANGES WIELDING WEAPON FOR EVERYONE INCLUDING THIS CLIENT.
                playerManager.playerNetwork.networkLeftHandWeaponID.Value = ItemDatabase.instance.unarmedWeapon.ID;
            }
        }

        public void LoadLeftWeapon()
        {
            if (playerManager.playerInventoryManager.LeftHandWeaponSciptable != null)
            {
                // INSTANTIATE WEAPON PREFUB AND LOAD IT INTO HAND.
                LeftHandWeaponInstance = Instantiate(playerManager.playerInventoryManager.LeftHandWeaponSciptable.ModelPrefub);
                leftHandSocket.LoadModel(LeftHandWeaponInstance);

                // SET WEAPON DAMAGE VALUES TO THE DAMAGE COLLIDER WHICH IS ON ITS PREFUB.
                LeftHandWeaponManager = LeftHandWeaponInstance.GetComponent<WeaponManager>();
                LeftHandWeaponManager.SetWeaponDamage(playerManager, ref playerManager.playerInventoryManager.LeftHandWeaponSciptable);
            }
        }

        public void UnloadLeftWeapon()
        {
            leftHandSocket.UnloadModel();
        }

        // ----------------
        // RIGT HAND WEAPON.
        public void SwitchRightWeapon()
        {
            if (!playerManager.IsOwner)
            {
                return;
            }

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
                playerManager.playerInventoryManager.RightHandWeaponSciptable = anotherWeapon;

                // NOTIFY NETWORK SO IT CHANGES WIELDING WEAPON FOR EVERYONE INCLUDING THIS CLIENT.
                playerManager.playerNetwork.networkRightHandWeaponID.Value = anotherWeapon.ID;
            }
            else
            {
                playerManager.playerInventoryManager.RightHandWeaponIndex = 0;
                playerManager.playerInventoryManager.RightHandWeaponSciptable = ItemDatabase.instance.unarmedWeapon;

                // NOTIFY NETWORK SO IT CHANGES WIELDING WEAPON FOR EVERYONE INCLUDING THIS CLIENT.
                playerManager.playerNetwork.networkRightHandWeaponID.Value = ItemDatabase.instance.unarmedWeapon.ID;
            }
        }

        public void LoadRightWeapon()
        {
            if (playerManager.playerInventoryManager.RightHandWeaponSciptable != null)
            {
                RightHandWeaponInstance = Instantiate(playerManager.playerInventoryManager.RightHandWeaponSciptable.ModelPrefub);
                rightHandSocket.LoadModel(RightHandWeaponInstance);

                RightHandWeaponManager = RightHandWeaponInstance.GetComponent<WeaponManager>();
                RightHandWeaponManager.SetWeaponDamage(playerManager, ref playerManager.playerInventoryManager.RightHandWeaponSciptable);
            }
        }

        public void UnloadRightWeapon()
        {
            rightHandSocket.UnloadModel();
        }

        // --------------
        // COMMON METHODS.
        private void LoadBothHandsWeapons()
        {
            LoadLeftWeapon();
            LoadRightWeapon();
        }

        private void UnloadBothHandsWeapons()
        {
            UnloadLeftWeapon();
            UnloadRightWeapon();
        }

        // ----------------------------------------------
        // ATTACK ANIMATION EVENTS - COLLIDERS MANAGEMENT
        public void ActivateDamageCollider()
        {
            if (playerManager.playerNetwork.networkIsUsingLeftHand.Value)
            {
                LeftHandWeaponManager.ActivateDamageCollider(true);
                playerManager.playerSFXManager.PlayAudioClip(
                    SFXManager.instance.GetRandomSFX(ref playerManager.playerInventoryManager.LeftHandWeaponSciptable.whooshes));
            }

            if (playerManager.playerNetwork.networkIsUsingRightHand.Value)
            {
                RightHandWeaponManager.ActivateDamageCollider(true);
                playerManager.playerSFXManager.PlayAudioClip(
                    SFXManager.instance.GetRandomSFX(ref playerManager.playerInventoryManager.RightHandWeaponSciptable.whooshes));
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
