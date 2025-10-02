using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Managers
{
    [System.Serializable]
    public class ListObjects<TKey, TValue> 
    {
        [SerializeField] private List<TValue> _list = new();
        private ReadOnlyCollection<TValue> _listReadonly;
        private Dictionary<TKey, TValue> _listDictionary;
        private Func<TValue, TKey> _getKeyFunc;

        public ReadOnlyCollection<TValue> List => _listReadonly ??= _list.AsReadOnly();
        public Func<TValue, TKey> GetKeyFunc
        {
            get => _getKeyFunc;
            set => _getKeyFunc = value;
        }

        public ListObjects(Func<TValue, TKey> getKeyFunc) 
        {
            GetKeyFunc = getKeyFunc;
        }


        public TValue Get(TKey key)
        {
            if (_listDictionary == null) this.InitializeDictionary();
            _listDictionary.TryGetValue(key, out var value);
            return value;
        }

        private void InitializeDictionary()
        {
            if (_getKeyFunc == null)
            {
                Debug.LogError("Get Key Func is null");
                return;
            }

            _listDictionary = new Dictionary<TKey, TValue>();
            foreach (TValue item in _list)
            {
                TKey key = _getKeyFunc.Invoke(item);
                if (key == null) continue;
                if (_listDictionary.ContainsKey(key))
                {
                    Debug.LogWarning($"Key {key} is contained in dictionary");
                    continue;
                }

                _listDictionary[key] = item;
            }
        }
    }
}
