using System;
using System.Collections.Generic;

namespace Asce.SaveLoads
{
    [System.Serializable]
    public class SaveDataWithCustoms : SaveData
    {
        public Dictionary<string, object> customs = new();

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is SaveDataWithCustoms customsData)
            {
                customs = new Dictionary<string, object>(customsData.customs);
            }
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