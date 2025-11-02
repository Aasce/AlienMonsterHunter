using Asce.Game.Entities.Enemies;
using Asce.Game.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Spawners
{
    public class InitialEnemySpawner : BaseSpawner
    {
        [Header("Spawn Settings")]
        [SerializeField] private int _spawnCount = 10;
        [SerializeField] private Vector2Int _levelRange = new(0, 10);

        protected override void Start()
        {
            base.Start();
            if (_autoStart) Spawn();
        }

        protected override void OnUpdateSpawning() { /* No update needed */ }

        protected override void Spawn()
        {
            for (int i = 0; i < _spawnCount; i++)
            {
                string name = Enemies[Random.Range(0, Enemies.Count)];
                Enemy enemyPrefab = GameManager.Instance.AllEnemies.Get(name);
                if (enemyPrefab == null) continue;

                NavMeshQueryFilter filter = new()
                {
                    agentTypeID = enemyPrefab.Agent.agentTypeID,
                    areaMask = NavMesh.AllAreas
                };

                if (_spawnArea != null && _spawnArea.TryGetSpawnPosition(filter, out Vector3 spawnPos))
                {
                    Enemy enemy = EnemyController.Instance.Spawn(name, spawnPos);
                    if (enemy == null) continue;

                    enemy.Leveling.SetLevel(Random.Range(_levelRange.x, _levelRange.y + 1));
                    enemy.gameObject.SetActive(true);
                }
            }
        }

    }
}
