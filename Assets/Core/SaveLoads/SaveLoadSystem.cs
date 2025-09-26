using System.IO;
using UnityEngine;

namespace Asce.Managers.SaveLoads
{
    public static class SaveLoadSystem
    {
        /// <summary>
        ///     Save object to JSON file at given path.
        /// </summary>
        public static void Save<T>(T target, string path)
        {
            try
            {
                // Combine with persistentDataPath
                string fullPath = Path.Combine(Application.persistentDataPath, path);

                // Ensure directory exists
                string directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Serialize to JSON
                string json = JsonUtility.ToJson(target, true);
                File.WriteAllText(fullPath, json); // Write file (overwrite if exists)
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SaveLoadSystem] Save failed: {ex}");
            }
        }

        /// <summary>
        ///     Load object from JSON file at given path.
        /// </summary>
        public static T Load<T>(string path)
        {
            try
            {
                string fullPath = Path.Combine(Application.persistentDataPath, path);
                if (!File.Exists(fullPath)) return default;
                
                // Read JSON text
                string json = File.ReadAllText(fullPath);
                return JsonUtility.FromJson<T>(json); // Deserialize
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SaveLoadSystem] Load failed: {ex}");
                return default;
            }
        }

        /// <summary>
        ///     Clear content of file if exists (overwrite with empty object).
        /// </summary>
        public static void Clear(string path)
        {
            try
            {
                string fullPath = Path.Combine(Application.persistentDataPath, path);
                if (File.Exists(fullPath)) File.WriteAllText(fullPath, "{}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SaveLoadSystem] Clear failed: {ex}");
            }
        }

        /// <summary>
        ///     Delete file if exists.
        /// </summary>
        public static void Delete(string path)
        {
            try
            {
                string fullPath = Path.Combine(Application.persistentDataPath, path);
                if (File.Exists(fullPath)) File.Delete(fullPath);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SaveLoadSystem] Delete failed: {ex}");
            }
        }
    }
}
