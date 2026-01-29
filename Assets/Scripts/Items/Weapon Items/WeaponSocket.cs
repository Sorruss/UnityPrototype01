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

        public void LoadModel(GameObject weaponModel)
        {
            model = weaponModel;
            model.transform.parent = transform;

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }
    }
}
