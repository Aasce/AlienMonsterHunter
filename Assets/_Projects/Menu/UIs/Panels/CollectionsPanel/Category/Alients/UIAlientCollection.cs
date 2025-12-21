using Asce.Game.Entities.Enemies;
using Asce.Game.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainMenu.UIs.Panels.Collections
{
    public class UIAlientCollection : UICollectionView<Enemy>
    {
        public override IEnumerable<Enemy> GetCollection() => GameManager.Instance.AllEnemies.Enemies;
    }
}
