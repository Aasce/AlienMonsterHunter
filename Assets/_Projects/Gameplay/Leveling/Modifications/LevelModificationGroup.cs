using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Levelings
{
    [System.Serializable]
    public class LevelModificationGroup
    {
        [SerializeField]
        private List<LevelModification> _modifications = new();
        private ReadOnlyCollection<LevelModification> _modificationsReadonly;
        private Dictionary<string, LevelModification> _modificationsDictionary;
        /// <summary> The list of modifications applied at this level (could be stat change, resource, unlock, etc.). </summary>
        public ReadOnlyCollection<LevelModification> Modifications => _modificationsReadonly ??= _modifications.AsReadOnly();
        public LevelModification GetModification(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            if (_modificationsDictionary == null) this.InitializeDictionary();
            return _modificationsDictionary[key];
        }
        public bool TryGetModification(string key, out LevelModification modification)
        {
            modification = null;
            if (string.IsNullOrEmpty(key)) return false;
            if (_modificationsDictionary == null) this.InitializeDictionary();

            if (_modificationsDictionary.TryGetValue(key, out modification)) return true;
            return false;
        }

        private void InitializeDictionary()
        {
            _modificationsDictionary = new();
            foreach (LevelModification modification in _modifications)
            {
                if (modification == null) continue;
                if (modification.TargetKey == null) continue;

                _modificationsDictionary[modification.TargetKey] = modification;
            }
        }
    }

}