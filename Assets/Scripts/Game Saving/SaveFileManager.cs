//#define SHOW_DEBUG

using System;
using System.IO;
using UnityEngine;


namespace FG
{
    public class SaveFileManager
    {
        public string filePath = "";
        public string fileName = "";

        public bool CheckIfFileExists()
        {
            return File.Exists(Path.Combine(filePath, fileName));
        }

        private string GetSaveFileNameOfCurrentSlot(CharacterSaveSlot slot)
        {
            string fileName = "characterSaveSlot_";
            fileName += slot.ToString();
            return fileName;
        }

        public void ConfigureSaveFileManager(CharacterSaveSlot slot)
        {
            filePath = Application.persistentDataPath;
            fileName = GetSaveFileNameOfCurrentSlot(slot);
        }

        public void SaveFile(CharacterSaveData saveData)
        {
            string savePath = Path.Combine(filePath, fileName);
#pragma warning disable CS0168
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(savePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                }
                string dataToSave = JsonUtility.ToJson(saveData, true);
                using (FileStream stream = new(savePath, FileMode.Create))
                {
                    using (StreamWriter writer = new(stream))
                    {
                        writer.Write(dataToSave);
                    }
                }
#if SHOW_DEBUG
                Debug.Log($"Save file was created successfully at the location {savePath}");
#endif
            }
            catch (Exception ex)
            {
#if SHOW_DEBUG
                Debug.LogError($"An error occured while trying to save the character save data with path {savePath}.\n[ERROR] {ex}");
#endif
            }
#pragma warning restore CS0168
        }

        public CharacterSaveData LoadFile()
        {
            string loadPath = Path.Combine(filePath, fileName);
            CharacterSaveData loadedData = new();
#pragma warning disable CS0168
            try
            {
                if (CheckIfFileExists())
                {
                    using (FileStream stream = new(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new(stream))
                        {
                            string data = reader.ReadToEnd();
                            loadedData = JsonUtility.FromJson<CharacterSaveData>(data);

                            //Debug.Log(data);
                        }
                    }
#if SHOW_DEBUG
                    Debug.Log($"Save file was loaded successfully.");
#endif
                }
                else
                {
#if SHOW_DEBUG
                    Debug.LogError($"Save file doesn't exist.");
#endif
                }
            }
            catch (Exception ex)
            {
#if SHOW_DEBUG
                Debug.LogError($"An error occured while trying to load the character save data with path {loadPath}.\n[ERROR] {ex}");
#endif
            }
#pragma warning restore CS0168

            return loadedData;
        }

        public void DeleteFile()
        {
            string file = Path.Combine(filePath, fileName);
            File.Delete(file);
#if SHOW_DEBUG
            Debug.Log($"Save file was deleted successfully {file}.");
#endif
        }
    }
}
