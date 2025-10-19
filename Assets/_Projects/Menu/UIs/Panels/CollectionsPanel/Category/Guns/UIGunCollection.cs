using Asce.Game.Guns;
using Asce.Game.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Menu.UIs.Characters
{
    public class UIGunCollection : UICollectionView<Gun>
    {
        protected override IEnumerable<Gun> Items => GameManager.Instance.AllGuns.Guns;
    }
}
