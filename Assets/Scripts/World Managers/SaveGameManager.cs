using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace FG
{
    public class SaveGameManager : MonoBehaviour
    {
        [HideInInspector] public static SaveGameManager instance { get; private set; }

        [Header("Don't change")]
        [SerializeField] private int worldSceneIndex = 1;
        [SerializeField] public int characterSaveSlots = 10;
        [SerializeField] public CharacterSaveSlot currentSaveSlot;
        [HideInInspector] public CharacterSaveData currentSaveData;

        [Header("Temporary")]
        [SerializeField] private bool SaveGameFlag = false;
        [SerializeField] private bool LoadGameFlag = false;
        [SerializeField] private bool DebugLog = false;

        private SaveFileManager saveFileManager;
        
        [HideInInspector] public PlayerManager player;

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

            saveFileManager = new();
        }

        private void Update()
        {
            if (LoadGameFlag)
            {
                LoadGameFlag = false;
                LoadGame();
            }

            if (SaveGameFlag)
            {
                SaveGameFlag = false;
                SaveGame();
            }

            if (DebugLog)
            {
                DebugLog = false;
                currentSaveData.bossListAwakened.DebugLog();
                currentSaveData.bossListDefeated.DebugLog();
            }
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        // GAME SAVE FUNCTIONALITY.
        public bool TryCreateNewGame()
        {
            int slot = GetAvailableSaveSlot();
            if (slot == -1)
            {
                return false;
            }
            
            currentSaveSlot = (CharacterSaveSlot)(slot);
            SaveGame();
            
            return true;
        }

        public void SaveGame()
        {
            // Populate data with values from player.
            player.ExportSaveData(currentSaveData);

            // Save populated data locally.
            saveFileManager.ConfigureSaveFileManager(currentSaveSlot);
            saveFileManager.SaveFile(currentSaveData);
        }

        public void LoadGame()
        {
            // Get saved data from locally saved file.
            saveFileManager.ConfigureSaveFileManager(currentSaveSlot);
            currentSaveData = saveFileManager.LoadFile();

            // Apply data to the player.
            player.ImportSaveData(currentSaveData);
        }

        public void DeleteSaveSlot()
        {
            saveFileManager.ConfigureSaveFileManager(currentSaveSlot);
            saveFileManager.DeleteFile();
        }

        public CharacterSaveData GetCurrentCharacterData()
        {
            saveFileManager.ConfigureSaveFileManager(currentSaveSlot);
            return saveFileManager.LoadFile();
        }

        // OTHER FUNCTIONALITY.
        public void LoadWorldScene(int sceneBuildIndex)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneBuildIndex);
            NetworkManager.Singleton.SceneManager.LoadScene(scenePath, LoadSceneMode.Single);
        }

        // TOOLS.
        public int GetWorldSceneIndex() => worldSceneIndex;

        private int GetAvailableSaveSlot()
        {
            for (int i = 1; i < characterSaveSlots + 1; ++i)
            {
                saveFileManager.ConfigureSaveFileManager((CharacterSaveSlot)i);
                if (!saveFileManager.CheckIfFileExists())
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
