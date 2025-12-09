using Asce.Core.Utils;
using Asce.Game.Entities.Enemies;
using Asce.Game.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Spawners
{
    public class TimedEnemySpawner : BaseSpawner
    {
        [Header("Enemies")]
        [SerializeField] private List<string> _enemyNames = new();

        [Header("Spawn Settings")]
        [SerializeField] private Cooldown _spawnCooldown = new(5f);
        [SerializeField] private Vector2Int _levelRange = new(0, 10);

        public List<string> Enemies => _enemyNames;

        protected override void Reset()
        {
            base.Reset();
            _name = "Timed";
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override void OnLoad()
        {
            base.OnLoad();
        }

        protected override void OnUpdateSpawning()
        {
            _spawnCooldown.Update(Time.deltaTime);
            if (_spawnCooldown.IsComplete)
            {
                Spawn();
                _spawnCooldown.Reset();
            }
        }

        protected override void Spawn()
        {
            string name = Enemies[Random.Range(0, Enemies.Count)];
            Enemy enemyPrefab = GameManager.Instance.AllEnemies.Get(name);
            if (enemyPrefab == null) return;

            NavMeshQueryFilter filter = new()
            {
                agentTypeID = enemyPrefab.Agent.agentTypeID,
                areaMask = NavMesh.AllAreas
            };

            if (_spawnArea != null && _spawnArea.TryGetSpawnPosition(filter, out Vector3 spawnPos))
            {
                Enemy enemy = EnemyController.Instance.Spawn(name, spawnPos);
                if (enemy == null) return;

                enemy.Leveling.SetLevel(Random.Range(_levelRange.x, _levelRange.y + 1));
                enemy.gameObject.SetActive(true);
            }
        }

    }
}
