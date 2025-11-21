using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.PrepareGame.Picks;
using System.Collections.Generic;
using System.Linq;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UICharacterCollectionView : UICollectionView<Character>
    {
        public override IEnumerable<Character> GetCollection()
        {
            IEnumerable<Character> characters = GameManager.Instance.AllCharacters.Characters;
            CharactersProgress progress = PlayerManager.Instance.Progress.CharactersProgress;
            if (progress == null) return characters;

            return characters.OrderBy(character => !progress.Get(character.Information.Name).IsUnlocked);
        }


        public override void ItemClick(UICollectionItem<Character> uiItem)
        {
            base.ItemClick(uiItem);
            PickController.Instance.PickCharacter(uiItem.Item);
        }

    }
}
