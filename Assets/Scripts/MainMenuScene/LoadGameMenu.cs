using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FG
{
    public class LoadGameMenu : MonoBehaviour
    {
        [SerializeField] GameObject saveSlotPrefub;
        [SerializeField] GameObject content;
        [Space]
        [SerializeField] GameObject deleteSaveSlotPopUp;
        [SerializeField] Button deleteSaveSlotButtonOk;
        [SerializeField] Button deleteSaveSlotButtonBack;
        [SerializeField] Button returnToMainMenuButton;

        [HideInInspector] public UI_SaveSlot currentlySelectedSlot;
        private CharacterSaveData[] characterSaveDatas;
        private GameObject[] characterSaveSlots;
        private SaveFileManager saveFileManager;

        private PlayerInput playerInput;

        [HideInInspector] public bool deleteSaveSlotPopUpActive = false;

        [HideInInspector] public static LoadGameMenu instance;

        private void Start()
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
            characterSaveDatas = new CharacterSaveData[SaveGameManager.instance.characterSaveSlots];
            characterSaveSlots = new GameObject[characterSaveDatas.Length];

            LoadSaveSlots();
            PopulateContent();
        }

        private void OnEnable()
        {
            if (playerInput == null)
            {
                playerInput = new PlayerInput();
            }

            playerInput.Enable();
            playerInput.UI.Backspace.performed += OnBackspace;
        }

        private void OnDisable()
        {
            playerInput.UI.Backspace.performed -= OnBackspace;
            playerInput.Disable();
        }

        private void OnBackspace(InputAction.CallbackContext obj)
        {
            if (currentlySelectedSlot == null)
            {
                return;
            }

            deleteSaveSlotPopUp.SetActive(true);
            deleteSaveSlotPopUpActive = true;
            deleteSaveSlotButtonBack.Select();
        }

        public void DeleteSaveSlot()
        {
            // Delete functionality.
            saveFileManager.ConfigureSaveFileManager((CharacterSaveSlot)currentlySelectedSlot.slot);
            saveFileManager.DeleteFile();

            // Delete UI functionality.
            Destroy(characterSaveSlots[currentlySelectedSlot.slot]);

            // UI functionality.
            deleteSaveSlotPopUp.SetActive(false);
            deleteSaveSlotPopUpActive = false;
            returnToMainMenuButton.Select();
        }

        public void HideDeleteSaveSlotPopUp()
        {
            deleteSaveSlotPopUp.SetActive(false);
            deleteSaveSlotPopUpActive = false;
            currentlySelectedSlot.GetComponent<Button>().Select();
        }

        private void PopulateContent()
        {
            int i = 1;
            float slotHeight = 0.0f;
            foreach (CharacterSaveData saveData in characterSaveDatas)
            {
                if (saveData == null)
                {
                    ++i;
                    continue;
                }

                // Placing new slot UI in content UI.
                GameObject saveSlot = Instantiate(saveSlotPrefub);
                characterSaveSlots[i] = saveSlot;
                saveSlot.transform.SetParent(content.transform, false);
                if (slotHeight == 0.0f)
                {
                    slotHeight = saveSlot.GetComponent<RectTransform>().rect.height;
                }

                // Configuring new slot UI.
                UI_SaveSlot script = saveSlot.GetComponent<UI_SaveSlot>();
                script.characterSaveSlot = (CharacterSaveSlot)(i);
                script.characterName = saveData.characterName;
                script.playTime = saveData.secondsPlayed;
                script.UpdateValues();

                // Additional setting for searching for slot later.
                script.slot = i;
                ++i;
            }

            // Adjusting content UI to hold new slot UI.
            float rectBottom = slotHeight * i;
            content.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, -rectBottom);
        }

        public void ClearCurrentSelectedSlot()
        {
            currentlySelectedSlot = null;
        }

        private void LoadSaveSlots()
        {
            for (int i = 0; i < SaveGameManager.instance.characterSaveSlots; ++i)
            {
                saveFileManager.ConfigureSaveFileManager((CharacterSaveSlot)(i + 1));
                if (saveFileManager.CheckIfFileExists())
                {
                    characterSaveDatas[i] = saveFileManager.LoadFile();
                }
            }
        }
    }
}
