using Asce.Managers;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    [CreateAssetMenu(menuName = "Asce/Entities/All Characters", fileName = "All Characters")]
    public class SO_AllCharacters : ScriptableObject
    {
        [SerializeField] private ListObjects<string, Character> _characters = new((character) => {
            if (character == null) return null;
            if (character.Information == null) return null;
            return character.Information.Name;        
        });

        public ReadOnlyCollection<Character> Characters => _characters.List;
        public Character Get(string name) => _characters.Get(name); 
    }
}
