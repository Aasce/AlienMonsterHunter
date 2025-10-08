using Asce.Game.Abilities;
using Asce.Game.AIs;
using Asce.Game.FOVs;
using Asce.Game.Stats;
using Asce.Game.VFXs;
using Asce.Managers.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class ElectroReactor_Machine : Machine
    {
        [Header("References")]
        [SerializeField] private FieldOfView _fov;
        [SerializeField] private MultiTargetDetection _targetDetection;

        [Header("Attack Settings")]
        [SerializeField] private float _damage = 10f;
        [SerializeField, Min(0f)] private float _shockRadius = 2f;
        [SerializeField] private Cooldown _attackCooldown = new(1f);

        [Header("Link Settings")]
        [SerializeField, Min(0f)] private float _maxLinkDistance;
        [SerializeField, Min(0f)] private float _linkWidth;
        [SerializeField] private Cooldown _checkMachineCooldown = new(.25f);
        private readonly HashSet<Transform> _linkedMachines = new();

        [Space]
        [SerializeField] private string _linkAbilityName = string.Empty;
        [SerializeField] private string _lightningVFXName = string.Empty;

        public MultiTargetDetection TargetDetection => _targetDetection;
        public float Damage => _damage;
        public float ShockRadius => _shockRadius;
        public Cooldown AttackCooldown => _attackCooldown;

        public float MaxLinkDistance => _maxLinkDistance;

        protected override void RefReset()
        {
            base.RefReset();
            if (this.LoadComponent(out _targetDetection))
            {
                _targetDetection.Origin = transform;
                _targetDetection.ForwardReference = transform;
            }
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            _linkedMachines.Clear();
        }

        public override void Initialize()
        {
            base.Initialize();
            _damage = Information.Stats.GetCustomStat("Damage");
            _shockRadius = Information.Stats.GetCustomStat("ShockRadius");
            _maxLinkDistance = Information.Stats.GetCustomStat("MaxLinkDistance");
            _linkWidth = Information.Stats.GetCustomStat("LinkWidth");
            _attackCooldown.SetBaseTime(Information.Stats.GetCustomStat("AttackSpeed"));
            _targetDetection.ViewRadius = Information.Stats.GetCustomStat("ViewRadius");
            if (_fov != null) _fov.ViewRadius = _targetDetection.ViewRadius;
        }

        private void Update()
        {
            _targetDetection.UpdateDetection();
            this.AttackVisibleTargets();
            this.CheckMachines();
        }

        private void CheckMachines()
        {
            _checkMachineCooldown.Update(Time.deltaTime);
            if (!_checkMachineCooldown.IsComplete) return;
            _checkMachineCooldown.Reset();

            this.Linking();
        }

        private void LateUpdate()
        {
            if (_fov != null) _fov.DrawFieldOfView();
        }

        public void Linking()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, MaxLinkDistance, LayerUtils.IntToLayerMask(gameObject.layer));
            foreach (Collider2D collider in colliders)
            {
                if (!collider.enabled) continue;
                if (_linkedMachines.Contains(collider.transform)) continue;
                if (collider.transform == transform) continue;
                if (!collider.TryGetComponent(out Machine other)) continue;
                this.SpawnLink(other);
                _linkedMachines.Add(other.transform);
            }
        }

        private void SpawnLink(Machine other)
        {
            ElectroReactor_Linker_Ability linker = AbilityController.Instance.Spawn(_linkAbilityName, gameObject) as ElectroReactor_Linker_Ability;
            if (linker == null) return;
            linker.DamageDeal = Damage;
            linker.LinkWidth = _linkWidth;
            linker.MaxLinkDistance = MaxLinkDistance;
            linker.Set(this, other);
            linker.transform.position = transform.position;
            linker.gameObject.SetActive(true);
            linker.OnActive();
        }


        /// <summary>
        ///     Attacks all visible enemies detected by MultiTargetDetection.
        /// </summary>
        private void AttackVisibleTargets()
        {
            _attackCooldown.Update(Time.deltaTime);
            if (!_attackCooldown.IsComplete) return;
            _attackCooldown.Reset();

            if (_targetDetection == null) return;

            IReadOnlyList<ITargetable> targets = _targetDetection.VisibleTargets;
            if (targets.Count == 0) return;

            foreach (var target in targets)
            {
                if (target == null) continue;
                this.Attack(target);
                this.SpawnLightningVFX(target.transform.position);
            }
        }


        private void Attack(ITargetable target)
        {
            CombatController.Instance.DamageDealing(target as ITakeDamageable, Damage);
        }

        private void SpawnLightningVFX(Vector2 endPoint)
        {
            LineVFXObject vfx = VFXController.Instance.Spawn(_lightningVFXName, transform.position) as LineVFXObject;
            if (vfx == null) return;
            if (vfx.LineRenderer == null) return;

            vfx.LineRenderer.positionCount = 2;
            vfx.LineRenderer.SetPosition(0, transform.position);
            vfx.LineRenderer.SetPosition(1, endPoint);
            vfx.LineRenderer.startWidth = _linkWidth * 1.5f;
            vfx.LineRenderer.endWidth = _linkWidth * 0.75f;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, MaxLinkDistance);
        }
#endif
    }
}
