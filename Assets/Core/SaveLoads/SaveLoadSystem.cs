using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Asce.Managers.SaveLoads
{
    public static class SaveLoadSystem
    {
        /// <summary>
        ///     Save object to JSON file at given path using Newtonsoft.Json.
        /// </summary>
        public static void Save<T>(T target, string path)
        {
            try
            {
                string fullPath = Path.Combine(Application.persistentDataPath, path);
                string directory = Path.GetDirectoryName(fullPath);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string json = JsonConvert.SerializeObject(target, JsonSettings.FieldOnly);
                File.WriteAllText(fullPath, json);

#if UNITY_EDITOR
                Debug.Log($"[SaveLoadSystem] Saved: {fullPath}");
#endif
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SaveLoadSystem] Save failed: {ex}");
            }
        }

        /// <summary>
        ///     Load object from JSON file at given path using Newtonsoft.Json.
        /// </summary>
        public static T Load<T>(string path)
        {
            try
            {
                string fullPath = Path.Combine(Application.persistentDataPath, path);
                if (!File.Exists(fullPath)) return default;

                string json = File.ReadAllText(fullPath);
                return JsonConvert.DeserializeObject<T>(json, JsonSettings.FieldOnly);
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
                if (File.Exists(fullPath))
                {
                    File.WriteAllText(fullPath, "{}");
#if UNITY_EDITOR
                    Debug.Log($"[SaveLoadSystem] Cleared: {fullPath}");
#endif
                }
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
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
#if UNITY_EDITOR
                    Debug.Log($"[SaveLoadSystem] Deleted: {fullPath}");
#endif
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SaveLoadSystem] Delete failed: {ex}");
            }
        }
    }
}
