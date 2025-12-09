using Asce.Core.Attributes;
using Asce.Game.Entities.Enemies;
using Asce.Game.Managers;
using Asce.Game.SaveLoads;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainGame.Managers
{
    public class AllEnemiesDefeatedWinCondition : GameStateCondition
    {
        [SerializeField, Readonly] private int _enemiesCount = 0;

        public override string ConditionName => "All Enemies Defeated";

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnCheck()
        {
            base.OnCheck();
            List<Enemy> enemies = EnemyController.Instance.GetAllEnemies();
            _enemiesCount = 0;
            foreach (Enemy enemy in enemies)
            {
                if (!enemy.gameObject.activeInHierarchy) continue;
                _enemiesCount++;
            }
        }

        public override bool IsSatisfied()
        {
            return _enemiesCount <= 0;
        }


        protected override void OnBeforeSave(GameStateConditionSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("EnemiesCount", _enemiesCount);
        }

        protected override void OnAfterLoad(GameStateConditionSaveData data)
        {
            base.OnAfterLoad(data);
            _enemiesCount = data.GetCustom<int>("EnemiesCount");
        }
    }
}