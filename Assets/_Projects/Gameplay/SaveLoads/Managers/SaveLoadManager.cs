using Asce.Managers;
using Asce.Managers.SaveLoads;
using Asce.Managers.Utils;
using System.IO;
using UnityEngine;

namespace Asce.SaveLoads
{
    public class SaveLoadManager : DontDestroyOnLoadSingleton<SaveLoadManager>
    {
        [SerializeField] private SO_AllSaveFiles _allSaveFiles;
        [SerializeField] private SO_AllSaveControllers _allSaveControllers;

		public SO_AllSaveFiles AllSaveFiles => _allSaveFiles;
        public SaveLoadController GetController(string name) => _allSaveControllers.Get(name);

        /// <summary> Save an object to the specified save file. </summary>
        public void Save<T>(string saveName, T data)
        {
            string path = _allSaveFiles.GetPath(saveName);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError($"[SaveLoadManager] Save file '{saveName}' not found.");
                return;
            }

            SaveLoadSystem.Save(data, path);
        }

        /// <summary> Load an object from the specified save file. </summary>
        public T Load<T>(string saveName, T defaultResult = default)
        {
            string path = _allSaveFiles.GetPath(saveName);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError($"[SaveLoadManager] Save file '{saveName}' not found.");
                return defaultResult;
            }

            return SaveLoadSystem.Load<T>(path);
        }

        /// <summary> Delete save file. </summary>
        public void Delete(string saveName)
        {
            string path = _allSaveFiles.GetPath(saveName);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError($"[SaveLoadManager] Save file '{saveName}' not found.");
                return;
            }

            SaveLoadSystem.Delete(path);
        }

        /// <summary> Save an object to the specified save file. </summary>
        public void SaveIntoFolder<T>(string folder, string saveName, T data)
        {
            string path = _allSaveFiles.GetFolderPath(folder);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError($"[SaveLoadManager] Save folder '{folder}' not found.");
                return;
            }

            string saveFileName = $"{saveName.ToSnakeCase()}.json";
            SaveLoadSystem.Save(data, Path.Combine(path, saveFileName));
        }

        /// <summary> Load an object from the specified save file. </summary>
        public T LoadFromFolder<T>(string folder, string saveName, T defaultResult = default)
        {
            string path = _allSaveFiles.GetFolderPath(folder);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError($"[SaveLoadManager] Save folder '{folder}' not found.");
                return defaultResult;
            }

            string saveFileName = $"{saveName.ToSnakeCase()}.json";
            return SaveLoadSystem.Load<T>(Path.Combine(path, saveFileName));
        }
    }
}
