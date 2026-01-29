using UnityEngine;

namespace FG
{
    public class NetworkObjectSpawner : MonoBehaviour
    {
        [SerializeField] public GameObject prefubToSpawn;

        private void Start()
        {
            NetworkObjectsManager.instance.TryToSpawnObject(this);
            gameObject.SetActive(false);
        }
    }
}
