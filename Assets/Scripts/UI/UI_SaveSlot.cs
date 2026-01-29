using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FG
{
    public class UI_SaveSlot : MonoBehaviour, ISelectHandler, IPointerEnterHandler
    {
        [SerializeField] private TextMeshProUGUI characterNameText;
        [SerializeField] private TextMeshProUGUI timePlayedText;
        
        [HideInInspector] public CharacterSaveSlot characterSaveSlot;
        [HideInInspector] public string characterName;
        [HideInInspector] public float playTime;
        [HideInInspector] public int slot;

        private string SecondsToTime(float seconds)
        {
            return "00:00:01";
        }

        public void UpdateValues()
        {
            characterNameText.text = characterName;
            timePlayedText.text = SecondsToTime(playTime);
        }
        
        public void LoadThisSlot()
        {
            SaveGameManager.instance.currentSaveSlot = characterSaveSlot;
            SaveGameManager.instance.LoadGame();
            SaveGameManager.instance.LoadWorldScene(SaveGameManager.instance.GetWorldSceneIndex());
        }

        public void OnSelect(BaseEventData eventData)
        {
            SelectThisSlot();
        }

        private void SelectThisSlot()
        {
            LoadGameMenu.instance.currentlySelectedSlot = this;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SelectThisSlot();
        }
    }
}
