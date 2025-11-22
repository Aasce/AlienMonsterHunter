using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.SaveLoads;
using System.Collections.Generic;
namespace Asce.Game.Players
{
    public class CharactersProgress : PlayerCollectionProgress<Character, CharacterProgress>
    {
        protected override IEnumerable<Character> Collection => GameManager.Instance.AllCharacters.Characters;

        protected override CharacterProgress CreateProgressInstance(Character item) => new (item.Information.Name, item.Information.Progress);
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
    }
}