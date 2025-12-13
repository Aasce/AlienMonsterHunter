using Asce.Game.Combats;
using Asce.Game.Entities;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.VFXs;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class LineFireLauncher_Missile_Ability : Ability, ISendDamageAbility
    {
        [Header("References")]
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;

        [Space]
        [SerializeField] private LayerMask _victimLayer;
        [SerializeField] private LayerMask _collideLayer;
        [SerializeField, Min(0f)] private float _force = 0f;

        [Space]
        [SerializeField] private string _explosionVFXName = string.Empty;

        [Header("Runtime")]
        [SerializeField, Readonly] private bool _isDealing = false;
        [SerializeField, Readonly] private float _damage = 0f;
        [SerializeField, Readonly] private float _explosionRadius = 2f;
        [SerializeField, Readonly] private Vector2 _direction;


        public event Action<DamageContainer> OnSendDamage;


        public Rigidbody2D Rigidbody => _rigidbody;
        public float Damage => _damage;
        public float ExplosionRadius => _explosionRadius;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _rigidbody);
        }

        public override void Initialize()
        {
            base.Initialize();
            _explosionRadius = Information.GetCustomValue("ExplosionRadius");
        }

        public override void Spawn()
        {
            base.Spawn();
            _isDealing = false;
        }

        public override void OnActive()
        {
            base.OnActive();
            if (Rigidbody == null) return;
            Rigidbody.AddForce(_direction * _force, ForceMode2D.Impulse);

            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public override void Despawn()
        {
            base.Despawn();
            Rigidbody.linearVelocity = Vector2.zero;
            Rigidbody.angularVelocity = 0f;
            Rigidbody.gravityScale = 0f;
            _direction = Vector2.zero;
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _explosionRadius = Information.GetCustomValue("ExplosionRadius");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("ExplosionRadius", out LevelModification explosionRadiusModification))
            {
                _explosionRadius += explosionRadiusModification.Value;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isDealing) return;
            if (!LayerUtils.IsInLayerMask(collision.gameObject.layer, _collideLayer)) return;
            if (collision.TryGetComponent(out ITargetable targetable))
                if (!targetable.IsTargetable) return;

            this.Explosion();
            this.SpawnVFX();

            _isDealing = true;
            this.DespawnTime.ToComplete();
        }

        private void Explosion()
        {
            ISendDamageable owner = Owner != null ? Owner.GetComponent<ISendDamageable>() : null;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius, _victimLayer);
            foreach (Collider2D collider in colliders)
            {
                if (!collider.enabled) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;

                this.SendDamage(owner, target);
            }
        }

        private void SendDamage(ISendDamageable owner, ITargetable target)
        {
            DamageContainer container = new(owner, target as ITakeDamageable)
            {
                Damage = Damage
            };
            CombatController.Instance.DamageDealing(container);
            OnSendDamage?.Invoke(container);
        }

        public void Set(float damage, Vector2 position, Vector2 direction)
        {
            _damage = damage;
            transform.position = position;
            _direction = direction.normalized;
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
