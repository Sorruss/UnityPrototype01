using UnityEngine;

namespace FG
{
    public class PlayerUIManager : MonoBehaviour
    {
        [HideInInspector] public static PlayerUIManager instance { get; private set; }

        [Header("Debug")]
        [SerializeField] private bool playAsClient = false;

        [Header("Flags")]
        public bool isPopUpOpened = false;

        [HideInInspector] public UI_HudManager hudManager;
        [HideInInspector] public UI_PopUpManager popUpManager;

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

            hudManager = GetComponentInChildren<UI_HudManager>();
            popUpManager = GetComponentInChildren<UI_PopUpManager>();
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (playAsClient)
            {
                playAsClient = false;
                NetworkManagerScript.instance.Shutdown();
                NetworkManagerScript.instance.StartAsClient();
            }
        }

        public void EnableCursor(bool enable)
        {
            Cursor.visible = enable;
            Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
