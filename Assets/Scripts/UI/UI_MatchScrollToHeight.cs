using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FG
{
    public class UI_MatchScrollToHeight : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private RectTransform content;
        [SerializeField] private ScrollRect scrollRect;

        [Header("Debug")]
        [SerializeField] private GameObject currentlySelected;
        [SerializeField] private GameObject previouslySelected;

        private void Update()
        {
            if (LoadGameMenu.instance.deleteSaveSlotPopUpActive)
                return;

            currentlySelected = EventSystem.current.currentSelectedGameObject;
            if (currentlySelected == null)
                return;

            if (currentlySelected == previouslySelected)
                return;

            RectTransform currentlySelectedRect = currentlySelected.GetComponent<RectTransform>();
            previouslySelected = currentlySelected;
            SnapTo(currentlySelectedRect);
        }

        private void SnapTo(RectTransform rect)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 newPosition = (Vector2)(
                scrollRect.transform.InverseTransformPoint(content.position) 
                - scrollRect.transform.InverseTransformPoint(rect.position));

            newPosition.x = 0.0f;
            content.anchoredPosition = newPosition;
        }
    }
}
