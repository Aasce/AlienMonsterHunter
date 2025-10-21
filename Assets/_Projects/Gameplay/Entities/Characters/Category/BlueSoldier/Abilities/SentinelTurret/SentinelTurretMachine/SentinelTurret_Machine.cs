using Asce.Game.AIs;
using Asce.Game.Effects;
using Asce.Game.FOVs;
using Asce.Game.SaveLoads;
using Asce.Game.Stats;
using Asce.Game.VFXs;
using Asce.Managers.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Asce.Game.Entities.Machines
{
    public class SentinelTurret_Machine : Machine
    {

        [Header("Detection")]
        [SerializeField] private FieldOfView _fov;

        [Space]
        [SerializeField] private Transform _weapon;
        [SerializeField] private Transform _barriel;

        [Space]
        [SerializeField] private SingleTargetDetection _targetDetection;

        [Header("Attack")]
        [SerializeField] private float _damage = 10f;
        [SerializeField] private Cooldown _attackCooldown = new(1f);

        [Header("VFXs")]
        [SerializeField] private string _bulletLineVFXName;

        public SingleTargetDetection TargetDetection => _targetDetection;
        public float Damage => _damage;
        public Cooldown AttackCooldown => _attackCooldown;

        protected override void RefReset()
        {
            base.RefReset();
            if (this.LoadComponent(out _targetDetection))
            {
                _targetDetection.Origin = transform;
                _targetDetection.ForwardReference = _weapon;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            _damage = Information.Stats.GetCustomStat("Damage");
            _attackCooldown.SetBaseTime(Information.Stats.GetCustomStat("AttackSpeed"));
            _targetDetection.ViewRadius = Information.Stats.GetCustomStat("ViewRadius");
            if (_fov != null) _fov.ViewRadius = _targetDetection.ViewRadius;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            TargetDetection.ResetTarget();
        }

        private void Update()
        {
            _targetDetection.UpdateDetection();
            this.AttackTargetHandle();
        }


        private void LateUpdate()
        {
            if (_fov != null)
            {
                _fov.DrawFieldOfView();
            }
        }


        private void AttackTargetHandle()
        {
            _attackCooldown.Update(Time.deltaTime);
            if (_attackCooldown.IsComplete)
            {
                _attackCooldown.Reset();
                ITargetable target = _targetDetection.CurrentTarget;
                if (target == null) return;
                Vector2 direction = target.transform.position - transform.position;

                if (_weapon != null)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                    _weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }

                float viewRadius = _targetDetection.ViewRadius;
                LayerMask seeLayer = _targetDetection.SeeLayer;
                Vector2 shootPosition = _barriel != null ? _barriel.transform.position : transform.position;
                RaycastHit2D hit = Physics2D.Raycast(shootPosition, direction.normalized, viewRadius, seeLayer);
                this.SpawnVFX(shootPosition, hit.collider != null ? hit.point : shootPosition + direction.normalized * viewRadius);

                if (hit.collider == null) return;
                if (hit.transform != target.transform) return;

                CombatController.Instance.DamageDealing(target as ITakeDamageable, _damage);
                EffectController.Instance.AddEffect("Slow", target as Entity, new EffectData()
                {
                    Duration = 3f,
                    Strength = 0.4f,
                });
            }
        }

        private void SpawnVFX(Vector2 startPoint, Vector2 endPoint)
        {
            LineVFXObject line = VFXController.Instance.Spawn(_bulletLineVFXName, startPoint) as LineVFXObject;
            if (line == null) return;
            if (line.LineRenderer == null) return;

            line.LineRenderer.positionCount = 2;
            line.LineRenderer.SetPosition(0, startPoint);
            line.LineRenderer.SetPosition(1, endPoint);
        }

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("AttackCooldown", _attackCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            _attackCooldown.CurrentTime = data.GetCustom<float>("AttackCooldown");
        }
    }
}
