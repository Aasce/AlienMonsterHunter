using Asce.Game.VFXs;
using Asce.Managers;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Entities.Enemies
{
    public class EnemyController : MonoBehaviourSingleton<EnemyController>
    {
        [SerializeField] private SO_Enemies _enemies;

        [Space]
        [SerializeField] private List<string> _enemyNames = new();
        [SerializeField] private Cooldown _spawnCooldown = new(5f);
        private readonly Dictionary<string, Pool<Enemy>> _pools = new();

        [Space]
        [SerializeField] private Vector2 _spawnAreaX = new(-37.5f, 37.5f);
        [SerializeField] private Vector2 _spawnAreaY = new(-25f, 25f);
        [SerializeField] private int _maxAttempts = 10;
        [SerializeField] private float _samplePositionMaxDistance = 0.5f;

        public List<string> Enemies => _enemyNames;


        private void Update()
        {
            _spawnCooldown.Update(Time.deltaTime);
            if (_spawnCooldown.IsComplete)
            {
                int randomIndex = Random.Range(0, _enemyNames.Count);
                string name = _enemyNames[randomIndex];

                for (int i = 0; i < _maxAttempts; i++)
                {
                    float randomX = Random.Range(_spawnAreaX.x, _spawnAreaX.y);
                    float randomY = Random.Range(_spawnAreaY.x, _spawnAreaY.y);
                    Vector3 candidate = new Vector3(randomX, randomY, 0f);

                    if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, _samplePositionMaxDistance, NavMesh.AllAreas))
                    {
                        Debug.DrawLine(candidate, hit.position, Color.red, 10f);
                        this.Spawn(name, hit.position);
                        break;
                    }
                }
                _spawnCooldown.Reset();
            }
        }

        public void Spawn(string name, Vector2 position)
        {
            if (string.IsNullOrEmpty(name)) return;
            if (_enemies == null) return;
            if (!_pools.ContainsKey(name)) this.CreatePool(name);
            if (!_pools.TryGetValue(name, out Pool<Enemy> pool)) return;
                        
            Enemy enemy = pool.Activate(out bool isCreated);
            if (enemy == null) return;
            enemy.Agent.Warp(position);

            if (isCreated)
            {

            }
            else
            {
                enemy.ResetStatus();
                enemy.gameObject.SetActive(true);
            }
        }

        public void Despawn(Enemy enemy)
        {
            if (enemy == null) return;
            if (enemy.Information == null) return;
            if (_pools.TryGetValue(enemy.Information.Name, out Pool<Enemy> pool))
            {
                pool.Deactivate(enemy);
                enemy.gameObject.SetActive(false);
                return;
            }
            GameObject.Destroy(enemy);
        }

        private void CreatePool(string name)
        {
            if (_enemies == null) return;

            Enemy enemyPrefab = _enemies.Get(name);
            if (enemyPrefab == null) return;

            GameObject poolParent = new GameObject($"{name} Pool");
            poolParent.transform.SetParent(this.transform);

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
