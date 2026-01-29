using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG
{
    public class ActionDatabase : MonoBehaviour
    {
        [HideInInspector] public static ActionDatabase instance;

        [Header("Weapon Actions")]
        public List<WeaponAction> actions = new List<WeaponAction>();

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            AssignIDs();
        }

        private void AssignIDs()
        {
            for (int i = 0; i < actions.Count; ++i)
            {
                actions[i].ActionID = i;
            }
        }

        public WeaponAction GetWeaponActionByID(int id)
        {
            return actions.FirstOrDefault(action => action.ActionID == id);
        }
    }
}
