using Asce.Managers;
using Asce.Managers.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    public class SaveLoadManager : DontDestroyOnLoadSingleton<SaveLoadManager>
    {
        [SerializeField] private SO_AllSaveFiles _allSaveFiles;

		public SO_AllSaveFiles AllSaveFiles => _allSaveFiles;

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
        public T Load<T>(string saveName)
        {
            string path = _allSaveFiles.GetPath(saveName);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError($"[SaveLoadManager] Save file '{saveName}' not found.");
                return default;
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
    }
}
