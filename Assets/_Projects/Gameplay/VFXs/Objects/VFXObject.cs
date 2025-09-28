using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.VFXs
{
    public class VFXObject : GameComponent
    {
        [SerializeField] protected string _name;
        [SerializeField] protected Cooldown _despawnCooldown = new();

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
        public virtual void Despawn() => VFXController.Instance.Despawn(this);
        public virtual void ResetStatus()
        {
            DespawnCooldown.Reset();
        }
    }
}
