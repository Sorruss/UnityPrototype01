using UnityEngine;

namespace FG
{
    public class Utility_DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private float Time = 5.0f;

        private void Awake()
        {
            Destroy(gameObject, Time);
        }
    }
}
