using System;
using System.Collections.Generic;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class SaveData
    {
        public Dictionary<string, object> customs = new();

        public virtual void CopyFrom(SaveData other)
        {
            customs = new Dictionary<string, object>(other.customs);

        }

        public void SetCustom<T>(string key, T value) => customs[key] = value;
        public T GetCustom<T>(string key, T defaultValue = default)
        {
            if (customs.TryGetValue(key, out var val))
            {
                try
                {
                    return (T)Convert.ChangeType(val, typeof(T));
                }
                catch { }
            }
            return defaultValue;
        }
    }
}