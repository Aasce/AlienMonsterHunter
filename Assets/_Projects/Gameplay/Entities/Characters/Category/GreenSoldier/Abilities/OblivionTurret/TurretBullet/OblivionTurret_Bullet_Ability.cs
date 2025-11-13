using Asce.Game.Combats;
using Asce.Game.Entities;
using Asce.Game.Entities.Characters;
using Asce.Game.Entities.Machines;
using Asce.Game.SaveLoads;
using Asce.Game.VFXs;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class OblivionTurret_Bullet_Ability : Ability
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        [Space]
        [SerializeField] private LayerMask _victimLayer;
        [SerializeField, Min(0f)] private float _damageDeal = 0f;
        [SerializeField, Min(0f)] private float _explosionRadius = 2f;
        [SerializeField] private bool _isDealing = false;

        [Space]
        [SerializeField] private LayerMask _collideLayer;
        [SerializeField, Min(0f)] private float _force = 0f;

        [Space]
        [SerializeField] private string _explosionVFXName = string.Empty;

        public Rigidbody2D Rigidbody => _rigidbody;
        public float DamageDeal
        {
            get => _damageDeal;
            set => _damageDeal = value;
        }
        public float ExplosionRadius
        {
            get => _explosionRadius;
            set => _explosionRadius = Mathf.Max(0f, value);
        }

        public bool IsDealing
        {
            get => _isDealing;
            set => _isDealing = value;
        }
        public float Force
        {
            get => _force;
            set => _force = value;
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _rigidbody);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (this.IsDealing) return;
            if (!LayerUtils.IsInLayerMask(collision.gameObject.layer, _collideLayer)) return;
            if (collision.TryGetComponent(out ITargetable targetable))
                if (!targetable.IsTargetable) return;

            ISendDamageable ownerSender = Owner.GetComponent<ISendDamageable>();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius, _victimLayer);
            foreach (Collider2D collider in colliders)
            {
                if (!collider.enabled) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;

                float damage = DamageDeal;
                if (target is Character) damage *= 0.25f;
                else if (target is Machine) damage *= 0.5f;

                CombatController.Instance.DamageDealing(new DamageContainer(ownerSender, target as ITakeDamageable)
                {
                    Damage = damage
                });
            }

            this.SpawnVFX();
            this.IsDealing = true;
            this.DespawnTime.ToComplete();
        }
        public override void OnSpawn()
        {
            base.OnSpawn();
            IsDealing = false;
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            Rigidbody.linearVelocity = Vector2.zero;
            Rigidbody.angularVelocity = 0f;
            Rigidbody.gravityScale = 0f;
        }

        public void Fire(Vector2 position, Vector2 direction)
        {
            if (Rigidbody == null) return;
            transform.position = position;
            Rigidbody.AddForce(direction.normalized * Force, ForceMode2D.Impulse);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void SpawnVFX()
        {
            VFXController.Instance.Spawn(_explosionVFXName, transform.position);
        }

        protected override void OnBeforeSave(AbilitySaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("LinearVelocity", Rigidbody.linearVelocity);
        }

        protected override void OnAfterLoad(AbilitySaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            Rigidbody.linearVelocity = data.GetCustom<Vector2>("LinearVelocity");
        }
    }
}
