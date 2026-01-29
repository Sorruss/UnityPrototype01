using UnityEngine;
using UnityEngine.UI;

namespace FG
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("MainMenu - Setting")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private Button mainMenuFirstButton;

        [Header("SaveSlotsMenu - Setting")]
        [SerializeField] private GameObject saveSlotsMenu;
        [SerializeField] private Button saveSlotsMenuFirstButton;

        [Header("Pop-ups")]
        [SerializeField] private GameObject noSlotsAvailablePopUp;
        [SerializeField] private Button noSlotsAvailbleFirstButton;

        public void NewGame()
        {
            if (!SaveGameManager.instance.TryCreateNewGame())
            {
                EnableNoSlotsAvailablePopUp(true);
                return;
            }
            SaveGameManager.instance.LoadWorldScene(SaveGameManager.instance.GetWorldSceneIndex());
        }

        public void StartGame()
        {
            NetworkManagerScript.instance.StartAsHost();
        }

        public void OpenMainMenu()
        {
            mainMenu.SetActive(true);
            saveSlotsMenu.SetActive(false);
            mainMenuFirstButton.Select();
        }

        public void OpenSaveSlotsMenu()
        {
            mainMenu.SetActive(false);
            saveSlotsMenu.SetActive(true);
            saveSlotsMenuFirstButton.Select();
        }

        public void EnableNoSlotsAvailablePopUp(bool enable)
        {
            noSlotsAvailablePopUp.SetActive(enable);
            if (enable)
            {
                noSlotsAvailbleFirstButton.Select();
            }
            else
            {
                mainMenuFirstButton.Select();
            }
        }
    }
}
