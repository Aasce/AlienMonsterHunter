using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.PrepareGame.Picks;
using System.Collections.Generic;
using System.Linq;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UIGunCollectionView : UICollectionView<Gun>
    {
        public override IEnumerable<Gun> GetCollection()
        {
            IEnumerable<Gun> guns = GameManager.Instance.AllGuns.Guns;
            GunsProgress progress = PlayerManager.Instance.Progress.GunsProgress;
            if (progress == null) return guns;

            return guns.OrderBy(gun => !progress.Get(gun.Information.Name).IsUnlocked);
        }

        public override void ItemClick(UICollectionItem<Gun> uiItem)
        {
            base.ItemClick(uiItem);
            PickController.Instance.PickGun(uiItem.Item);
        }

    }
}
