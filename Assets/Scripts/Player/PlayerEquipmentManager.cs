using UnityEngine;
using System.Collections.Generic;

namespace FG
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        private PlayerManager playerManager;

        // -------
        // WEAPONS
        [Header("----------- WEAPONS -----------")]

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

        // ----------
        // EQUIPMENT UNISEX
        [Header("----------- EQUIPMENT UNISEX -----------")]
        [Header("Equipment Unisex Head (Hand)")]
        [SerializeField] private GameObject EquipmentHatParent;
        [HideInInspector] public GameObject[] equipmentHats;
        [SerializeField] private GameObject EquipmentHoodParent;
        [HideInInspector] public GameObject[] equipmentHoods;
        [SerializeField] private GameObject EquipmentFaceCoverParent;
        [HideInInspector] public GameObject[] equipmentFaceCovers;
        [SerializeField] private GameObject EquipmentHelmetAccessoryParent;
        [HideInInspector] public GameObject[] equipmentHelmetAccessories;
        
        [Header("Equipment Unisex Chest (Hand)")]
        [SerializeField] private GameObject EquipmentBackAccessoryParent;
        [HideInInspector] public GameObject[] equipmentBackAccessories;
        [SerializeField] private GameObject EquipmentLeftShoulderParent;
        [HideInInspector] public GameObject[] equipmentLeftShoulders;
        [SerializeField] private GameObject EquipmentRightShoulderParent;
        [HideInInspector] public GameObject[] equipmentRightShoulders;

        [Header("Equipment Unisex Hand (Hand)")]
        [SerializeField] private GameObject EquipmentLeftElbowParent;
        [HideInInspector] public GameObject[] equipmentLeftElbows;
        [SerializeField] private GameObject EquipmentRightElbowParent;
        [HideInInspector] public GameObject[] equipmentRightElbows;

        [Header("Equipment Unisex Leg (Hand)")]
        [SerializeField] private GameObject EquipmentHipsParent;
        [HideInInspector] public GameObject[] equipmentHips;
        [SerializeField] private GameObject EquipmentLeftKneeParent;
        [HideInInspector] public GameObject[] equipmentLeftKnees;
        [SerializeField] private GameObject EquipmentRightKneeParent;
        [HideInInspector] public GameObject[] equipmentRightKnees;

        // ----------
        // ARMOR MALE
        [Header("----------- ARMOR MALE -----------")]
        // ARMOR MALE HEAD
        [Header("Armor Male Head (Hand)")]
        [SerializeField] private GameObject MaleArmorFullHelmetParent;
        [HideInInspector] public GameObject[] maleArmorFullHelmets;

        // ARMOR MALE CHEST
        [Header("Armor Male Chest (Hand)")]
        [SerializeField] private GameObject MaleArmorTorsoParent;
        [HideInInspector] public GameObject[] maleArmorTorsos;
        [SerializeField] private GameObject MaleArmorLeftUpperArmParent;
        [HideInInspector] public GameObject[] maleArmorLeftUpperArms;
        [SerializeField] private GameObject MaleArmorRightUpperArmParent;
        [HideInInspector] public GameObject[] maleArmorRightUpperArms;
        
        // ARMOR MALE HAND
        [Header("Armor Male Hand (Hand)")]
        [SerializeField] private GameObject MaleArmorLeftLowerArmParent;
        [HideInInspector] public GameObject[] maleArmorLeftLowerArms;
        [SerializeField] private GameObject MaleArmorLeftHandParent;
        [HideInInspector] public GameObject[] maleArmorLeftHands;
        [SerializeField] private GameObject MaleArmorRightLowerArmParent;
        [HideInInspector] public GameObject[] maleArmorRightLowerArms;
        [SerializeField] private GameObject MaleArmorRightHandParent;
        [HideInInspector] public GameObject[] maleArmorRightHands;

        // ARMOR MALE LEG
        [Header("Armor Male Leg (Hand)")]
        [SerializeField] private GameObject MaleArmorHipsParent;
        [HideInInspector] public GameObject[] maleArmorHips;
        [SerializeField] private GameObject MaleArmorLeftLegParent;
        [HideInInspector] public GameObject[] maleArmorLeftLegs;
        [SerializeField] private GameObject MaleArmorRightLegParent;
        [HideInInspector] public GameObject[] maleArmorRightLegs;

        // ------------
        // ARMOR FEMALE
        [Header("----------- ARMOR FEMALE -----------")]
        // ARMOR MALE HEAD
        [Header("Armor Male Head (Hand)")]
        [SerializeField] private GameObject FemaleArmorFullHelmetParent;
        [HideInInspector] public GameObject[] femaleArmorFullHelmets;

        // ARMOR MALE CHEST
        [Header("Armor Male Chest (Hand)")]
        [SerializeField] private GameObject FemaleArmorTorsoParent;
        [HideInInspector] public GameObject[] femaleArmorTorsos;
        [SerializeField] private GameObject FemaleArmorLeftUpperArmParent;
        [HideInInspector] public GameObject[] femaleArmorLeftUpperArms;
        [SerializeField] private GameObject FemaleArmorRightUpperArmParent;
        [HideInInspector] public GameObject[] femaleArmorRightUpperArms;

        // ARMOR MALE HAND
        [Header("Armor Male Hand (Hand)")]
        [SerializeField] private GameObject FemaleArmorLeftLowerArmParent;
        [HideInInspector] public GameObject[] femaleArmorLeftLowerArms;
        [SerializeField] private GameObject FemaleArmorLeftHandParent;
        [HideInInspector] public GameObject[] femaleArmorLeftHands;
        [SerializeField] private GameObject FemaleArmorRightLowerArmParent;
        [HideInInspector] public GameObject[] femaleArmorRightLowerArms;
        [SerializeField] private GameObject FemaleArmorRightHandParent;
        [HideInInspector] public GameObject[] femaleArmorRightHands;

        // ARMOR MALE LEG
        [Header("Armor Male Leg (Hand)")]
        [SerializeField] private GameObject FemaleArmorHipsParent;
        [HideInInspector] public GameObject[] femaleArmorHips;
        [SerializeField] private GameObject FemaleArmorLeftLegParent;
        [HideInInspector] public GameObject[] femaleArmorLeftLegs;
        [SerializeField] private GameObject FemaleArmorRightLegParent;
        [HideInInspector] public GameObject[] femaleArmorRightLegs;

        [Header("Debug")]
        public bool debugEquipArmor = false;
        public bool debugChangeGender = false;

        protected override void Awake()
        {
            base.Awake();

            playerManager = GetComponent<PlayerManager>();

            // WEAPONS
            FindSockets();

            // EQUIPMENT
            FindArmorMale();
            FindArmorFemale();
            FindEquipmentUnisex();
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
                EquipFullArmor();
            }

            if (debugChangeGender)
            {
                debugChangeGender = false;

                if (!playerManager.IsOwner)
                    playerManager.playerBodyManager.SwapGender(playerManager.playerNetwork.networkIsMale.Value);
                else
                    playerManager.playerNetwork.networkIsMale.Value = !playerManager.playerNetwork.networkIsMale.Value;
            }
        }

        // --------------
        // WEAPON SOCKETS
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

        // ----------------------
        // FIND ARMOR / EQUIPMENT
        private void FindArmorMale()
        {
            // FULL HELMET
            List<GameObject> children = new();
            foreach (Transform model in MaleArmorFullHelmetParent.transform)
                children.Add(model.gameObject);
            maleArmorFullHelmets = children.ToArray();

            // TORSO
            children.Clear();
            foreach (Transform model in MaleArmorTorsoParent.transform)
                children.Add(model.gameObject);
            maleArmorTorsos = children.ToArray();

            // LEFT UPPER ARM
            children.Clear();
            foreach (Transform model in MaleArmorLeftUpperArmParent.transform)
                children.Add(model.gameObject);
            maleArmorLeftUpperArms = children.ToArray();

            // RIGHT UPPER ARM
            children.Clear();
            foreach (Transform model in MaleArmorRightUpperArmParent.transform)
                children.Add(model.gameObject);
            maleArmorRightUpperArms = children.ToArray();

            // LEFT LOWER ARM
            children.Clear();
            foreach (Transform model in MaleArmorLeftLowerArmParent.transform)
                children.Add(model.gameObject);
            maleArmorLeftLowerArms = children.ToArray();

            // LEFT HAND
            children.Clear();
            foreach (Transform model in MaleArmorLeftHandParent.transform)
                children.Add(model.gameObject);
            maleArmorLeftHands = children.ToArray();

            // RIGHT LOWER ARM
            children.Clear();
            foreach (Transform model in MaleArmorRightLowerArmParent.transform)
                children.Add(model.gameObject);
            maleArmorRightLowerArms = children.ToArray();

            // RIGHT HAND
            children.Clear();
            foreach (Transform model in MaleArmorRightHandParent.transform)
                children.Add(model.gameObject);
            maleArmorRightHands = children.ToArray();

            // HIPS
            children.Clear();
            foreach (Transform model in MaleArmorHipsParent.transform)
                children.Add(model.gameObject);
            maleArmorHips = children.ToArray();

            // LEFT LEG
            children.Clear();
            foreach (Transform model in MaleArmorLeftLegParent.transform)
                children.Add(model.gameObject);
            maleArmorLeftLegs = children.ToArray();

            // RIGHT LEG
            children.Clear();
            foreach (Transform model in MaleArmorRightLegParent.transform)
                children.Add(model.gameObject);
            maleArmorRightLegs = children.ToArray();
        }

        private void FindArmorFemale()
        {
            // FULL HELMET
            List<GameObject> children = new();
            foreach (Transform model in FemaleArmorFullHelmetParent.transform)
                children.Add(model.gameObject);
            femaleArmorFullHelmets = children.ToArray();

            // TORSO
            children.Clear();
            foreach (Transform model in FemaleArmorTorsoParent.transform)
                children.Add(model.gameObject);
            femaleArmorTorsos = children.ToArray();

            // LEFT UPPER ARM
            children.Clear();
            foreach (Transform model in FemaleArmorLeftUpperArmParent.transform)
                children.Add(model.gameObject);
            femaleArmorLeftUpperArms = children.ToArray();

            // RIGHT UPPER ARM
            children.Clear();
            foreach (Transform model in FemaleArmorRightUpperArmParent.transform)
                children.Add(model.gameObject);
            femaleArmorRightUpperArms = children.ToArray();

            // LEFT LOWER ARM
            children.Clear();
            foreach (Transform model in FemaleArmorLeftLowerArmParent.transform)
                children.Add(model.gameObject);
            femaleArmorLeftLowerArms = children.ToArray();

            // LEFT HAND
            children.Clear();
            foreach (Transform model in FemaleArmorLeftHandParent.transform)
                children.Add(model.gameObject);
            femaleArmorLeftHands = children.ToArray();

            // RIGHT LOWER ARM
            children.Clear();
            foreach (Transform model in FemaleArmorRightLowerArmParent.transform)
                children.Add(model.gameObject);
            femaleArmorRightLowerArms = children.ToArray();

            // RIGHT HAND
            children.Clear();
            foreach (Transform model in FemaleArmorRightHandParent.transform)
                children.Add(model.gameObject);
            femaleArmorRightHands = children.ToArray();

            // HIPS
            children.Clear();
            foreach (Transform model in FemaleArmorHipsParent.transform)
                children.Add(model.gameObject);
            femaleArmorHips = children.ToArray();

            // LEFT LEG
            children.Clear();
            foreach (Transform model in FemaleArmorLeftLegParent.transform)
                children.Add(model.gameObject);
            femaleArmorLeftLegs = children.ToArray();

            // RIGHT LEG
            children.Clear();
            foreach (Transform model in FemaleArmorRightLegParent.transform)
                children.Add(model.gameObject);
            femaleArmorRightLegs = children.ToArray();
        }

        private void FindEquipmentUnisex()
        {
            // HAT
            List<GameObject> children = new();
            foreach (Transform model in EquipmentHatParent.transform)
                children.Add(model.gameObject);
            equipmentHats = children.ToArray();

            // HOOD
            children.Clear();
            foreach (Transform model in EquipmentHoodParent.transform)
                children.Add(model.gameObject);
            equipmentHoods = children.ToArray();

            // FACE COVER
            children.Clear();
            foreach (Transform model in EquipmentFaceCoverParent.transform)
                children.Add(model.gameObject);
            equipmentFaceCovers = children.ToArray();

            // HELMET
            children.Clear();
            foreach (Transform model in EquipmentHelmetAccessoryParent.transform)
                children.Add(model.gameObject);
            equipmentHelmetAccessories = children.ToArray();

            // BACK
            children.Clear();
            foreach (Transform model in EquipmentBackAccessoryParent.transform)
                children.Add(model.gameObject);
            equipmentBackAccessories = children.ToArray();

            // LEFT SHOULDER
            children.Clear();
            foreach (Transform model in EquipmentLeftShoulderParent.transform)
                children.Add(model.gameObject);
            equipmentLeftShoulders = children.ToArray();

            // RIGHT SHOULDER
            children.Clear();
            foreach (Transform model in EquipmentRightShoulderParent.transform)
                children.Add(model.gameObject);
            equipmentRightShoulders = children.ToArray();

            // LEFT ELBOW
            children.Clear();
            foreach (Transform model in EquipmentLeftElbowParent.transform)
                children.Add(model.gameObject);
            equipmentLeftElbows = children.ToArray();

            // RIGHT ELBOW
            children.Clear();
            foreach (Transform model in EquipmentRightElbowParent.transform)
                children.Add(model.gameObject);
            equipmentRightElbows = children.ToArray();

            // HIPS
            children.Clear();
            foreach (Transform model in EquipmentHipsParent.transform)
                children.Add(model.gameObject);
            equipmentHips = children.ToArray();

            // LEFT KNEE
            children.Clear();
            foreach (Transform model in EquipmentLeftKneeParent.transform)
                children.Add(model.gameObject);
            equipmentLeftKnees = children.ToArray();

            // RIGHT KNEE
            children.Clear();
            foreach (Transform model in EquipmentRightKneeParent.transform)
                children.Add(model.gameObject);
            equipmentRightKnees = children.ToArray();
        }

        // ----------------
        // LEFT HAND WEAPON
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
                    if (playerManager.playerInventoryManager.LeftHandWeaponScriptables[i].ID != ItemDatabase.instance.unarmedWeapon.ID)
                    {
                        anotherWeapon = playerManager.playerInventoryManager.LeftHandWeaponScriptables[i];
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
                    if (playerManager.playerInventoryManager.LeftHandWeaponScriptables[i].ID != ItemDatabase.instance.unarmedWeapon.ID)
                    {
                        anotherWeapon = playerManager.playerInventoryManager.LeftHandWeaponScriptables[i];
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
        // RIGT HAND WEAPON
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
        public void EquipFullArmor()
        {
            EquipHeadArmor(playerManager.playerInventoryManager.HeadArmorScriptable);
            EquipChestArmor(playerManager.playerInventoryManager.ChestArmorScriptable);
            EquipHandArmor(playerManager.playerInventoryManager.HandArmorScriptable);
            EquipLegArmor(playerManager.playerInventoryManager.LegArmorScriptable);
        }

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
                model.LoadModel(playerManager, playerManager.playerNetwork.networkIsMale.Value);

            // 5. DISABLE CERTAIN BODY FEATURES DEPENDING ON THE HEAD PIECE TYPE
            switch (armorItem.headEquipmentType)
            {
                case HeadEquipmentType.HAT:
                    break;
                case HeadEquipmentType.HOOD:
                    playerManager.playerBodyManager.DisableHair();
                    break;
                case HeadEquipmentType.FACE_COVER:
                    playerManager.playerBodyManager.DisableFacialHair();
                    break;
                case HeadEquipmentType.FULL_HELMET:
                    playerManager.playerBodyManager.DisableHair();
                    playerManager.playerBodyManager.DisableHead();
                    break;
                default:
                    break;
            }

            // 6. RECALCULATE ARMOR VALUES
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

            // 3. DISABLE BODY PARTS
            playerManager.playerBodyManager.DisableBody();

            // 4. SET PASSED ARMOR PIECE IN INVENTORY
            playerManager.playerInventoryManager.ChestArmorScriptable = armorItem;
            if (playerManager.IsOwner)
                playerManager.playerNetwork.networkArmorChestplateID.Value = armorItem.ID;

            // 5. EQUIP PASSED ARMOR PIECE MODELS
            foreach (var model in armorItem.armorModels)
                model.LoadModel(playerManager, playerManager.playerNetwork.networkIsMale.Value);

            // 6. RECALCULATE ARMOR VALUES
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

            // 3. DISABLE BODY PARTS
            playerManager.playerBodyManager.DisableHands();

            // 4. SET PASSED ARMOR PIECE IN INVENTORY
            playerManager.playerInventoryManager.HandArmorScriptable = armorItem;
            if (playerManager.IsOwner)
                playerManager.playerNetwork.networkArmorGauntletsID.Value = armorItem.ID;

            // 5. EQUIP PASSED ARMOR PIECE MODELS
            foreach (var model in armorItem.armorModels)
                model.LoadModel(playerManager, playerManager.playerNetwork.networkIsMale.Value);

            // 6. RECALCULATE ARMOR VALUES
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

            // 3. DISABLE BODY PARTS
            playerManager.playerBodyManager.DisableLegs();

            // 4. SET PASSED ARMOR PIECE IN INVENTORY
            playerManager.playerInventoryManager.LegArmorScriptable = armorItem;
            if (playerManager.IsOwner)
                playerManager.playerNetwork.networkArmorLegginsID.Value = armorItem.ID;

            // 5. EQUIP PASSED ARMOR PIECE MODELS
            foreach (var model in armorItem.armorModels)
                model.LoadModel(playerManager, playerManager.playerNetwork.networkIsMale.Value);

            // 6. RECALCULATE ARMOR VALUES
            playerManager.playerStatsManager.RecalculateTotalArmorStatsImpact();
        }

        // ---------------
        // ARMOR - UNEQUIP
        private void UnequipHeadArmor()
        {
            // ----
            // MALE
            foreach (GameObject armorItem in maleArmorFullHelmets)
                armorItem.SetActive(false);

            // ------
            // FEMALE
            foreach (GameObject armorItem in femaleArmorFullHelmets)
                armorItem.SetActive(false);

            // ------
            // UNISEX
            // HATS
            foreach (GameObject equipment in equipmentHats)
                equipment.SetActive(false);

            // HOODS
            foreach (GameObject equipment in equipmentHoods)
                equipment.SetActive(false);

            // FACE COVERS
            foreach (GameObject equipment in equipmentFaceCovers)
                equipment.SetActive(false);

            // HELMET ACCESSORIES
            foreach (GameObject equipment in equipmentHelmetAccessories)
                equipment.SetActive(false);

            // -------------
            // BODY FEATURES
            playerManager.playerBodyManager.EnableHead();
            playerManager.playerBodyManager.EnableHair();
        }

        private void UnequipChestArmor()
        {
            // ----
            // MALE
            // MALE TORSO
            foreach (GameObject armorItem in maleArmorTorsos)
                armorItem.SetActive(false);

            // MALE LEFT UPPER ARM
            foreach (GameObject armorItem in maleArmorLeftUpperArms)
                armorItem.SetActive(false);

            // MALE RIGHT UPPER ARM
            foreach (GameObject armorItem in maleArmorRightUpperArms)
                armorItem.SetActive(false);

            // ------
            // FEMALE
            // FEMALE TORSO
            foreach (GameObject armorItem in femaleArmorTorsos)
                armorItem.SetActive(false);

            // FEMALE LEFT UPPER ARM
            foreach (GameObject armorItem in femaleArmorLeftUpperArms)
                armorItem.SetActive(false);

            // FEMALE RIGHT UPPER ARM
            foreach (GameObject armorItem in femaleArmorRightUpperArms)
                armorItem.SetActive(false);

            // ------
            // UNISEX
            // BACK ACCESSORIES
            foreach (GameObject equipment in equipmentBackAccessories)
                equipment.SetActive(false);

            // LEFT SHOULDER
            foreach (GameObject equipment in equipmentLeftShoulders)
                equipment.SetActive(false);

            // LEFT ELBOW
            foreach (GameObject equipment in equipmentLeftElbows)
                equipment.SetActive(false);

            // RIGHT SHOULDER
            foreach (GameObject equipment in equipmentRightShoulders)
                equipment.SetActive(false);

            // RIGHT ELBOW
            foreach (GameObject equipment in equipmentRightElbows)
                equipment.SetActive(false);

            // -------------
            // BODY FEATURES
            playerManager.playerBodyManager.EnableBody();
        }

        private void UnequipHandArmor()
        {
            // ----
            // MALE
            // MALE LEFT LOWER ARM
            foreach (GameObject armorItem in maleArmorLeftLowerArms)
                armorItem.SetActive(false);

            // MALE LEFT HAND
            foreach (GameObject armorItem in maleArmorLeftHands)
                armorItem.SetActive(false);

            // MALE RIGHT LOWER ARM
            foreach (GameObject armorItem in maleArmorRightLowerArms)
                armorItem.SetActive(false);

            // MALE RIGHT HAND
            foreach (GameObject armorItem in maleArmorRightHands)
                armorItem.SetActive(false);

            // ------
            // FEMALE
            // FEMALE LEFT LOWER ARM
            foreach (GameObject armorItem in femaleArmorLeftLowerArms)
                armorItem.SetActive(false);

            // FEMALE LEFT HAND
            foreach (GameObject armorItem in femaleArmorLeftHands)
                armorItem.SetActive(false);

            // FEMALE RIGHT LOWER ARM
            foreach (GameObject armorItem in femaleArmorRightLowerArms)
                armorItem.SetActive(false);

            // FEMALE RIGHT HAND
            foreach (GameObject armorItem in femaleArmorRightHands)
                armorItem.SetActive(false);

            // -------------
            // BODY FEATURES
            playerManager.playerBodyManager.EnableHands();
        }

        private void UnequipLegArmor()
        {
            // ----
            // MALE
            // MALE HIPS
            foreach (GameObject armorItem in maleArmorHips)
                armorItem.SetActive(false);

            // MALE LEFT LEG
            foreach (GameObject armorItem in maleArmorLeftLegs)
                armorItem.SetActive(false);

            // MALE RIGHT LEG
            foreach (GameObject armorItem in maleArmorRightLegs)
                armorItem.SetActive(false);

            // ------
            // FEMALE
            // FEMALE HIPS
            foreach (GameObject armorItem in femaleArmorHips)
                armorItem.SetActive(false);

            // FEMALE LEFT LEG
            foreach (GameObject armorItem in femaleArmorLeftLegs)
                armorItem.SetActive(false);

            // FEMALE RIGHT LEG
            foreach (GameObject armorItem in femaleArmorRightLegs)
                armorItem.SetActive(false);

            // ------
            // UNISEX
            // HIPS
            foreach (GameObject equipment in equipmentHips)
                equipment.SetActive(false);

            // LEFT KNEE
            foreach (GameObject equipment in equipmentLeftKnees)
                equipment.SetActive(false);

            // RIGHT KNEE
            foreach (GameObject equipment in equipmentRightKnees)
                equipment.SetActive(false);

            // -------------
            // BODY FEATURES
            playerManager.playerBodyManager.EnableLegs();
        }

        // --------------
        // COMMON METHODS
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
