using UnityEngine;
using System.Collections.Generic;

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

        // ARMOR MALE HEAD
        [Header("Armor Male Full Helmet")]
        [SerializeField] private GameObject MaleArmorFullHelmetParent;
        public GameObject[] maleArmorFullHelmets;

        // ARMOR MALE CHEST
        [Header("Armor Male Torso")]
        [SerializeField] private GameObject MaleArmorTorsoParent;
        public GameObject[] maleArmorTorsos;

        [Header("Armor Male Left Upper Arm")]
        [SerializeField] private GameObject MaleArmorLeftUpperArmParent;
        public GameObject[] maleArmorLeftUpperArms;

        [Header("Armor Male Left Lower Arm")]
        [SerializeField] private GameObject MaleArmorLeftLowerArmParent;
        public GameObject[] maleArmorLeftLowerArms;

        [Header("Armor Male Right Upper Arm")]
        [SerializeField] private GameObject MaleArmorRightUpperArmParent;
        public GameObject[] maleArmorRightUpperArms;

        [Header("Armor Male Right Lower Arm")]
        [SerializeField] private GameObject MaleArmorRightLowerArmParent;
        public GameObject[] maleArmorRightLowerArms;

        // ARMOR MALE HAND
        [Header("Armor Male Left Hand")]
        [SerializeField] private GameObject MaleArmorLeftHandParent;
        public GameObject[] maleArmorLeftHands;

        [Header("Armor Male Right Hand")]
        [SerializeField] private GameObject MaleArmorRightHandParent;
        public GameObject[] maleArmorRightHands;

        // ARMOR MALE LEG
        [Header("Armor Male Left Upper Leg")]
        [SerializeField] private GameObject MaleArmorLeftUpperLegParent;
        public GameObject[] maleArmorLeftUpperLegs;

        [Header("Armor Male Left Lower Leg")]
        [SerializeField] private GameObject MaleArmorLeftLowerLegParent;
        public GameObject[] maleArmorLeftLowerLegs;

        [Header("Armor Male Left Leg")]
        [SerializeField] private GameObject MaleArmorLeftLegParent;
        public GameObject[] maleArmorLeftLegs;

        [Header("Armor Male Right Upper Leg")]
        [SerializeField] private GameObject MaleArmorRightUpperLegParent;
        public GameObject[] maleArmorRightUpperLegs;

        [Header("Armor Male Right Lower Leg")]
        [SerializeField] private GameObject MaleArmorRightLowerLegParent;
        public GameObject[] maleArmorRightLowerLegs;

        [Header("Armor Male Right Leg")]
        [SerializeField] private GameObject MaleArmorRightLegParent;
        public GameObject[] maleArmorRightLegs;

        [Header("Debug Equip Armor")]
        public bool debugEquipArmor = false;

        protected override void Awake()
        {
            base.Awake();

            playerManager = GetComponent<PlayerManager>();
            
            // WEAPONS
            FindSockets();
            
            // MALE ARMOR - HEAD
            FindArmorMaleFullHelmets();

            // MALE ARMOR - CHEST
            FindArmorMaleTorsos();
            FindArmorMaleLeftUpperArms();
            FindArmorMaleLeftLowerArms();
            FindArmorMaleRightUpperArms();
            FindArmorMaleRightLowerArms();

            // MALE ARMOR - HAND
            FindArmorMaleLeftHands();
            FindArmorMaleRightHands();

            // MALE ARMOR - LEG
            FindArmorMaleLeftUpperLegs();
            FindArmorMaleLeftLowerLegs();
            FindArmorMaleLeftLegs();
            FindArmorMaleRightUpperLegs();
            FindArmorMaleRightLowerLegs();
            FindArmorMaleRightLegs();
        }

        protected override void Start()
        {
            base.Start();

            LoadBothHandsWeapons();
        }

        protected override void Update()
        {
            base.Update();

            if (debugEquipArmor)
            {
                debugEquipArmor = false;

                EquipHeadArmor(playerManager.playerInventoryManager.HeadArmorScriptable);
                EquipChestArmor(playerManager.playerInventoryManager.ChestArmorScriptable);
                EquipHandArmor(playerManager.playerInventoryManager.HandArmorScriptable);
                EquipLegArmor(playerManager.playerInventoryManager.LegArmorScriptable);
            }
        }

        // ------------------------
        // WEAPONS RELATED ON_AWAKE
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

        // ---------------------------
        // FIND MALE ARMOR HEAD
        private void FindArmorMaleFullHelmets()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorFullHelmetParent.transform)
                children.Add(model.gameObject);

            maleArmorFullHelmets = children.ToArray();
        }

        // FIND MALE ARMOR CHEST
        private void FindArmorMaleTorsos()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorTorsoParent.transform)
                children.Add(model.gameObject);

            maleArmorTorsos = children.ToArray();
        }

        private void FindArmorMaleLeftUpperArms()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorLeftUpperArmParent.transform)
                children.Add(model.gameObject);

            maleArmorLeftUpperArms = children.ToArray();
        }

        private void FindArmorMaleLeftLowerArms()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorLeftLowerArmParent.transform)
                children.Add(model.gameObject);

            maleArmorLeftLowerArms = children.ToArray();
        }

        private void FindArmorMaleRightUpperArms()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorRightUpperArmParent.transform)
                children.Add(model.gameObject);

            maleArmorRightUpperArms = children.ToArray();
        }

        private void FindArmorMaleRightLowerArms()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorRightLowerArmParent.transform)
                children.Add(model.gameObject);

            maleArmorRightLowerArms = children.ToArray();
        }

        // FIND MALE ARMOR HAND
        private void FindArmorMaleLeftHands()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorLeftHandParent.transform)
                children.Add(model.gameObject);

            maleArmorLeftHands = children.ToArray();
        }

        private void FindArmorMaleRightHands()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorRightHandParent.transform)
                children.Add(model.gameObject);

            maleArmorRightHands = children.ToArray();
        }

        // FIND MALE ARMOR LEG
        private void FindArmorMaleLeftUpperLegs()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorLeftUpperLegParent.transform)
                children.Add(model.gameObject);

            maleArmorLeftUpperLegs = children.ToArray();
        }

        private void FindArmorMaleLeftLowerLegs()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorLeftLowerLegParent.transform)
                children.Add(model.gameObject);

            maleArmorLeftLowerLegs = children.ToArray();
        }

        private void FindArmorMaleLeftLegs()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorLeftLegParent.transform)
                children.Add(model.gameObject);

            maleArmorLeftLegs = children.ToArray();
        }

        private void FindArmorMaleRightUpperLegs()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorRightUpperLegParent.transform)
                children.Add(model.gameObject);

            maleArmorRightUpperLegs = children.ToArray();
        }

        private void FindArmorMaleRightLowerLegs()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorRightLowerLegParent.transform)
                children.Add(model.gameObject);

            maleArmorRightLowerLegs = children.ToArray();
        }

        private void FindArmorMaleRightLegs()
        {
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorRightLegParent.transform)
                children.Add(model.gameObject);

            maleArmorRightLegs = children.ToArray();
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
                playerManager.playerInventoryManager.LeftHandWeaponScriptable.animatorOverriderOH);
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
                    leftHandSocket.PlaceModelAsEquipped(LeftHandWeaponInstance);
                else if (leftWeaponType == WeaponType.SHIELD)
                    leftHandShieldSocket.PlaceModelAsEquipped(LeftHandWeaponInstance);

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
                    if (playerManager.playerInventoryManager.RightHandWeaponScriptables[i].ID != ItemDatabase.instance.unarmedWeapon.ID)
                    {
                        anotherWeapon = playerManager.playerInventoryManager.RightHandWeaponScriptables[i];
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
                    if (playerManager.playerInventoryManager.RightHandWeaponScriptables[i].ID != ItemDatabase.instance.unarmedWeapon.ID)
                    {
                        anotherWeapon = playerManager.playerInventoryManager.RightHandWeaponScriptables[i];
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
                playerManager.playerInventoryManager.RightHandWeaponScriptable.animatorOverriderOH);
        }

        public void LoadRightWeapon()
        {
            if (playerManager.playerInventoryManager.RightHandWeaponScriptable != null)
            {
                RightHandWeaponInstance = Instantiate(playerManager.playerInventoryManager.RightHandWeaponScriptable.ModelPrefub);
                rightHandSocket.PlaceModelAsEquipped(RightHandWeaponInstance);

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
            // 1. RETURN TWO-HAND WEAPON IN ITS SOCKET
            // 2. RETURN NOT-TWO-HAND WEAPON FROM BACK IN ITS SOCKET
            if (playerManager.playerNetwork.networkIsTwoHandingLeftWeapon.Value)
            {
                // TRANSFER ORIGINALLY LEFT WEAPON FROM RIGHT BACK TO LEFT SOCKET
                WeaponType weaponType = playerManager.playerInventoryManager.LeftHandWeaponScriptable.weaponType;
                if (weaponType == WeaponType.WEAPON)
                    leftHandSocket.PlaceModelAsEquipped(LeftHandWeaponInstance);
                else if (weaponType == WeaponType.SHIELD)
                    leftHandShieldSocket.PlaceModelAsEquipped(LeftHandWeaponInstance);

                // TRANSFER ORIGINALLY RIGHT WEAPON FROM BACK TO RIGHT SOCKET
                if (RightHandWeaponInstance != null)
                    rightHandSocket.PlaceModelAsEquipped(RightHandWeaponInstance);
            }
            else
            {
                if (LeftHandWeaponInstance != null)
                {
                    // LOAD LEFT WEAPON IN LEFT SOCKET
                    WeaponType weaponType = playerManager.playerInventoryManager.LeftHandWeaponScriptable.weaponType;
                    if (weaponType == WeaponType.WEAPON)
                        leftHandSocket.PlaceModelAsEquipped(LeftHandWeaponInstance);
                    else if (weaponType == WeaponType.SHIELD)
                        leftHandShieldSocket.PlaceModelAsEquipped(LeftHandWeaponInstance);
                }
            }

            // 3. APPLY 1H ANIMSET
            playerManager.animator.SetBool("IsTwoHanding", false);
            playerManager.playerAnimatorManager.UpdateAnimatorOverrider(
                playerManager.playerInventoryManager.RightHandWeaponScriptable.animatorOverriderOH);

            // 4. RESET DAMAGE COLLIDER
            LeftHandWeaponManager.TransferWeaponValuesToCollider(playerManager, 
                ref playerManager.playerInventoryManager.LeftHandWeaponScriptable);
            RightHandWeaponManager.TransferWeaponValuesToCollider(playerManager,
                ref playerManager.playerInventoryManager.RightHandWeaponScriptable);
        }

        public void TwoHandLeftWeapon()
        {
            // WE CAN'T TWO HAND "UNARMED" WEAPON
            if (playerManager.playerInventoryManager.LeftHandWeaponScriptable.ID == ItemDatabase.instance.unarmedWeapon.ID)
            {
                if (playerManager.IsOwner)
                    playerManager.playerNetwork.networkIsTwoHanding.Value = false;

                return;
            }

            // 1. PLACE RIGHT WEAPON IN BACK SOCKET
            if (RightHandWeaponInstance != null)
            {
                backSocket.PlaceModelAsUnequipped(RightHandWeaponInstance, 
                    playerManager.playerInventoryManager.RightHandWeaponScriptable.weaponClass);
            }
            
            // 2. FIT LEFT WEAPON IN RIGHT HAND SOCKET
            rightHandSocket.PlaceModelAsEquipped(LeftHandWeaponInstance);

            // 3. APPLY 2H ANIMSET
            playerManager.animator.SetBool("IsTwoHanding", true);
            playerManager.playerAnimatorManager.UpdateAnimatorOverrider(
                playerManager.playerInventoryManager.TwoHandedWeaponScriptable.animatorOverriderTH);
        }

        public void TwoHandRightWeapon()
        {
            // WE CAN'T TWO HAND "UNARMED" WEAPON
            if (playerManager.playerInventoryManager.RightHandWeaponScriptable.ID == ItemDatabase.instance.unarmedWeapon.ID)
            {
                if (playerManager.IsOwner)
                    playerManager.playerNetwork.networkIsTwoHanding.Value = false;

                return;
            }

            // 1. PLACE LEFT WEAPON IN BACK SOCKET
            if (RightHandWeaponInstance != null)
            {
                backSocket.PlaceModelAsUnequipped(LeftHandWeaponInstance,
                    playerManager.playerInventoryManager.LeftHandWeaponScriptable.weaponClass);
            }
            
            // 2. APPLY 2H ANIMSET
            playerManager.animator.SetBool("IsTwoHanding", true);
            playerManager.playerAnimatorManager.UpdateAnimatorOverrider(
                playerManager.playerInventoryManager.TwoHandedWeaponScriptable.animatorOverriderTH);
        }

        // -------------
        // ARMOR - EQUIP
        public void EquipHeadArmor(HeadArmorItem armorItem)
        {
            // 1. UNEQUIP ALL RELATED ARMOR
            UnequipHeadArmor();
            
            // 2. IF PASSED ARMOR PIECE IS NULL -> SET IT TO NULL IN INVENTORY AND RETURN
            if (armorItem == null)
            {
                playerManager.playerInventoryManager.HeadArmorScriptable = null;
                if (playerManager.IsOwner)
                    playerManager.playerNetwork.networkArmorHelmetID.Value = -1;
                return;
            }

            // 3. SET PASSED ARMOR PIECE IN INVENTORY
            playerManager.playerInventoryManager.HeadArmorScriptable = armorItem;
            if (playerManager.IsOwner)
                playerManager.playerNetwork.networkArmorHelmetID.Value = armorItem.ID;

            // 4. EQUIP PASSED ARMOR PIECE MODELS
            foreach (var model in armorItem.armorModels)
                model.LoadModel(playerManager, true);

            // 5. RECALCULATE ARMOR VALUES
            playerManager.playerStatsManager.RecalculateTotalArmorStatsImpact();
        }

        public void EquipChestArmor(ChestArmorItem armorItem)
        {
            // 1. UNEQUIP ALL RELATED ARMOR
            UnequipChestArmor();

            // 2. IF PASSED ARMOR PIECE IS NULL -> SET IT TO NULL IN INVENTORY AND RETURN
            if (armorItem == null)
            {
                playerManager.playerInventoryManager.ChestArmorScriptable = null;
                if (playerManager.IsOwner)
                    playerManager.playerNetwork.networkArmorChestplateID.Value = -1;
                return;
            }

            // 3. SET PASSED ARMOR PIECE IN INVENTORY
            playerManager.playerInventoryManager.ChestArmorScriptable = armorItem;
            if (playerManager.IsOwner)
                playerManager.playerNetwork.networkArmorChestplateID.Value = armorItem.ID;

            // 4. EQUIP PASSED ARMOR PIECE MODELS
            foreach (var model in armorItem.armorModels)
                model.LoadModel(playerManager, true);

            // 5. RECALCULATE ARMOR VALUES
            playerManager.playerStatsManager.RecalculateTotalArmorStatsImpact();
        }

        public void EquipHandArmor(HandArmorItem armorItem)
        {
            // 1. UNEQUIP ALL RELATED ARMOR
            UnequipHandArmor();

            // 2. IF PASSED ARMOR PIECE IS NULL -> SET IT TO NULL IN INVENTORY AND RETURN
            if (armorItem == null)
            {
                playerManager.playerInventoryManager.HandArmorScriptable = null;
                if (playerManager.IsOwner)
                    playerManager.playerNetwork.networkArmorGauntletsID.Value = -1;
                return;
            }

            // 3. SET PASSED ARMOR PIECE IN INVENTORY
            playerManager.playerInventoryManager.HandArmorScriptable = armorItem;
            if (playerManager.IsOwner)
                playerManager.playerNetwork.networkArmorGauntletsID.Value = armorItem.ID;

            // 4. EQUIP PASSED ARMOR PIECE MODELS
            foreach (var model in armorItem.armorModels)
                model.LoadModel(playerManager, true);

            // 5. RECALCULATE ARMOR VALUES
            playerManager.playerStatsManager.RecalculateTotalArmorStatsImpact();
        }

        public void EquipLegArmor(LegArmorItem armorItem)
        {
            // 1. UNEQUIP ALL RELATED ARMOR
            UnequipLegArmor();

            // 2. IF PASSED ARMOR PIECE IS NULL -> SET IT TO NULL IN INVENTORY AND RETURN
            if (armorItem == null)
            {
                playerManager.playerInventoryManager.LegArmorScriptable = null;
                if (playerManager.IsOwner)
                    playerManager.playerNetwork.networkArmorLegginsID.Value = -1;
                return;
            }

            // 3. SET PASSED ARMOR PIECE IN INVENTORY
            playerManager.playerInventoryManager.LegArmorScriptable = armorItem;
            if (playerManager.IsOwner)
                playerManager.playerNetwork.networkArmorLegginsID.Value = armorItem.ID;

            // 4. EQUIP PASSED ARMOR PIECE MODELS
            foreach (var model in armorItem.armorModels)
                model.LoadModel(playerManager, true);

            // 5. RECALCULATE ARMOR VALUES
            playerManager.playerStatsManager.RecalculateTotalArmorStatsImpact();
        }

        // ---------------
        // ARMOR - UNEQUIP
        private void UnequipHeadArmor()
        {
            // MALE FULL HELMET
            foreach (GameObject armorItem in maleArmorFullHelmets)
                armorItem.SetActive(false);
        }

        private void UnequipChestArmor()
        {
            // MALE TORSO
            foreach (GameObject armorItem in maleArmorTorsos)
                armorItem.SetActive(false);

            // MALE LEFT UPPER ARM
            foreach (GameObject armorItem in maleArmorLeftUpperArms)
                armorItem.SetActive(false);

            // MALE LEFT LOWER ARM
            foreach (GameObject armorItem in maleArmorLeftLowerArms)
                armorItem.SetActive(false);

            // MALE RIGHT UPPER ARM
            foreach (GameObject armorItem in maleArmorRightUpperArms)
                armorItem.SetActive(false);

            // MALE RIGHT LOWER ARM
            foreach (GameObject armorItem in maleArmorRightLowerArms)
                armorItem.SetActive(false);
        }

        private void UnequipHandArmor()
        {
            // MALE LEFT HAND
            foreach (GameObject armorItem in maleArmorLeftHands)
                armorItem.SetActive(false);

            // MALE RIGHT HAND
            foreach (GameObject armorItem in maleArmorRightHands)
                armorItem.SetActive(false);
        }

        private void UnequipLegArmor()
        {
            // MALE LEFT UPPER LEG
            foreach (GameObject armorItem in maleArmorLeftUpperLegs)
                armorItem.SetActive(false);

            // MALE LEFT LOWER LEG
            foreach (GameObject armorItem in maleArmorLeftLowerLegs)
                armorItem.SetActive(false);

            // MALE LEFT LEG
            foreach (GameObject armorItem in maleArmorLeftLegs)
                armorItem.SetActive(false);

            // MALE RIGHT UPPER LEG
            foreach (GameObject armorItem in maleArmorRightUpperLegs)
                armorItem.SetActive(false);

            // MALE RIGHT LOWER LEG
            foreach (GameObject armorItem in maleArmorRightLowerLegs)
                armorItem.SetActive(false);

            // MALE RIGHT LEG
            foreach (GameObject armorItem in maleArmorRightLegs)
                armorItem.SetActive(false);
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
            if (playerManager.playerNetwork.networkIsTwoHanding.Value)
            {
                if (playerManager.playerNetwork.networkIsTwoHandingLeftWeapon.Value)
                {
                    LeftHandWeaponManager.ActivateDamageCollider(true);
                    playerManager.playerSFXManager.PlayAudioClip(
                        SFXManager.instance.GetRandomSFX(ref playerManager.playerInventoryManager.TwoHandedWeaponScriptable.whooshesSoundFX));
                }

                if (playerManager.playerNetwork.networkIsTwoHandingRightWeapon.Value)
                {
                    RightHandWeaponManager.ActivateDamageCollider(true);
                    playerManager.playerSFXManager.PlayAudioClip(
                        SFXManager.instance.GetRandomSFX(ref playerManager.playerInventoryManager.TwoHandedWeaponScriptable.whooshesSoundFX));
                }
            }
            else
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
        }

        public void DeactivateDamageCollider()
        {
            if (LeftHandWeaponManager != null)
                LeftHandWeaponManager.ActivateDamageCollider(false);

            if (RightHandWeaponManager != null)
                RightHandWeaponManager.ActivateDamageCollider(false);
        }
    }
}
