using Asce.Game.Managers;
using Asce.Game.Maps;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainMenu.UIs.Maps
{
    public class UIMapCollection : UICollectionView<Map>
    {
        protected override IEnumerable<Map> Items => GameManager.Instance.AllMaps.Maps;
    }
}
