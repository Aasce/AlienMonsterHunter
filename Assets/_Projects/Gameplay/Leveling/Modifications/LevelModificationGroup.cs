using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Levelings
{
    [Serializable]
    public class LevelModificationGroup
    {
        [SerializeField]
        private List<LevelModification> _modifications = new();
        private ReadOnlyCollection<LevelModification> _modificationsReadonly;
        private Dictionary<string, LevelModification> _modificationsDictionary;

        /// <summary>
        /// The list of modifications applied at this level (could be stat change, resource, unlock, etc.).
        /// </summary>
        public ReadOnlyCollection<LevelModification> Modifications => _modificationsReadonly ??= _modifications.AsReadOnly();

        public LevelModification GetModification(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            if (_modificationsDictionary == null) InitializeDictionary();
            return _modificationsDictionary.TryGetValue(key, out var mod) ? mod : null;
        }

        public bool TryGetModification(string key, out LevelModification modification)
        {
            modification = null;
            if (string.IsNullOrEmpty(key)) return false;
            if (_modificationsDictionary == null) InitializeDictionary();
            return _modificationsDictionary.TryGetValue(key, out modification);
        }

        private void InitializeDictionary()
        {
            _modificationsDictionary = new Dictionary<string, LevelModification>();
            foreach (var modification in _modifications)
            {
                if (modification == null || string.IsNullOrEmpty(modification.TargetKey))
                    continue;

                _modificationsDictionary[modification.TargetKey] = modification;
            }
        }

        /// <summary>
        /// Combine two LevelModificationGroups. 
        /// If duplicate keys exist, keep the modification from the first group.
        /// </summary>
        public static LevelModificationGroup operator +(LevelModificationGroup a, LevelModificationGroup b)
        {
            if (a == null && b == null) return null;
            if (a == null) return b.Clone();
            if (b == null) return a.Clone();

            LevelModificationGroup result = new LevelModificationGroup();
            HashSet<string> addedKeys = new();

            // Add from A first (takes priority)
            foreach (var mod in a._modifications)
            {
                if (mod == null || string.IsNullOrEmpty(mod.TargetKey)) continue;
                result._modifications.Add(mod.Clone());
                addedKeys.Add(mod.TargetKey);
            }

            // Add from B if not duplicate
            foreach (var mod in b._modifications)
            {
                if (mod == null || string.IsNullOrEmpty(mod.TargetKey)) continue;
                if (!addedKeys.Contains(mod.TargetKey))
                {
                    result._modifications.Add(mod.Clone());
                    addedKeys.Add(mod.TargetKey);
                }
            }

            return result;
        }

        /// <summary> Creates a deep clone of this LevelModificationGroup. </summary>
        public LevelModificationGroup Clone()
        {
            LevelModificationGroup clone = new();
            foreach (LevelModification mod in _modifications)
            {
                if (mod == null) continue;
                clone._modifications.Add(mod.Clone());
            }
            return clone;
        }
    }
}
