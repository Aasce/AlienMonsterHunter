using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.Game.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asce.MainMenu.UIs.Panels.Collections
{
    public class UICharacterCollection : UICollectionView<Character>
    {
        public override IEnumerable<Character> GetCollection()
        {
            IEnumerable<Character> characters = GameManager.Instance.AllCharacters.Characters;
            CharactersProgress progress = PlayerManager.Instance.Progress.CharactersProgress;
            if (progress == null) return characters;

            return characters.OrderBy(character => !progress.Get(character.Information.Name).IsUnlocked);
        }


    }
}
