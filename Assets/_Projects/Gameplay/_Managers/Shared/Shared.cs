using System.Collections.Generic;
using UnityEngine;

namespace Asce.Core
{
    public class Shared
    {
        private readonly Dictionary<string, object> _data = new();

        public Dictionary<string, object> Data => _data;


        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) return default;
            if (!Data.TryGetValue(key, out object value)) return default;
            if (value is not T tValue) return default;
            return tValue;
        }


        public bool TryGet<T> (string key, out T value)
        {
            value = default;
            if (string.IsNullOrEmpty(key)) return false;
            if (!Data.TryGetValue(key, out object objValue)) return false;
            if (objValue is not T tValue) return false;
            value = tValue;
            return true;
        }


        public void Add(string key, object value)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (Data.ContainsKey(key)) return;

            Data.Add(key, value);
        }

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (!Data.ContainsKey(key)) return;
            Data.Remove(key);
        }

        public void Set(string key, object value)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (!Data.ContainsKey(key)) return;
            Data[key] = value;
        }

        public void SetOrAdd(string key, object value)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (Data.ContainsKey(key)) this.Set(key, value);
            else this.Add(key, value);
        }

        public void Clear()
        {
            Data.Clear();
        }
    }
}
