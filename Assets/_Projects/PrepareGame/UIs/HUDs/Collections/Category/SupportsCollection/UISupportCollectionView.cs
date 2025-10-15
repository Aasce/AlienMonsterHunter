using Asce.Game.Managers;
using Asce.Game.Supports;
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
            List<UISupportPickedSlot> slots = UIPrepareGameController.Instance.HUDController.Picked.SupportSlots;
            int index = slots.FindIndex((uiSlot) => uiSlot.Item == uiItem.Item);
            if (index >= 0) return;
            foreach (UISupportPickedSlot slot in slots) 
            {
                if (slot.Item == null)
                {
                    slot.Set(uiItem.Item);
                    return;
                }
            }


        }
    }
}
