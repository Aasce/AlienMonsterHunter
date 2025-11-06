using System.Collections.Generic;
using UnityEngine;

namespace Asce.Managers
{
    public static class Shared
    {
        private static readonly Dictionary<string, object> _data = new();

        public static Dictionary<string, object> Data => _data;


        public static T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning($"[Shared-Get] Key is null or empty");
                return default;
            }

            if (!Data.TryGetValue(key, out object value))
            {
                return default;
            }

            if (value is not T tValue) return default;
            return tValue;
        }

        public static void Add(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning($"[Shared-Add] Key is null or empty");
                return;
            }

            if (Data.ContainsKey(key)) return;
            

            _data.Add(key, value);
        }

        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning($"[Shared-Remove] Key is null or empty");
                return;
            }

            if (!Data.ContainsKey(key)) return;
            _data.Remove(key);
        }

        public static void Set(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning($"[Shared-Set] Key is null or empty");
                return;
            }

            if (!Data.ContainsKey(key))  return;
            Data[key] = value;
        }

        public static void SetOrAdd(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning($"[Shared-SetOrAdd] Key is null or empty");
                return;
            }

            if (Data.ContainsKey(key)) Set(key, value);
            else Add(key, value);
        }

        public static void Clear()
        {
            _data.Clear();
        }
    }
}
