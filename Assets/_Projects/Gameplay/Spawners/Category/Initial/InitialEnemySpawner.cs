using Asce.Game.Entities.Enemies;
using Asce.Game.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Spawners
{
    public class InitialEnemySpawner : BaseSpawner
    {
        [Header("Enemies")]
        [SerializeField] private List<EnemySpawnContainer> _enemies = new();

        public List<EnemySpawnContainer> Enemies => _enemies;

        protected override void Reset()
        {
            base.Reset();
            _name = "Initial";
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnCreate()
        {
            base.OnCreate();
            this.Spawn();
        }

        public override void OnLoad()
        {
            base.OnLoad();
        }

        protected override void OnUpdateSpawning() { }

        protected override void Spawn()
        {
            if (_spawnArea == null) return;
            foreach (EnemySpawnContainer container in _enemies)
            {
                if (container == null) continue;
                Enemy enemyPrefab = GameManager.Instance.AllEnemies.Get(container.EnemyName);
                if (enemyPrefab == null) continue;
                NavMeshQueryFilter filter = new()
                {
                    agentTypeID = enemyPrefab.Agent.agentTypeID,
                    areaMask = NavMesh.AllAreas
                };

                this.SpawnEnemies(container, filter);
            }
        }

        private void SpawnEnemies(EnemySpawnContainer container, NavMeshQueryFilter filter)
        {
            for (int i = 0; i < container.Quantity; i++)
            {
                if (!_spawnArea.TryGetSpawnPosition(filter, out Vector3 spawnPos))
                    continue;

                Enemy enemy = EnemyController.Instance.Spawn(container.EnemyName, spawnPos);
                if (enemy == null) continue;

                enemy.Leveling.SetLevel(Random.Range(container.LevelRange.x, container.LevelRange.y + 1));
                enemy.gameObject.SetActive(true);
            }
        }
    }
}
