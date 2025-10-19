using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    [CreateAssetMenu(menuName = "Asce/Entities/Character Information", fileName = "Entity Information")]
    public class SO_CharacterInformation : SO_EntityInformation
    {
        [Header("Character")]
        [SerializeField] private CharacterRoleType _role = CharacterRoleType.Special;
        [SerializeField, Range(0, 10)] private int _difficulty = 5;

        [Space]
        [SerializeField] private List<string> _abilityNames = new();
        private ReadOnlyCollection<string> _abilityNamesReadonly;

        public CharacterRoleType Role => _role;
        public int Difficulty => _difficulty;

        public ReadOnlyCollection<string> AbilitiesName => _abilityNamesReadonly ??= _abilityNames.AsReadOnly();
    }
}