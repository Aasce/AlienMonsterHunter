using Asce.Game.Abilities;
using Asce.Game.Combats;
using Asce.Game.SaveLoads;
using Asce.Game.Stats;
using Asce.Game.VFXs;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class Veylar_Enemy : Enemy
    {
        [Header("Veylar")]
        [SerializeField] private CircleCollider2D _collider;

        [Space]
        [SerializeField] private string _veylarEggsAbilityName = string.Empty;
        [SerializeField] private Cooldown _maturationCooldown = new(5f);
        [SerializeField] private Vector2 _sizeRange = Vector2.one;

        [Header("Phase")]
        [SerializeField] private int _maxPhase = 3;
        [SerializeField] private int _currentPhase = 0;

        [Header("Lay")]
        [SerializeField] private int _layOnDeadAtPhase = 2;
        [SerializeField] private bool _layable = true;
        [SerializeField] private Cooldown _layCooldown = new(5f);

        [Header("VFXs")]
        [SerializeField] private string _explosionVFXName = string.Empty;

        public bool Layable
        {
            get => _layable;
            set => _layable = value;
        }

        public override void Initialize()
        {
            base.Initialize();
            Agent.stoppingDistance = Stats.AttackRange.FinalValue * 0.8f;
            Stats.AttackRange.OnFinalValueChanged += (oldValue, newValue) =>
            {
                Agent.stoppingDistance = newValue;
            };

            OnDead += Veylar_OnDead;
            _maturationCooldown.ToComplete();
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            _currentPhase = 0;
            _layable = true;
            _maturationCooldown.ToComplete();
            _layCooldown.Reset();
        }

        protected override void Update()
        {
            base.Update();
            _maturationCooldown.Update(Time.deltaTime);
            if (_maturationCooldown.IsComplete)
            {
                _maturationCooldown.Reset();
                int maxPhase = Layable ? _maxPhase : _maxPhase - 2;
                if (_currentPhase < maxPhase)
                {
                    _currentPhase++;
                    this.SetSize();
                }
            }

            if (!Layable) return;
            if (_currentPhase < _maxPhase) return;
            _layCooldown.Update(Time.deltaTime);
            if (_layCooldown.IsComplete)
            {
                _layCooldown.Reset();
                this.SpawnEggs(1);
            }
        }

        protected override void MoveToTaget() => this.DefaultMoveToTaget();

        protected override void Attack()
        {
            ITargetable target = TargetDetection.CurrentTarget;
            float damage = Stats.AttackDamage.FinalValue;
            CombatController.Instance.DamageDealing(new DamageContainer(this, target as ITakeDamageable)
            {
                Damage = damage
            });
        }

        private void Explosion()
        {
            float explosionDamage = Information.Stats.GetCustomStat("ExplosionDamage");
            float explosionRadius = Information.Stats.GetCustomStat("ExplosionRadius");
            float radius = explosionRadius + _currentPhase * .5f;
            float finalExplosionDamage = explosionDamage * _currentPhase;

            LayerMask targetLayer = TargetDetection.TargetLayer;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, radius, targetLayer);
            foreach (Collider2D collider in colliders)
            {
                if (!collider.enabled) continue;
                if (collider.transform == null) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;

                CombatController.Instance.DamageDealing(new DamageContainer(this, target as ITakeDamageable)
                {
                    Damage = finalExplosionDamage
                });
            }

            this.SpawnExplosionVFX(radius);
        }

        private void SetSize()
        {
            float t = (_currentPhase - 1) / (float)(_maxPhase - 1);
            float size = Mathf.Lerp(_sizeRange.x, _sizeRange.y, t);
            _collider.radius = size;
            Agent.radius = size * 0.5f;
            View.RootTransform.localScale = Vector3.one * size;

            float multiPerPhase = Information.Stats.GetCustomStat("MultiPerPhase");
            Stats.Health.Add(multiPerPhase, StatValueType.Ratio);
            Stats.Armor.Add(multiPerPhase, StatValueType.Ratio);
            Stats.AttackDamage.Add(multiPerPhase, StatValueType.Ratio);
        }

        private void SpawnEggs(int eggCount)
        {
            VeylarEggs_Abiliity eggs = AbilityController.Instance.Spawn(_veylarEggsAbilityName, gameObject) as VeylarEggs_Abiliity;
            if (eggs == null) return;

            eggs.EggCount = eggCount;
            eggs.transform.position = transform.position;
            eggs.gameObject.SetActive(true);
            eggs.OnActive();
        }

        private void SpawnExplosionVFX(float radius)
        {
            VeylarExplosionSplatterVFXObject vfx = VFXController.Instance.Spawn(_explosionVFXName, transform.position) as VeylarExplosionSplatterVFXObject;
            if (vfx == null) return;
            vfx.SetSize(radius);
        }

        private void Veylar_OnDead()
        {
            this.Explosion();
            
            if (!Layable) return;
            if (_currentPhase < _layOnDeadAtPhase) return;
            int eggCount = (int)Information.Stats.GetCustomStat("EggCount");
            eggCount += (_currentPhase - _layOnDeadAtPhase);
            this.SpawnEggs(eggCount);
        }

        protected override void OnBeforeSave(EnemySaveData data)
        {
            data.SetCustom("CurrentPhase", _currentPhase);
            data.SetCustom("Layable", _layable);
            data.SetCustom("MaturationCooldown", _maturationCooldown.CurrentTime);
            data.SetCustom("LayCooldown", _layCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(EnemySaveData data)
        {
            _currentPhase = data.GetCustom("CurrentPhase", 1);
            _layable = data.GetCustom("Layable", false);
            _maturationCooldown.CurrentTime = data.GetCustom("MaturationCooldown", 0f);
            _layCooldown.CurrentTime = data.GetCustom("LayCooldown", 0f);
            this.SetSize();
        }
    }
}
