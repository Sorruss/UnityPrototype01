using System.Collections;
using UnityEngine;

namespace FG
{
    public class UI_SideMenuManager : MonoBehaviour
    {
        [Header("Main Window")]
        [SerializeField] private GameObject menu;

        // --------------
        // COMMON METHODS
        public void OpenSideMenu()
        {
            PlayerUIManager.instance.popUpManager.CloseAllPopUps();
            PlayerUIManager.instance.CloseAllMenus();

            PlayerUIManager.instance.hudManager.HideHUD();

            menu.SetActive(true);
            PlayerUIManager.instance.EnableCursor(true);
            PlayerUIManager.instance.isMenuOpened = true;
        }

        public void CloseSideMenu()
        {
            PlayerUIManager.instance.hudManager.ShowHUD();

            menu.SetActive(false);
            PlayerUIManager.instance.EnableCursor(false);
        }

        public void WaitAndCloseSideMenu()
        {
            StartCoroutine(CloseAfterFixedUpdate());
        }

        private IEnumerator CloseAfterFixedUpdate()
        {
            yield return new WaitForFixedUpdate();

            CloseSideMenu();
            yield return null;
        }
    }
}
