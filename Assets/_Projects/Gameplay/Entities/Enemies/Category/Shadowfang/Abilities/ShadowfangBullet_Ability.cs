using Asce.Game.Combats;
using Asce.Game.Effects;
using Asce.Game.Entities;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class ShadowfangBullet_Ability : Ability, ISendDamageAbility
    {
        [Header("References")]
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;

        [Header("Toxic Effects")]
        [SerializeField, Readonly] private float _toxicDuration = 0f;
        [SerializeField, Readonly] private float _toxicStrength = 0f;

        [Space]
        [SerializeField] private LayerMask _layer;
        [SerializeField, Min(0f)] private float _force = 0f;

        [Header("Runtime")]
        [SerializeField, Readonly] private bool _isDealing = false;
        [SerializeField, Readonly] private float _damage = 0f;
        [SerializeField, Readonly] private Vector2 _direction;

        public event Action<DamageContainer> OnSendDamage;

        public Rigidbody2D Rigidbody => _rigidbody;
        public float Damage => _damage;

        public override void Initialize()
        {
            base.Initialize();
            _toxicDuration = Information.GetCustomValue("ToxicDuration");
            _toxicStrength = Information.GetCustomValue("ToxicStrength");
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
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

        public override void OnDespawn()
        {
            base.OnDespawn();
            Rigidbody.linearVelocity = Vector2.zero;
            Rigidbody.angularVelocity = 0f;
            Rigidbody.gravityScale = 0f;
            _direction = Vector2.zero;
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _toxicDuration = Information.GetCustomValue("ToxicDuration");
            _toxicStrength = Information.GetCustomValue("ToxicStrength");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("ToxicDuration", out LevelModification toxicDurationModification))
            {
                _toxicDuration += toxicDurationModification.Value;
            }

            if (modificationGroup.TryGetModification("ToxicStrength", out LevelModification toxicStrengthModification))
            {
                _toxicStrength += toxicStrengthModification.Value;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isDealing) return;
            if (!LayerUtils.IsInLayerMask(collision.gameObject.layer, _layer)) return;
            if (!collision.TryGetComponent(out ITargetable target)) return;
            if (!target.IsTargetable) return;
            this.SendDamage(target);

            _isDealing = true;
            this.DespawnTime.ToComplete();
        }

        private void SendDamage(ITargetable target)
        {
            ISendDamageable owner = Owner != null ? Owner.GetComponent<ISendDamageable>() : null;
            EffectController.Instance.AddEffect("Toxic", Owner.GetComponent<Entity>(), target as Entity, new EffectData()
            {
                Duration = _toxicDuration,
                Strength = _toxicStrength,
            });

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

        protected override void OnBeforeSave(AbilitySaveData data)
        {
            base.OnBeforeSave(data);

            data.SetCustom("ToxicDuration", _toxicDuration);
            data.SetCustom("ToxicStrength", _toxicStrength);
            data.SetCustom("LinearVelocity", Rigidbody.linearVelocity);
        }

        protected override void OnAfterLoad(AbilitySaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _toxicDuration = data.GetCustom<float>("ToxicDuration");
            _toxicStrength = data.GetCustom<float>("ToxicStrength");
            Rigidbody.linearVelocity = data.GetCustom<Vector2>("LinearVelocity");
        }
    }
}