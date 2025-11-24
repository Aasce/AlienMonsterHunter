using Asce.Game.Combats;
using Asce.Game.Effects;
using Asce.Game.Entities;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class HellFlamethrower_Fire_Ability : Ability, ISendDamageAbility
    {
        [Header("References")]
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;
        [SerializeField] private ParticleSystem _fireVFX;

        [Space]
        [SerializeField] private LayerMask _victimLayer;
        [SerializeField, Min(0f)] private float _damage = 0f;
        [SerializeField, Min(0f)] private Cooldown _damageInterval = new(0.5f);
        [SerializeField, Min(0f)] private float _igniteDuration = 5f;
        [SerializeField, Min(0f)] private float _igniteStrength = 3f;

        [Space]
        [SerializeField, Min(0f)] private float _force = 0f;
        [SerializeField] private Vector2 _radiusRange = new (0.5f, 2f);

        [Header("Runtime")]
        [SerializeField, Readonly] private float _currentRadius = 0f;
        [SerializeField, Readonly] private Vector2 _direction;
        private ISendDamageable _ownerSender;
        private readonly Collider2D[] _overlapResults = new Collider2D[16];
        private ContactFilter2D _contactFilter;

        public event Action<DamageContainer> OnSendDamage;

        public Rigidbody2D Rigidbody => _rigidbody;
        public float Damage => _damage;

        public float CurrentRadius
        {
            get => _currentRadius;
            protected set
            {
                _currentRadius = Mathf.Clamp(value, _radiusRange.x, _radiusRange.y);
            }
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _rigidbody);
        }

        public override void Initialize()
        {
            base.Initialize();
            _igniteDuration = Information.GetCustomValue("IgniteDuration");
            _igniteStrength = Information.GetCustomValue("IgniteStrength");
            CurrentRadius = _radiusRange.x;
            this.InitializeFireVFX();
            _contactFilter = new ContactFilter2D
            {
                layerMask = _victimLayer,
                useLayerMask = true,
                useTriggers = true,
            };
        }

        private void InitializeFireVFX()
        {
            var mainModule = _fireVFX.main;
            mainModule.startSize = _currentRadius * 2f;
            mainModule.startLifetime = DespawnTime.BaseTime;

            var sizeOverLifetime = _fireVFX.sizeOverLifetime;
            sizeOverLifetime.enabled = true;

            AnimationCurve curve = new();
            curve.AddKey(0f, _radiusRange.x * 2f);
            curve.AddKey(1f, _radiusRange.y * 2f);
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);

            _fireVFX.Clear(true);
            _fireVFX.Play(true);
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            CurrentRadius = _radiusRange.x;
            _damageInterval.Reset();
        }

        public override void OnActive()
        {
            base.OnActive();
            _ownerSender = Owner.GetComponent<ISendDamageable>();

            if (Rigidbody == null) return;
            Rigidbody.AddForce(_direction * _force, ForceMode2D.Impulse);

            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _igniteDuration = Information.GetCustomValue("IgniteDuration");
            _igniteStrength = Information.GetCustomValue("IgniteStrength");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("IgniteDuration", out LevelModification igniteDurationModification))
            {
                _igniteDuration += igniteDurationModification.Value;
            }

            if (modificationGroup.TryGetModification("IgniteStrength", out LevelModification igniteStrengthModification))
            {
                _igniteStrength += igniteStrengthModification.Value;
            }
        }

        private void Update()
        {
            CurrentRadius += Time.deltaTime;
            _damageInterval.Update(Time.deltaTime);
            if (_damageInterval.IsComplete)
            {
                _damageInterval.Reset();
                this.SendDamage();
            }
        }

        private void SendDamage()
        {
            int count = Physics2D.OverlapCircle(transform.position, CurrentRadius, _contactFilter, _overlapResults);
            for (int i = 0; i < count; i++)
            {
                Collider2D collider = _overlapResults[i];
                if (!collider.enabled) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;
                SendDamage(target);
            }
        }

        private void SendDamage(ITargetable target)
        {
            DamageContainer container = new (_ownerSender, target as ITakeDamageable)
            {
                Damage = Damage
            };
            EffectController.Instance.AddEffect("Ignite", _ownerSender as Entity, target as Entity, new EffectData()
            {
                Duration = _igniteDuration,
                Strength = _igniteStrength,
            });
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
            data.SetCustom("IgniteDuration", _igniteDuration);
            data.SetCustom("IgniteStrength", _igniteStrength);
            data.SetCustom("DamageCooldown", _damageInterval.CurrentTime);
            data.SetCustom("LinearVelocity", Rigidbody.linearVelocity);
            data.SetCustom("CurrentRadius", CurrentRadius);
        }

        protected override void OnAfterLoad(AbilitySaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _igniteDuration = data.GetCustom<float>("IgniteDuration");
            _igniteStrength = data.GetCustom<float>("IgniteStrength");
            _damageInterval.CurrentTime = data.GetCustom<float>("DamageCooldown");
            Rigidbody.linearVelocity = data.GetCustom<Vector2>("LinearVelocity");
            CurrentRadius = data.GetCustom<float>("CurrentRadius");

            this.LoadVFX();
        }

        protected override IEnumerator LoadOwner(AbilitySaveData data)
        {
            yield return base.LoadOwner(data);
            _ownerSender = Owner != null ? Owner.GetComponent<ISendDamageable>() : null;
        }

        private void LoadVFX()
        {
            float progressTime = Mathf.Clamp(
                DespawnTime.BaseTime - DespawnTime.CurrentTime,
                0f,
                DespawnTime.BaseTime
            );

            if (_fireVFX != null)
            {
                _fireVFX.Simulate(progressTime, withChildren: true, restart: true, fixedTimeStep: false);
                _fireVFX.Play(true);
            }
        }
    }
}
