using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.PrepareGame.Picks;
using System.Collections.Generic;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UIGunCollectionView : UICollectionView<Gun>
    {
        public override IEnumerable<Gun> Collection => GameManager.Instance.AllGuns.Guns;

        public override void ItemClick(UICollectionItem<Gun> uiItem)
        {
            base.ItemClick(uiItem);
            PickController.Instance.PickGun(uiItem.Item);
            //if (UIPrepareGameController.Instance.HUDController.Picked.GunSlot == null) return;
            //UIPrepareGameController.Instance.HUDController.Picked.GunSlot.Set(uiItem.Item);
        }

    }
}
