using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Entities
{
    [CreateAssetMenu(menuName = "Asce/Entities/Character Information", fileName = "Entity Information")]
    public class SO_CharacterInformation : SO_EntityInformation
    {
        [Header("Character")]
        [SerializeField] private List<string> _abilityNames = new();
        private ReadOnlyCollection<string> _abilityNamesReadonly;

        public ReadOnlyCollection<string> AbilitiesName => _abilityNamesReadonly ??= _abilityNames.AsReadOnly();
    }
}