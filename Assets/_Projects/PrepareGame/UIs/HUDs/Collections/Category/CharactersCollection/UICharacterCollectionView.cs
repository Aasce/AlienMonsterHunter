using Asce.Game.Entities;
using Asce.Game.Managers;
using System.Collections.Generic;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UICharacterCollectionView : UICollectionView<Character>
    {
        public override IEnumerable<Character> Collection => GameManager.Instance.AllCharacters.Characters;

    }
}
