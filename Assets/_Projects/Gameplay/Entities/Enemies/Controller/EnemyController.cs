using Asce.Game.Managers;
using Asce.Managers;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Entities.Enemies
{
    public class EnemyController : MonoBehaviourSingleton<EnemyController>
    {
        [Space]
        [SerializeField] private List<string> _enemyNames = new();
        [SerializeField] private Cooldown _spawnCooldown = new(5f);
        private readonly Dictionary<string, Pool<Enemy>> _pools = new();

        [Space]
        [SerializeField] private Vector2 _spawnAreaX = new(-37.5f, 37.5f);
        [SerializeField] private Vector2 _spawnAreaY = new(-25f, 25f);
        [SerializeField] private int _maxAttempts = 10;
        [SerializeField] private float _samplePositionMaxDistance = 0.5f;
        [SerializeField] private Vector2Int _levelRange = new(0, 11);

        public List<string> Enemies => _enemyNames;


        private void Update()
        {
            _spawnCooldown.Update(Time.deltaTime);
            if (_spawnCooldown.IsComplete)
            {
                int randomIndex = Random.Range(0, _enemyNames.Count);
                string name = _enemyNames[randomIndex];

                Enemy enemyPrefab = GameManager.Instance.AllEnemies.Get(name);
                NavMeshQueryFilter filter = new()
                {
                    agentTypeID = enemyPrefab.Agent.agentTypeID,
                    areaMask = NavMesh.AllAreas
                };

                for (int i = 0; i < _maxAttempts; i++)
                {
                    float randomX = Random.Range(_spawnAreaX.x, _spawnAreaX.y);
                    float randomY = Random.Range(_spawnAreaY.x, _spawnAreaY.y);
                    Vector3 candidate = new Vector3(randomX, randomY, 0f);
                    
                    if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, _samplePositionMaxDistance, filter))
                    {
                        Debug.DrawLine(candidate, hit.position, Color.red, 10f);
                        Enemy enemy = this.Spawn(name, hit.position);
                        if (enemy == null) break;
                        enemy.Leveling.SetLevel(Random.Range(_levelRange.x, _levelRange.y));
                        enemy.gameObject.SetActive(true);

                        break;
                    }
                }
                _spawnCooldown.Reset();
            }
        }

        public List<Enemy> GetAllEnemies()
        {
            List<Enemy> enemies = new();
            var pools = _pools.Values;
            foreach (var pool in pools)
            {
                enemies.AddRange(pool.Activities);
            }
            return enemies;
        }

        public Enemy Spawn(string name, Vector2 position)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (!_pools.ContainsKey(name)) this.CreatePool(name);
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
            Enemy enemyPrefab = GameManager.Instance.AllEnemies.Get(name);
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
