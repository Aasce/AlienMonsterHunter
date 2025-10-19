using Asce.Game.Supports;
using Asce.Game.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Menu.UIs.Characters
{
    public class UISupportCollection : UICollectionView<Support>
    {
        protected override IEnumerable<Support> Items => GameManager.Instance.AllSupports.Supports;
    }
}
