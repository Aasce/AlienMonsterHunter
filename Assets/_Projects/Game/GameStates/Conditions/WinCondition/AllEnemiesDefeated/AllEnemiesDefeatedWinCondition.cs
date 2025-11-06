using Asce.Game.Entities.Enemies;
using Asce.Managers.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainGame.Managers
{
    public class AllEnemiesDefeatedWinCondition : WinCondition
    {
        [SerializeField, Readonly] private int _enemiesCount = 0;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override bool IsSatisfied()
        {
            List<Enemy> enemies = EnemyController.Instance.GetAllEnemies();
            _enemiesCount = 0;
            foreach (Enemy enemy in enemies)
            {
                if (!enemy.gameObject.activeInHierarchy) continue;
                _enemiesCount++;
            }
            return _enemiesCount <= 0;
        }
    }
}