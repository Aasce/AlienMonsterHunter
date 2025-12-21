using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.Game.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Asce.MainMenu.UIs.Panels.Collections
{
    public class UIGunCollection : UICollectionView<Gun>
    {
        public override IEnumerable<Gun> GetCollection()
        {
            IEnumerable<Gun> guns = GameManager.Instance.AllGuns.Guns;
            GunsProgress progress = PlayerManager.Instance.Progress.GunsProgress;
            if (progress == null) return guns;

            return guns.OrderBy(gun => !progress.Get(gun.Information.Name).IsUnlocked);
        }


    }
}
