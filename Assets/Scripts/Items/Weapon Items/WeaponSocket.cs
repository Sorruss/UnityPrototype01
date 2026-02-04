using UnityEngine;

namespace FG
{   // CLASS WHICH ATTACHES TO BONES TO LOCATE SOCKET TO ATTACH WEAPON SCRIPTABLE TO.
    public class WeaponSocket : MonoBehaviour
    {
        public CharacterWeaponSocket socket;
        private GameObject model;

        public void UnloadModel()
        {
            if (model != null)
            {
                Destroy(model);
            }
        }

        public void PlaceModelAsEquipped(GameObject weaponModel)
        {
            model = weaponModel;
            model.transform.parent = transform;

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }

        public void PlaceModelAsUnequipped(GameObject weaponModel, WeaponClass weaponClass)
        {
            model = weaponModel;
            model.transform.parent = transform;
            model.transform.localScale = Vector3.one;

            if (weaponClass == WeaponClass.STRAIGHT_SWORD)
            {
                model.transform.localPosition = new Vector3(-0.3273017f, -0.002505384f, -0.1036283f);
                model.transform.localRotation = Quaternion.Euler(-17.194f, 230.278f, 167.929f);
            }
            else if (weaponClass == WeaponClass.SHIELD)
            {
                model.transform.localPosition = new Vector3(0.03637858f, 0.02979704f, -0.06785431f);
                model.transform.localRotation = Quaternion.Euler(-11.155f, -192.938f, -19.209f);
            }
        }
    }
}
