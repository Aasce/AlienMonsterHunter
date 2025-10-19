using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Menu.UIs.Characters
{
    public class UICharacterCollection : UICollectionView<Character>
    {
        protected override IEnumerable<Character> Items => GameManager.Instance.AllCharacters.Characters;
    }
}
