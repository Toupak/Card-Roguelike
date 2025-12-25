using System.IO;
using UnityEngine;

namespace Save_System
{
    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem instance;

        private string folderName = "/Save/";
        private string folderPath => Application.persistentDataPath + folderName;
        
        private string fileName = "SaveFile_1";
        private string filePath => folderPath + fileName;

        private bool isNewSave;
        public bool IsNewSave => isNewSave;

        private SaveData saveData;
        public SaveData SaveData => saveData;
        
        private void Awake()
        {
            instance = this;

            SetupSaveFolder();
            isNewSave = !CheckForSaveFile();

            if (isNewSave)
                CreateNewSave();
            else
                LoadSave();
        }
        
        private void SetupSaveFolder()
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }

        private bool CheckForSaveFile()
        {
            return File.Exists(filePath);
        }

        private void CreateNewSave()
        {
            Debug.Log("Create New Save");
            saveData = new SaveData();
            Save();
        }
        
        private void LoadSave()
        {
            Debug.Log("Load Save");
            if (CheckForSaveFile())
            {
                string json = File.ReadAllText(filePath);
                saveData = JsonUtility.FromJson<SaveData>(json);
            }
        }

        public void Save()
        {
            Debug.Log("Save");
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(filePath, json);
        }
    }
}
