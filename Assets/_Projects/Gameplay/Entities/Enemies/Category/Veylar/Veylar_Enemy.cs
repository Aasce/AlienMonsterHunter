using Asce.Game.Abilities;
using Asce.Game.Combats;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.Stats;
using Asce.Game.VFXs;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class Veylar_Enemy : Enemy
    {
        [Space]
        [SerializeField] private string _veylarEggsAbilityName = string.Empty;

        [Header("Phase")]
        [SerializeField, Readonly] private Cooldown _maturationCooldown = new(5f);
        [SerializeField] private Vector2 _sizeRange = Vector2.one;
        [SerializeField] private int _maxPhase = 3;
        [SerializeField, Readonly] private int _currentPhase = 0;

        [Header("Explosion")]
        [SerializeField, Readonly] private float _explosionDamage;

        [Header("Lay")]
        [SerializeField] private int _layOnDeadAtPhase = 2;
        [SerializeField, Readonly] private bool _layable = true;
        [SerializeField, Readonly] private Cooldown _layCooldown = new(5f);

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
            _layCooldown.SetBaseTime(Information.Stats.GetCustomStat("LayCooldown"));
            _maturationCooldown.SetBaseTime(Information.Stats.GetCustomStat("MaturationCooldown"));
            _explosionDamage = Information.Stats.GetCustomStat("ExplosionDamage");

            OnDead += Veylar_OnDead;
            _maturationCooldown.Reset();
            _currentPhase = 1;
            this.ApplySize();
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            _layCooldown.SetBaseTime(Information.Stats.GetCustomStat("LayCooldown"));
            _maturationCooldown.SetBaseTime(Information.Stats.GetCustomStat("MaturationCooldown"));
            _explosionDamage = Information.Stats.GetCustomStat("ExplosionDamage");
            _layable = true;
            _maturationCooldown.Reset();
            _currentPhase = 1;
            this.ApplySize();
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _layCooldown.SetBaseTime(Information.Stats.GetCustomStat("LayCooldown"));
            _maturationCooldown.SetBaseTime(Information.Stats.GetCustomStat("MaturationCooldown"));
            _explosionDamage = Information.Stats.GetCustomStat("ExplosionDamage");
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

            if (modificationGroup.TryGetModification("LayCooldown", out LevelModification layCooldownModification))
            {
                _layCooldown.BaseTime += layCooldownModification.Value;
                _layCooldown.Reset();
            }

            if (modificationGroup.TryGetModification("MaturationCooldown", out LevelModification maturationCooldownModification))
            {
                _maturationCooldown.BaseTime += maturationCooldownModification.Value;
                _maturationCooldown.Reset();
            }
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
                    this.ApplySize();
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
            float explosionRadius = Information.Stats.GetCustomStat("ExplosionRadius");
            float radius = explosionRadius + _currentPhase * .5f;
            float finalExplosionDamage = _explosionDamage * _currentPhase;

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

        private void ApplySize()
        {
            float t = (_currentPhase - 1) / (float)(_maxPhase - 1);
            float size = Mathf.Lerp(_sizeRange.x, _sizeRange.y, t);
            
            this.SetSize(size);

            float multiPerPhase = Information.Stats.GetCustomStat("MultiPerPhase");
            Stats.Health.Add(multiPerPhase, StatValueType.Ratio);
            Stats.Armor.Add(multiPerPhase, StatValueType.Ratio);
            Stats.AttackDamage.Add(multiPerPhase, StatValueType.Ratio);
        }

        private void SpawnEggs(int eggCount)
        {
            VeylarEggs_Abiliity eggs = AbilityController.Instance.Spawn(_veylarEggsAbilityName, gameObject) as VeylarEggs_Abiliity;
            if (eggs == null) return;

            eggs.Leveling.SetLevel(Leveling.CurrentLevel);
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

        private void Veylar_OnDead(Combats.DamageContainer container)
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
            data.SetCustom("ExplosionDamage", _explosionDamage);
            data.SetCustom("Layable", _layable);
            data.SetCustom("MaturationCooldown", _maturationCooldown.CurrentTime);
            data.SetCustom("LayCooldown", _layCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(EnemySaveData data)
        {
            _currentPhase = data.GetCustom("CurrentPhase", 1);
            _explosionDamage = data.GetCustom("ExplosionDamage", 0f);
            _layable = data.GetCustom("Layable", false);
            _maturationCooldown.CurrentTime = data.GetCustom("MaturationCooldown", 0f);
            _layCooldown.CurrentTime = data.GetCustom("LayCooldown", 0f);
            this.ApplySize();
        }
    }
}
