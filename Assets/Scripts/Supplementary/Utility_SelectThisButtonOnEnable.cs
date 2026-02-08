using UnityEngine;
using UnityEngine.UI;

namespace FG
{
    public class Utility_SelectThisButtonOnEnable : MonoBehaviour
    {
        [Header("Button To Select")]
        [SerializeField] private Button button;

        private void OnEnable()
        {
            button.Select();
            button.OnSelect(null);
        }
    }
}
