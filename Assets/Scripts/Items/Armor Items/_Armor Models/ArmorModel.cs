using UnityEngine;

namespace FG
{
    [CreateAssetMenu(menuName = "Item/Armor Model")]
    public class ArmorModel : ScriptableObject
    {
        [Header("Model Type")]
        public EquipmentModelType equipmentModelType;

        [Header("Model Names")]
        public string maleModelName;
        public string femaleModelName;

        public void LoadModel(PlayerManager player, bool isMale)
        {
            if (isMale)
                LoadMaleModel(player);
            else
                LoadFemaleModel(player);
        }

        private void LoadMaleModel(PlayerManager player)
        {
            switch (equipmentModelType)
            {
                case EquipmentModelType.FULL_HELMET:
                    foreach (var model in player.playerEquipmentManager.maleArmorFullHelmets)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.OPEN_HELMET:
                    break;
                case EquipmentModelType.HOOD:
                    break;
                case EquipmentModelType.HEAD_ACESSORIE:
                    break;
                case EquipmentModelType.FACE_COVER:
                    break;
                case EquipmentModelType.TORSO:
                    foreach (var model in player.playerEquipmentManager.maleArmorTorsos)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.BACK:
                    break;
                case EquipmentModelType.LEFT_SHOULDER:
                    break;
                case EquipmentModelType.LEFT_UPPERARM:
                    foreach (var model in player.playerEquipmentManager.maleArmorLeftUpperArms)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_ELBOW:
                    break;
                case EquipmentModelType.LEFT_LOWERARM:
                    foreach (var model in player.playerEquipmentManager.maleArmorLeftLowerArms)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_HAND:
                    foreach (var model in player.playerEquipmentManager.maleArmorLeftHands)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_SHOULDER:
                    break;
                case EquipmentModelType.RIGHT_UPPERARM:
                    foreach (var model in player.playerEquipmentManager.maleArmorRightUpperArms)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_ELBOW:
                    break;
                case EquipmentModelType.RIGHT_LOWERARM:
                    foreach (var model in player.playerEquipmentManager.maleArmorRightLowerArms)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_HAND:
                    foreach (var model in player.playerEquipmentManager.maleArmorRightHands)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.HIPS:
                    break;
                case EquipmentModelType.HIPS_ATTACHMENT:
                    break;
                case EquipmentModelType.LEFT_UPPERLEG:
                    foreach (var model in player.playerEquipmentManager.maleArmorLeftUpperLegs)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_KNEE:
                    break;
                case EquipmentModelType.LEFT_LOWERLEG:
                    foreach (var model in player.playerEquipmentManager.maleArmorLeftLowerLegs)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_LEG:
                    foreach (var model in player.playerEquipmentManager.maleArmorLeftLegs)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_UPPERLEG:
                    foreach (var model in player.playerEquipmentManager.maleArmorRightUpperLegs)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_KNEE:
                    break;
                case EquipmentModelType.RIGHT_LOWERLEG:
                    foreach (var model in player.playerEquipmentManager.maleArmorRightLowerLegs)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_LEG:
                    foreach (var model in player.playerEquipmentManager.maleArmorRightLegs)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }

        private void LoadFemaleModel(PlayerManager player)
        {

        }
    }
}
