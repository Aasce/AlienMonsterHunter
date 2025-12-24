using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.Spawners
{
    public abstract class BaseSpawner : GameComponent
    {
        [SerializeField, Readonly] protected string _name;

        [Header("References")]
        [SerializeField] protected SpawnArea _spawnArea;

        [Header("Runtime")]
        [SerializeField] protected bool _active = true;

        public string Name => _name;
        public SpawnArea SpawnArea => _spawnArea;
        public bool Active
        {
            get => _active;
            set => _active = value;
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _spawnArea);
        }

        public virtual void Initialize()
        {

        }

        public virtual void OnCreate() { }
        public virtual void OnLoad() { }

        protected virtual void Update()
        {
            if (!Active) return;
            this.OnUpdateSpawning();
        }

        /// <summary> Called every frame while spawner is active. </summary>
        protected abstract void OnUpdateSpawning();

        /// <summary> Called when the spawner needs to create new entities. </summary>
        protected abstract void Spawn();
    }
}
