using Asce.Managers;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class EnemyController : MonoBehaviourSingleton<EnemyController>
    {

        [SerializeField] private List<Enemy> _enemies = new();
        [SerializeField] private Cooldown _spawnCooldown = new(5f);
        private Dictionary<int, Pool<Enemy>> _pools = new();

        [Space]
        [SerializeField] private Vector2 _spawnAreaX = new(-37.5f, 37.5f);
        [SerializeField] private Vector2 _spawnAreaY = new(-25f, 25f);

        public List<Enemy> Enemies => _enemies;


        private void Update()
        {
            _spawnCooldown.Update(Time.deltaTime);
            if (_spawnCooldown.IsComplete)
            {
                this.SpawnEnemy();
                _spawnCooldown.Reset();
            }
        }

        private void SpawnEnemy()
        {
            int randomIndex = Random.Range(0, _enemies.Count);
            Enemy enemyPrefab = _enemies[randomIndex];

            if (!_pools.ContainsKey(randomIndex))
            {
                _pools[randomIndex] = new Pool<Enemy>()
                {
                    Prefab = enemyPrefab,
                    Parent = this.transform,
                    IsSetActive = false
                };
            }

            float randomX = Random.Range(_spawnAreaX.x, _spawnAreaX.y);
            float randomY = Random.Range(_spawnAreaY.x, _spawnAreaY.y);

            Enemy enemyInstance = _pools[randomIndex].Activate(out bool isCreated);
            if (enemyInstance == null) return;

            enemyInstance.transform.SetParent(this.transform);
            enemyInstance.transform.position = new Vector3(randomX, randomY, 0f);

            if (isCreated)
            {
                //enemyInstance.Health.OnCurrentValueChanged += (float oldValue, float newValue) =>
                //{
                //    if (newValue <= 0f)
                //    {
                //        enemyInstance.NetworkObject.Despawn();
                //        _pools[randomIndex].Deactivate(enemyInstance);
                //    }
                //};
            }
            else
            {
                enemyInstance.gameObject.SetActive(true);
            }

        }
    }
}
