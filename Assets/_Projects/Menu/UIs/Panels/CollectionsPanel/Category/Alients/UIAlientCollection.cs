using Asce.Game.Entities.Enemies;
using Asce.Game.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Menu.UIs.Characters
{
    public class UIAlientCollection : UICollectionView<Enemy>
    {
        protected override IEnumerable<Enemy> Items => GameManager.Instance.AllEnemies.Enemies;
    }
}
