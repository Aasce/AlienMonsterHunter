using Asce.Game.Abilities;
using Asce.Game.AIs;
using Asce.Game.Combats;
using Asce.Game.FOVs;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.VFXs;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Asce.Game.Entities.Machines
{
    public class ElectroReactor_Machine : Machine, IMachineAttackable
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
        [SerializeField, Min(0f)] private float _linkDamage = 5f;
        [SerializeField] private Cooldown _checkMachineCooldown = new(.25f);
        private readonly HashSet<Transform> _linkedMachines = new();

        [Space]
        [SerializeField] private string _linkAbilityName = string.Empty;

        public event Action<ITargetable> OnAttacked;


        public MultiTargetDetection TargetDetection => _targetDetection;
        public float Damage => _damage;
        public float ShockRadius => _shockRadius;
        public float LinkDamage => _linkDamage;
        public float MaxLinkDistance => _maxLinkDistance;
        public Cooldown AttackCooldown => _attackCooldown;

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
            TargetDetection.ResetTarget();
            _linkedMachines.Clear();
        }

        public override void Initialize()
        {
            base.Initialize();
            _damage = Information.Stats.GetCustomStat("Damage");
            _shockRadius = Information.Stats.GetCustomStat("ShockRadius");
            _maxLinkDistance = Information.Stats.GetCustomStat("MaxLinkDistance");
            _linkDamage = Information.Stats.GetCustomStat("LinkDamage");
            _attackCooldown.SetBaseTime(Information.Stats.GetCustomStat("AttackSpeed"));
            _targetDetection.ViewRadius = Information.Stats.GetCustomStat("ViewRadius");
            _fov.ViewRadius = _targetDetection.ViewRadius;
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _damage = Information.Stats.GetCustomStat("Damage");
            _shockRadius = Information.Stats.GetCustomStat("ShockRadius");
            _maxLinkDistance = Information.Stats.GetCustomStat("MaxLinkDistance");
            _linkDamage = Information.Stats.GetCustomStat("LinkDamage");
            _attackCooldown.SetBaseTime(Information.Stats.GetCustomStat("AttackSpeed"));
            _targetDetection.ViewRadius = Information.Stats.GetCustomStat("ViewRadius");
            _fov.ViewRadius = _targetDetection.ViewRadius;

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

            if (modificationGroup.TryGetModification("ShockRadius", out LevelModification shockRadiusModification))
            {
                _shockRadius += shockRadiusModification.Value;
            }

            if (modificationGroup.TryGetModification("LinkDamage", out LevelModification linkDamageModification))
            {
                _linkDamage += linkDamageModification.Value;
            }

            if (modificationGroup.TryGetModification("ViewRadius", out LevelModification viewRadiusModification))
            {
                _targetDetection.ViewRadius += viewRadiusModification.Value;
                _fov.ViewRadius = _targetDetection.ViewRadius;
            }
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
            _fov.DrawFieldOfView();
        }

        public void Linking()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, MaxLinkDistance, LayerUtils.IntToLayerMask(gameObject.layer));
            foreach (Collider2D collider in colliders)
            {
                if (!collider.enabled) continue;
                if (collider.transform == transform) continue;
                if (_linkedMachines.Contains(collider.transform)) continue;
                if (!collider.TryGetComponent(out Machine other)) continue;
                this.SpawnLink(other);
                _linkedMachines.Add(other.transform);
            }
        }

        private void SpawnLink(Machine other)
        {
            ElectroReactor_Linker_Ability linker = AbilityController.Instance.Spawn(_linkAbilityName, gameObject) as ElectroReactor_Linker_Ability;
            if (linker == null) return;
            linker.DamageDeal = _linkDamage;
            linker.MaxLinkDistance = MaxLinkDistance;
            linker.LinkWidth = Information.Stats.GetCustomStat("LinkWidth");
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

            IReadOnlyList<ITargetable> targets = _targetDetection.VisibleTargets;
            if (targets.Count == 0) return;

            foreach (ITargetable target in targets)
            {
                if (target == null) continue;
                this.Attack(target);
            }

            _attackCooldown.Reset();
        }

        public void Attack(ITargetable target)
        {
            CombatController.Instance.DamageDealing(new DamageContainer(this, target as ITakeDamageable)
            {
                Damage = Damage
            });
            OnAttacked?.Invoke(target);
        }

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("Damage", _damage);
            data.SetCustom("ShockRadius", _shockRadius);
            data.SetCustom("LinkDamage", _linkDamage);
            data.SetCustom("AttackCooldown", _attackCooldown.CurrentTime);
            data.SetCustom("ViewRadius", _targetDetection.ViewRadius);
        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            _damage = data.GetCustom<float>("Damage");
            _shockRadius = data.GetCustom<float>("ShockRadius");
            _linkDamage = data.GetCustom<float>("LinkDamage");
            _attackCooldown.CurrentTime = data.GetCustom<float>("AttackCooldown");
            _targetDetection.ViewRadius = data.GetCustom<float>("ViewRadius");
            _fov.ViewRadius = _targetDetection.ViewRadius;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, MaxLinkDistance);
        }
#endif
    }
}
