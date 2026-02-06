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
                case EquipmentModelType.HAT:
                    foreach (var model in player.playerEquipmentManager.equipmentHats)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.HOOD:
                    foreach (var model in player.playerEquipmentManager.equipmentHoods)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.HEAD_ACESSORIE:
                    foreach (var model in player.playerEquipmentManager.equipmentHelmetAccessories)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.FACE_COVER:
                    foreach (var model in player.playerEquipmentManager.equipmentFaceCovers)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
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
                    foreach (var model in player.playerEquipmentManager.equipmentBackAccessories)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_SHOULDER:
                    foreach (var model in player.playerEquipmentManager.equipmentLeftShoulders)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
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
                    foreach (var model in player.playerEquipmentManager.equipmentLeftElbows)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
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
                    foreach (var model in player.playerEquipmentManager.equipmentRightShoulders)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
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
                    foreach (var model in player.playerEquipmentManager.equipmentRightElbows)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
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
                    foreach (var model in player.playerEquipmentManager.maleArmorHips)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.HIPS_ATTACHMENT:
                    foreach (var model in player.playerEquipmentManager.equipmentHips)
                    {
                        if (model.name != maleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_KNEE:
                    foreach (var model in player.playerEquipmentManager.equipmentLeftKnees)
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
                case EquipmentModelType.RIGHT_KNEE:
                    foreach (var model in player.playerEquipmentManager.equipmentRightKnees)
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
            switch (equipmentModelType)
            {
                case EquipmentModelType.FULL_HELMET:
                    foreach (var model in player.playerEquipmentManager.femaleArmorFullHelmets)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.HAT:
                    foreach (var model in player.playerEquipmentManager.equipmentHats)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.HOOD:
                    foreach (var model in player.playerEquipmentManager.equipmentHoods)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.HEAD_ACESSORIE:
                    foreach (var model in player.playerEquipmentManager.equipmentHelmetAccessories)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.FACE_COVER:
                    foreach (var model in player.playerEquipmentManager.equipmentFaceCovers)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.TORSO:
                    foreach (var model in player.playerEquipmentManager.femaleArmorTorsos)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.BACK:
                    foreach (var model in player.playerEquipmentManager.equipmentBackAccessories)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_SHOULDER:
                    foreach (var model in player.playerEquipmentManager.equipmentLeftShoulders)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_UPPERARM:
                    foreach (var model in player.playerEquipmentManager.femaleArmorLeftUpperArms)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_ELBOW:
                    foreach (var model in player.playerEquipmentManager.equipmentLeftElbows)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_LOWERARM:
                    foreach (var model in player.playerEquipmentManager.femaleArmorLeftLowerArms)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_HAND:
                    foreach (var model in player.playerEquipmentManager.femaleArmorLeftHands)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_SHOULDER:
                    foreach (var model in player.playerEquipmentManager.equipmentRightShoulders)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_UPPERARM:
                    foreach (var model in player.playerEquipmentManager.femaleArmorRightUpperArms)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_ELBOW:
                    foreach (var model in player.playerEquipmentManager.equipmentRightElbows)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_LOWERARM:
                    foreach (var model in player.playerEquipmentManager.femaleArmorRightLowerArms)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_HAND:
                    foreach (var model in player.playerEquipmentManager.femaleArmorRightHands)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.HIPS:
                    foreach (var model in player.playerEquipmentManager.femaleArmorHips)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.HIPS_ATTACHMENT:
                    foreach (var model in player.playerEquipmentManager.equipmentHips)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_KNEE:
                    foreach (var model in player.playerEquipmentManager.equipmentLeftKnees)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.LEFT_LEG:
                    foreach (var model in player.playerEquipmentManager.femaleArmorLeftLegs)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_KNEE:
                    foreach (var model in player.playerEquipmentManager.equipmentRightKnees)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                case EquipmentModelType.RIGHT_LEG:
                    foreach (var model in player.playerEquipmentManager.femaleArmorRightLegs)
                    {
                        if (model.name != femaleModelName)
                            continue;

                        model.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
