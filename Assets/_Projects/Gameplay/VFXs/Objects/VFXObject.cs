using Asce.Core;
using Asce.Core.Utils;
using System;
using UnityEngine;

namespace Asce.Game.VFXs
{
    public class VFXObject : GameComponent
    {
        [SerializeField] protected string _name;
        [SerializeField] protected Cooldown _despawnCooldown = new();

        public event Action OnSpawn;
        public event Action OnDespawn;

        public string Name => _name;
        public Cooldown DespawnCooldown => _despawnCooldown;

        protected virtual void Update()
        {
            DespawnCooldown.Update(Time.deltaTime);
            if (DespawnCooldown.IsComplete)
            {
                this.Despawn();
            }
        }

        public virtual void Stop() => DespawnCooldown.ToComplete();
        public virtual void Spawn() 
        {
            OnSpawn?.Invoke();
        }

        public virtual void Despawn()
        {
            OnDespawn?.Invoke();
            VFXController.Instance.Despawn(this);
        }
        public virtual void ResetStatus()
        {
            DespawnCooldown.Reset();
        }
    }
}
