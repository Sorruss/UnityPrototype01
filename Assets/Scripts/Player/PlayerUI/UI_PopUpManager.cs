using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FG
{
    public class UI_PopUpManager : MonoBehaviour
    {
        [Header("PopUp - You Died")]
        [SerializeField] private GameObject youDiedPopUp;
        [SerializeField] private TextMeshProUGUI youDiedBackgroundText;
        [SerializeField] private TextMeshProUGUI youDiedForegroundText;
        [SerializeField] private CanvasGroup youDiedCanvasGroup;

        [Header("PopUp - Boss Defeated")]
        [SerializeField] private string bossDefeatedDefaultText = "GREAT ENEMY SLAYED.";
        [SerializeField] private GameObject bossDefeatedPopUp;
        [SerializeField] private TextMeshProUGUI bossDefeatedBackgroundText;
        [SerializeField] private TextMeshProUGUI bossDefeatedForegroundText;
        [SerializeField] private CanvasGroup bossDefeatedCanvasGroup;

        [Header("PopUp - Interactable")]
        [SerializeField] private GameObject interactablePopUp;
        [SerializeField] private TextMeshProUGUI interactableText;
        [SerializeField] private TextMeshProUGUI interactableQuantityText;

        [Header("PopUp - Item Pick Up")]
        [SerializeField] private GameObject itemPickUpPopUp;
        [SerializeField] private TextMeshProUGUI itemPickUpItemName;
        [SerializeField] private Image itemPickUpItemIcon;
        [SerializeField] private TextMeshProUGUI itemPickUpItemAmount;

        public void CloseAllPopUps()
        {
            interactablePopUp.SetActive(false);
            itemPickUpPopUp.SetActive(false);

            PlayerUIManager.instance.isPopUpOpened = false;
        }

        // ------------------
        // POP UPS ACTIVATORS
        public void SendYouDiedPopUp()
        {
            youDiedPopUp.SetActive(true);

            StartCoroutine(FadeInOverTime(youDiedCanvasGroup, 2.0f));
            StartCoroutine(StretchTextOverTime(youDiedBackgroundText, 25.0f, 6.0f));
            StartCoroutine(FadeOutOverTime(youDiedCanvasGroup, 2.0f, 5.0f));
        }

        public void SendBossDefeatedPopUp(string text = "")
        {
            if (text.Length == 0)
            {
                bossDefeatedForegroundText.text = bossDefeatedDefaultText;
                bossDefeatedBackgroundText.text = bossDefeatedDefaultText;
            }
            else
            {
                bossDefeatedForegroundText.text = text;
                bossDefeatedBackgroundText.text = text;
            }
            
            bossDefeatedPopUp.SetActive(true);

            StartCoroutine(FadeInOverTime(bossDefeatedCanvasGroup, 2.0f));
            StartCoroutine(StretchTextOverTime(bossDefeatedBackgroundText, 25.0f, 6.0f));
            StartCoroutine(FadeOutOverTime(bossDefeatedCanvasGroup, 2.0f, 5.0f));
        }

        public void SendInteractablePopUp(string text)
        {
            interactableText.text = text;
            interactablePopUp.SetActive(true);
        }

        public void SendItemPickedUpPopUp(Item item, int amount)
        {
            itemPickUpItemAmount.enabled = false;
            itemPickUpItemName.text = item.Name;
            itemPickUpItemIcon.sprite = item.Icon;

            if (amount > 0)
            {
                itemPickUpItemAmount.enabled = true;
                itemPickUpItemAmount.text = $"X{amount}";
            }

            itemPickUpPopUp.SetActive(true);
            PlayerUIManager.instance.isPopUpOpened = true;
        }

        public void UpdateInteractablePopUpQuantityText(int quantity)
        {
            if (quantity > 1)
                interactableQuantityText.text = quantity.ToString();
            else
                interactableQuantityText.text = "";
        }

        // -----------------
        // POP UPS MODIFIERS
        private IEnumerator StretchTextOverTime(TextMeshProUGUI text, float stretchValue, float duration, float delay = 0.0f)
        {
            if (delay > 0.0f)
            {
                yield return new WaitForSeconds(delay);
            }

            if (duration > 0.0f)
            {
                float timer = 0.0f;
                text.characterSpacing = 0.0f;

                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    text.characterSpacing = stretchValue * (timer / duration);
                    yield return null;
                }
            }
            else
            {
                text.characterSpacing = stretchValue;
            }
        }

        private IEnumerator FadeInOverTime(CanvasGroup canvasGroup, float duration, float delay = 0.0f)
        {
            if (delay > 0.0f)
            {
                yield return new WaitForSeconds(delay);
            }

            if (duration > 0.0f)
            {
                float timer = 0.0f;
                canvasGroup.alpha = 0.0f;

                yield return null;
                
                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = timer / duration;
                    yield return null;
                }
            }
            else
            {
                canvasGroup.alpha = 1.0f;
            }
        }

        private IEnumerator FadeOutOverTime(CanvasGroup canvasGroup, float duration, float delay = 0.0f)
        {
            if (delay > 0.0f)
            {
                yield return new WaitForSeconds(delay);
            }

            if (duration > 0.0f)
            {
                float timer = 0.0f;
                canvasGroup.alpha = 1.0f;

                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = 1 - (timer / duration);
                    yield return null;
                }
            }
            else
            {
                canvasGroup.alpha = 0.0f;
            }
        }
    }
}
