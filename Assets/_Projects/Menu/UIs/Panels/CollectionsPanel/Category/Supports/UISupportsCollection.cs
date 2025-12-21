using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Game.Supports;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asce.MainMenu.UIs.Panels.Collections
{
    public class UISupportCollection : UICollectionView<Support>
    {
        public override IEnumerable<Support> GetCollection()
        {
            IEnumerable<Support> supports = GameManager.Instance.AllSupports.Supports;
            SupportsProgress progress = PlayerManager.Instance.Progress.SupportsProgress;
            if (progress == null) return supports;

            return supports.OrderBy(support => !progress.Get(support.Information.Name).IsUnlocked);
        }
    }
}
