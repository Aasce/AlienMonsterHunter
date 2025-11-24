using Asce.Game.Combats;
using Asce.Game.Effects;
using Asce.Game.Entities;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.VFXs;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Asce.Game.Abilities
{
    public class ThermalContagion_Ability : CharacterAbility, ISendDamageAbility
    {
        [Header("References")]
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;
        [SerializeField, Readonly] private Vector2 _moveDirection;

        [SerializeField] private LayerMask _targetLayer;
        [SerializeField, Min(0f)] private float _force = 10f;

        [Header("Contagion")]
        [SerializeField] private float _damage = 3f;
        [SerializeField] private float _contagionRadius = 3f;
        [SerializeField, Min(0f)] private float _igniteDuration = 5f;
        [SerializeField, Min(0f)] private float _igniteStrength = 3f;

        [SerializeField] private string _igniteEffectName = "Ignite";
        [SerializeField] private string _contagionVFXName = "Thermal Contagion Contagion";

        [Header("Runtime")]
        [SerializeField] private bool _isDealing = false;

        public event Action<DamageContainer> OnSendDamage;

        public Rigidbody2D Rigidbody => _rigidbody;
        public float Damage => _damage;
        public bool IsDealing
        {
            get => _isDealing;
            set => _isDealing = value;
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out  _rigidbody);
        }

        public override void Initialize()
        {
            base.Initialize();
            _damage = Information.GetCustomValue("Damage");
            _contagionRadius = Information.GetCustomValue("ContagionRadius");
            _igniteDuration = Information.GetCustomValue("IgniteDuration");
            _igniteStrength = Information.GetCustomValue("IgniteStrength");
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            IsDealing = false;
        }

        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);
            if (_owner == null) return;

            Vector2 direction = position - (Vector2)_owner.transform.position;
            _moveDirection = direction;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public override void OnActive()
        {
            base.OnActive();
            _rigidbody.AddForce(_moveDirection.normalized * _force, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (this.IsDealing) return;
            if (!LayerUtils.IsInLayerMask(collision.gameObject.layer, _targetLayer)) return;
            if (!collision.TryGetComponent(out ITargetable target)) return;
            if (!target.IsTargetable) return;

            if (target is Entity targetEntity)
            {
                Entity ownerEntity = Owner != null ? Owner.GetComponent<Entity>() : null;
                if (targetEntity.Effects.Contains(_igniteEffectName))
                {
                    this.Contagion(target);
                }

                this.SendDamage(ownerEntity, targetEntity);
            }

            this.IsDealing = true;
            this.DespawnTime.ToComplete();
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _damage = Information.GetCustomValue("Damage");
            _contagionRadius = Information.GetCustomValue("ContagionRadius");
            _igniteDuration = Information.GetCustomValue("IgniteDuration");
            _igniteStrength = Information.GetCustomValue("IgniteStrength");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("Damage", out LevelModification damageModification))
            {
                _damage += damageModification.Value;
            }

            if (modificationGroup.TryGetModification("ContagionRadius", out LevelModification contagionRadiusModification))
            {
                _contagionRadius += contagionRadiusModification.Value;
            }

            if (modificationGroup.TryGetModification("IgniteDuration", out LevelModification igniteDurationModification))
            {
                _igniteDuration += igniteDurationModification.Value;
            }

            if (modificationGroup.TryGetModification("IgniteStrength", out LevelModification igniteStrengthModification))
            {
                _igniteStrength += igniteStrengthModification.Value;
            }
        }

        private void Contagion(ITargetable collideTarget)
        {
            Entity ownerEntity = Owner != null ? Owner.GetComponent<Entity>() : null;
            Vector2 position = collideTarget.transform.position;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, _contagionRadius, _targetLayer);
            foreach (Collider2D collider in colliders)
            {
                if (!collider.enabled) continue;
                if (collider.transform == collideTarget.transform) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;

                Entity targetEntity = target as Entity;
                if (targetEntity == null) continue;

                this.SpawnVFX(position, target, () => this.SendDamage(ownerEntity, targetEntity));
            }
        }

        private void SendDamage(Entity ownerEntity, Entity targetEntity)
        {
            float damageScale = targetEntity.Effects.Contains(_igniteEffectName) ? 1.5f : 1f; 
            EffectController.Instance.AddEffect(_igniteEffectName, ownerEntity, targetEntity, new EffectData()
            {
                Duration = _igniteDuration,
                Strength = _igniteStrength,
            });
            DamageContainer container = new(ownerEntity, targetEntity)
            {
                Damage = _damage * damageScale
            };
            CombatController.Instance.DamageDealing(container);
            OnSendDamage?.Invoke(container);
        }

        private void SpawnVFX(Vector2 position, ITargetable target, Action onVFXReachingDestination)
        {
            VFXObject vfx = VFXController.Instance.Spawn(_contagionVFXName, position);
            if (vfx == null) return;
            if (!vfx.TryGetComponent(out MoveToDestinationVFXObject moveToDestination)) return;

            moveToDestination.StartMoveToDestination(position, target.transform);
            moveToDestination.OnReachingDestination += onVFXReachingDestination;
        }

        protected override void OnBeforeSave(AbilitySaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("MoveDirection", _moveDirection);
            data.SetCustom("LinearVelocity", _rigidbody.linearVelocity);

            data.SetCustom("Damage", _damage);
            data.SetCustom("ContagionRadius", _contagionRadius);
            data.SetCustom("IgniteDuration", _igniteDuration);
            data.SetCustom("IgniteStrength", _igniteStrength);
        }

        protected override void OnAfterLoad(AbilitySaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _moveDirection = data.GetCustom<Vector2>("MoveDirection");
            _rigidbody.linearVelocity = data.GetCustom<Vector2>("LinearVelocity");

            _damage = data.GetCustom<float>("Damage");
            _contagionRadius = data.GetCustom<float>("ContagionRadius");
            _igniteDuration = data.GetCustom<float>("IgniteDuration");
            _igniteStrength = data.GetCustom<float>("IgniteStrength");
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            float radius = Application.isPlaying ? _contagionRadius : Information.GetCustomValue("ContagionRadius");
            Gizmos.DrawWireSphere(transform.position, radius);
        }
#endif
    }
}
