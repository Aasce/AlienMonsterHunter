using Asce.Game.Managers;
using Asce.Game.Supports;
using Asce.PrepareGame.Picks;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UISupportCollectionView : UICollectionView<Support>
    {
        public override IEnumerable<Support> Collection => GameManager.Instance.AllSupports.Supports;

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
