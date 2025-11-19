using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.SaveLoads;
using System.Collections.Generic;
namespace Asce.Game.Players
{
    public class CharactersProgress : PlayerCollectionProgress<Character, CharacterProgress>
    {
        protected override IEnumerable<Character> Collection => GameManager.Instance.AllCharacters.Characters;

        protected override CharacterProgress CreateProgressInstance(string name) => new (name);
        protected override string GetInformationName(Character item) => item == null? string.Empty : item.Information.Name;

        public void ApplyTo(Character character)
        {
            if (character == null) return;
            CharacterProgress characterProgress = Get(character.Information.Name);
            if (characterProgress == null) return;
            if (!characterProgress.IsUnlocked) return;

            character.Leveling.SetLevel(characterProgress.Level);
            character.Leveling.SetExp(characterProgress.Exp);
        }


        public void SetLevel(string name, int level, int exp)
        {
            CharacterProgress characterProgress = Get(name);
            if (characterProgress == null) return;
            if (!characterProgress.IsUnlocked) return;

            characterProgress.Level = level;
            characterProgress.Exp = exp;
            this.SaveProgress(characterProgress);
        }

        protected override void SaveProgress(CharacterProgress characterProgress)
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.SaveCharacterProgress(characterProgress);
        }

        protected override void LoadProgress(CharacterProgress characterProgress)
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.LoadCharacterProgress(characterProgress);
        }
    }
}