using Asce.Managers;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Spawners
{
    public abstract class BaseSpawner : GameComponent
    {
        [SerializeField] protected SpawnArea _spawnArea;

        [Header("Enemies")]
        [SerializeField] private List<string> _enemyNames = new();

        [Header("Spawner Info")]
        [SerializeField] protected bool _autoStart = true;
        [SerializeField] protected bool _active = true;

        public List<string> Enemies => _enemyNames;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _spawnArea);
        }

        protected virtual void Start()
        {
            if (_autoStart) StartSpawning();
        }

        protected virtual void Update()
        {
            if (!_active) return;
            OnUpdateSpawning();
        }

        /// <summary> Called once when spawning should begin </summary>
        public virtual void StartSpawning() => _active = true;

        /// <summary> Called once when spawning should stop. </summary>
        public virtual void StopSpawning() => _active = false;

        /// <summary> Called every frame while spawner is active. </summary>
        protected abstract void OnUpdateSpawning();

        /// <summary> Called when the spawner needs to create new entities. </summary>
        protected abstract void Spawn();
    }
}
