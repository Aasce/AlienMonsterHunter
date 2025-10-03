using Asce.Game.Guns;
using Asce.Game.Managers;
using System.Collections.Generic;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UIGunCollectionView : UICollectionView<Gun>
    {
        public override IEnumerable<Gun> Collection => GameManager.Instance.AllGuns.Guns;

        public override void ItemClick(UICollectionItem<Gun> uiItem)
        {
            base.ItemClick(uiItem);
            if (UIPrepareGameController.Instance.HUD.Picked.GunSlot == null) return;
            UIPrepareGameController.Instance.HUD.Picked.GunSlot.Set(uiItem.Item);
        }

    }
}
