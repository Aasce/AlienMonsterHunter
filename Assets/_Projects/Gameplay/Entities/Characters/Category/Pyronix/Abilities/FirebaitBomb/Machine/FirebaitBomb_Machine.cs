using Asce.Game.AIs;
using Asce.Game.Combats;
using Asce.Game.Effects;
using Asce.Game.FOVs;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.VFXs;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class FirebaitBomb_Machine : Machine
    {
        [Header("References")]
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;
        [SerializeField, Readonly] private FieldOfView _fov;
        [SerializeField, Readonly] private MultiTargetDetection _targetDetection;

        [Header("Explosion Settings")]
        [SerializeField, Readonly] private float _explosionDamage = 10f;
        [SerializeField, Readonly] private Cooldown _explosionCooldown = new(5f);
        [SerializeField, Min(0f)] private float _igniteDuration = 5f;
        [SerializeField, Min(0f)] private float _igniteStrength = 3f;
        [SerializeField] private string _explosionVFXName = "";

        [Header("Taunt Settings")]
        [SerializeField, Readonly] private Cooldown _tauntCooldown = new(.2f);

        [Header("Runtime")]
        [SerializeField, Readonly] private Vector2 _moveDirection;
        [SerializeField, Readonly] private bool _isStopMoving = false;

        public Rigidbody2D Rigidbody => _rigidbody;
        public MultiTargetDetection TargetDetection => _targetDetection;
        public Cooldown ExplosionCooldown => _explosionCooldown;
        public Vector2 MoveDirection
        {
            get => _moveDirection;
            set => _moveDirection = value.normalized;
        }
        public bool IsStopMoving => _isStopMoving;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _rigidbody);
            this.LoadComponent(out _fov);
            this.LoadComponent(out _targetDetection);

        }

        public override void Initialize()
        {
            base.Initialize();
            _explosionDamage = Information.Stats.GetCustomStat("ExplosionDamage");
            _igniteDuration = Information.Stats.GetCustomStat("IgniteDuration");
            _igniteStrength = Information.Stats.GetCustomStat("IgniteStrength");
            TargetDetection.ViewRadius = Information.Stats.GetCustomStat("ViewRadius");
            _fov.ViewRadius = TargetDetection.ViewRadius;

            OnDead += FirebaitBomb_Machine_OnDead;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            TargetDetection.ResetTarget();
            _explosionCooldown.Reset();
            _isStopMoving = false;
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _explosionDamage = Information.Stats.GetCustomStat("ExplosionDamage");
            _igniteDuration = Information.Stats.GetCustomStat("IgniteDuration");
            _igniteStrength = Information.Stats.GetCustomStat("IgniteStrength");
            TargetDetection.ViewRadius = Information.Stats.GetCustomStat("ViewRadius");
            _fov.ViewRadius = _targetDetection.ViewRadius;

            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);

            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("ExplosionDamage", out LevelModification explosionDamageModification))
            {
                _explosionDamage += explosionDamageModification.Value;
            }

            if (modificationGroup.TryGetModification("ViewRadius", out LevelModification viewRadiusModification))
            {
                TargetDetection.ViewRadius += viewRadiusModification.Value;
                _fov.ViewRadius = _targetDetection.ViewRadius;
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

        private void Update()
        {
            TargetDetection.UpdateDetection();
            this.TauntHandle();
            this.ExplosionHandle();
        }

        private void FixedUpdate()
        {
            this.MoveHandle();
        }

        private void LateUpdate()
        {
            _fov.DrawFieldOfView();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!LayerUtils.IsInLayerMask(collision, TargetDetection.ObstacleLayer)) return;
            _isStopMoving = true;    
        }

        private void MoveHandle()
        {
            if (_isStopMoving) return;
            if (Effects.Unmoveable.IsAffect) return;

            float speed = Stats.Speed.FinalValue;
            if (speed <= 0f) return;

            // Move forward
            Vector2 currentPosition = _rigidbody.position;
            Vector2 nextPosition = currentPosition + _moveDirection * speed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(nextPosition);

            // Rotate drone to face move direction
            if (_moveDirection.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg - 90f;
                _rigidbody.MoveRotation(angle);
            }
        }

        private void TauntHandle()
        {
            _tauntCooldown.Update(Time.deltaTime);
            if (_tauntCooldown.IsComplete)
            {
                _tauntCooldown.Reset();
                foreach (ITargetable target in TargetDetection.VisibleTargets)
                {
                    if (target is not Enemies.Enemy enemy) continue;
                    enemy.TargetDetection.CurrentTarget = this;
                }
            }
        }

        private void ExplosionHandle()
        {
            _explosionCooldown.Update(Time.deltaTime);
            if (_explosionCooldown.IsComplete)
            {
                CombatController.Instance.Killing(this, victim: this);
            }
        }

        private void Explosion()
        {
            foreach (ITargetable target in TargetDetection.VisibleTargets)
            {
                CombatController.Instance.DamageDealing(new DamageContainer(this, target as ITakeDamageable)
                {
                    Damage = _explosionDamage
                });
                EffectController.Instance.AddEffect("Ignite", this, target as Entity, new EffectData()
                {
                    Duration = _igniteDuration,
                    Strength = _igniteStrength,
                });
            }
            this.SpawnVFX();
        }

        private void SpawnVFX()
        {
            VFXController.Instance.Spawn(_explosionVFXName, transform.position);
        }

        private void FirebaitBomb_Machine_OnDead(DamageContainer container)
        {
            this.Explosion();
        }

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("ExplosionDamage", _explosionDamage);
            data.SetCustom("ExplosionCooldown", _explosionCooldown.CurrentTime);
            data.SetCustom("IgniteDuration", _igniteDuration);
            data.SetCustom("IgniteStrength", _igniteStrength);

            data.SetCustom("ViewRadius", TargetDetection.ViewRadius);
        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            _explosionDamage = data.GetCustom<float>("ExplosionDamage");
            _explosionCooldown.CurrentTime = data.GetCustom<float>("ExplosionCooldown");
            _igniteDuration = data.GetCustom<float>("IgniteDuration");
            _igniteStrength = data.GetCustom<float>("IgniteStrength");

            TargetDetection.ViewRadius = data.GetCustom<int>("ViewRadius");
            _fov.ViewRadius = _targetDetection.ViewRadius;
        }
    }
}
