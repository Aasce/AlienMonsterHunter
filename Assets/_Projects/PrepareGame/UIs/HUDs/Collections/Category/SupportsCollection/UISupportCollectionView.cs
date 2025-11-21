using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Game.Supports;
using Asce.PrepareGame.Picks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UISupportCollectionView : UICollectionView<Support>
    {
        public override IEnumerable<Support> GetCollection()
        {
            IEnumerable<Support> supports = GameManager.Instance.AllSupports.Supports;
            SupportsProgress progress = PlayerManager.Instance.Progress.SupportsProgress;
            if (progress == null) return supports;

            return supports.OrderBy(support => !progress.Get(support.Information.Name).IsUnlocked);
        }

        public override void ItemClick(UICollectionItem<Support> uiItem)
        {
            base.ItemClick(uiItem);
            int index = PickController.Instance.SupportPrefabs.FindIndex((support) => support == uiItem.Item);
            if (index >= 0) return; // Already picked

            for (int i = 0; i < PickController.Instance.MaxSupport; i++)
            {
                if (i >= PickController.Instance.SupportPrefabs.Count) // Out of range, pick here
                {
                    PickController.Instance.PickSupport(i, uiItem.Item);
                    return;
                }

                if (PickController.Instance.SupportPrefabs[i] == null) // Empty slot
                {
                    PickController.Instance.PickSupport(i, uiItem.Item);
                    return;
                }
            }

        }
    }
}
