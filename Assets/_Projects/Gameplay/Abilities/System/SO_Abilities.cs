using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Abilities
{
    [CreateAssetMenu(menuName = "Asce/Abilities/Abilities Data", fileName = "Abilities Data")]
    public class SO_Abilities : ScriptableObject
    {
        [SerializeField] private List<Ability> _abilities = new();
        private ReadOnlyCollection<Ability> _abilitiesReadonly;
        private Dictionary<string, Ability> _abilitiesDictionary;

        public ReadOnlyCollection<Ability> Abilities => _abilitiesReadonly ??= _abilities.AsReadOnly();
        public Ability Get(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (_abilitiesDictionary == null) InitializeDictionary();
            return _abilitiesDictionary.TryGetValue(name, out Ability ability) ? ability : null;
        }

        private void InitializeDictionary()
        {
            _abilitiesDictionary = new Dictionary<string, Ability>();
            foreach (Ability ability in _abilities)
            {
                if (ability == null) continue;
                if (_abilitiesDictionary.ContainsKey(ability.Name)) continue;

                _abilitiesDictionary.Add(ability.Name, ability);
            }
        }
    }
}