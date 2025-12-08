using Asce.Game.Supports;
using Asce.Game.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainMenu.UIs.Characters
{
    public class UISupportCollection : UICollectionView<Support>
    {
        protected override IEnumerable<Support> Items => GameManager.Instance.AllSupports.Supports;
    }
}
