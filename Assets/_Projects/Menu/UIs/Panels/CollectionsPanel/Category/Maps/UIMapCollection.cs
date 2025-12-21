using Asce.Game.Managers;
using Asce.Game.Maps;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainMenu.UIs.Panels.Collections
{
    public class UIMapCollection : UICollectionView<Map>
    {
        public override IEnumerable<Map> GetCollection() => GameManager.Instance.AllMaps.Maps;
    }
}
