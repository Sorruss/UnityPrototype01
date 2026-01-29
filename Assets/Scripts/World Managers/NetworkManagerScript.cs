using UnityEngine;
using Unity.Netcode;

namespace FG
{
    public class NetworkManagerScript : MonoBehaviour
    {
        [HideInInspector] public static NetworkManagerScript instance { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void StartAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartAsClient()
        {
            NetworkManager.Singleton.StartClient();
        }

        public void Shutdown()
        {
            NetworkManager.Singleton.Shutdown();
        }
    }
}
