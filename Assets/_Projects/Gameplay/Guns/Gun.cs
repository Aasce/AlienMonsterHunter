using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Guns
{
    public abstract class Gun : GameComponent
    {
        [SerializeField] protected Transform _barrel;

        [Space]
        [SerializeField, Min(0f)] protected float _damage = 10f;
        [SerializeField] private Cooldown _shootCooldown = new(0.5f);

        public Vector2 BarrelPosition => _barrel != null ? _barrel.position : transform.position;
        public float Damage => _damage;
        public Cooldown ShootCooldown => _shootCooldown;

        protected virtual void Update()
        {
            ShootCooldown.Update(Time.deltaTime);
        }

        public void Shoot(Vector2 direction)
        {
            if (ShootCooldown.IsComplete)
            {
                this.Shooting(direction);
                ShootCooldown.Reset();
            }
        }

        protected abstract void Shooting(Vector2 direction);
    }
}
