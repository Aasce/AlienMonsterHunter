using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.Managers;
using Asce.SaveLoads;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Players
{
    public class CharactersProgress : GameComponent
    {
        [SerializeField] private List<CharacterProgress> _charactersProgress = new();
        private Dictionary<string, CharacterProgress> _charactersProgressDictionary;
        private ReadOnlyCollection<CharacterProgress> _charactersProgressReadonly;

        public ReadOnlyCollection<CharacterProgress> AllProgresses => _charactersProgressReadonly ??= _charactersProgress.AsReadOnly();
        public CharacterProgress Get(string name)
        {
            if (_charactersProgressDictionary == null) this.Initialize();
            if (_charactersProgressDictionary.TryGetValue(name, out CharacterProgress progress))
            {
                return progress;
            }
            return null;
        }

        public void ApplyTo(Character character)
        {
            if (character == null) return;
            CharacterProgress characterProgress = Get(character.Information.Name);
            if (characterProgress == null) return;
            if (!characterProgress.IsUnlocked) return;

            character.Leveling.SetLevel(characterProgress.Level);
            character.Leveling.SetExp(characterProgress.Exp);
        }

        public void Unlock(string name)
        {
            CharacterProgress characterProgress = Get(name);
            if (characterProgress == null) return;
            if (characterProgress.IsUnlocked) return;

            characterProgress.IsUnlocked = true;

            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.SaveCharacterProgress(characterProgress);
        }

        public void SetLevel(string name, int level, int exp)
        {
            CharacterProgress characterProgress = Get(name);
            if (characterProgress == null) return;
            if (!characterProgress.IsUnlocked) return;

            characterProgress.Level = level;
            characterProgress.Exp = exp;

            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.SaveCharacterProgress(characterProgress);
        }

        private void Initialize()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;
            
            IEnumerable<Character> characters = GameManager.Instance.AllCharacters.Characters;
            foreach (Character character in characters)
            {
                CharacterProgress characterProgress = new(character.Information.Name);
                playerProgressController.LoadCharacterProgress(characterProgress);
                _charactersProgress.Add(characterProgress);
            }

            _charactersProgressDictionary = new Dictionary<string, CharacterProgress>();
            foreach (var characterProgress in _charactersProgress)
            {
                _charactersProgressDictionary[characterProgress.Name] = characterProgress;
            }
        }
    }
}