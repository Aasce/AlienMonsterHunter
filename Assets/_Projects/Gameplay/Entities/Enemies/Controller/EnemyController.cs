using Asce.Game.Managers;
using Asce.Managers;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class EnemyController : MonoBehaviourSingleton<EnemyController>
    {
        private readonly Dictionary<string, Pool<Enemy>> _pools = new();


        public List<Enemy> GetAllEnemies()
        {
            List<Enemy> enemies = new();
            foreach (var pool in _pools.Values)
                enemies.AddRange(pool.Activities);
            return enemies;
        }

        public Enemy Spawn(string name, Vector2 position)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (!_pools.ContainsKey(name)) CreatePool(name);
            if (!_pools.TryGetValue(name, out Pool<Enemy> pool)) return null;

            Enemy enemy = pool.Activate(out bool isCreated);
            if (enemy == null) return null;

            enemy.Agent.Warp(position);
            if (isCreated) enemy.Initialize();
            else enemy.ResetStatus();

            return enemy;
        }

        public void Despawn(Enemy enemy)
        {
            if (enemy == null || enemy.Information == null) return;

            if (_pools.TryGetValue(enemy.Information.Name, out Pool<Enemy> pool))
            {
                pool.Deactivate(enemy);
                enemy.gameObject.SetActive(false);
            }
            else
            {
                Destroy(enemy.gameObject);
            }
        }

        private void CreatePool(string name)
        {
            Enemy enemyPrefab = GameManager.Instance.AllEnemies.Get(name);
            if (enemyPrefab == null) return;

            GameObject poolParent = new($"{name} Pool");
            poolParent.transform.SetParent(transform);

            Pool<Enemy> pool = new()
            {
                Prefab = enemyPrefab,
                Parent = poolParent.transform,
                IsSetActive = false
            };

            _pools[name] = pool;
        }
    }
}
