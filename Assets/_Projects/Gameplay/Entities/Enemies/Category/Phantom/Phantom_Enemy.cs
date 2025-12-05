using Asce.Game.Combats;
using Asce.Game.Effects;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class Phantom_Enemy : Enemy
    {
        [Header("Visibility Settings")]
        [SerializeField] private float _showDistance = 5f;
        [SerializeField] private Cooldown _checkCooldown = new(0.1f);
        [SerializeField] private float _checkDistance = 10f;
        [SerializeField, Min(0f)] private float _fadeSpeed = 5f;

        private float _currentAlpha = 1f;
        private float _targetAlpha = 1f;

        [Header("Healing")]
        [SerializeField, Readonly] private float _healingAmount = 1f;
        [SerializeField] private Cooldown _healCooldown = new(0.25f);

        [Header("Decay")]
        [SerializeField, Readonly] private float _decayDuration = 5f;
        [SerializeField, Readonly] private float _decayStrength = .05f;

        private string _untargetableStatId;

        public override void Initialize()
        {
            base.Initialize();

            _showDistance = Information.Stats.GetCustomStat("ShowDistance");
            _healingAmount = Information.Stats.GetCustomStat("HealAmount");
            _decayDuration = Information.Stats.GetCustomStat("DecayDuration");
            _decayStrength = Information.Stats.GetCustomStat("DecayStrength");
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            this.Hide();
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _showDistance = Information.Stats.GetCustomStat("ShowDistance");
            _healingAmount = Information.Stats.GetCustomStat("HealAmount");
            _decayDuration = Information.Stats.GetCustomStat("DecayDuration");
            _decayStrength = Information.Stats.GetCustomStat("DecayStrength");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("ShowDistance", out LevelModification showDistanceModification))
            {
                _showDistance += showDistanceModification.Value;
            }

            if (modificationGroup.TryGetModification("HealAmount", out LevelModification healAmountModification))
            {
                _healingAmount += healAmountModification.Value;
            }

            if (modificationGroup.TryGetModification("DecayDuration", out LevelModification decayDurationModification))
            {
                _decayDuration += decayDurationModification.Value;
            }

            if (modificationGroup.TryGetModification("DecayStrength", out LevelModification decayStrengthModification))
            {
                _decayStrength += decayStrengthModification.Value;
            }
        }

        protected override void Update()
        {
            base.Update();

            // Smooth alpha update
            _currentAlpha = Mathf.Lerp(_currentAlpha, _targetAlpha, Time.deltaTime * _fadeSpeed);
            View.SetAlpha(_currentAlpha);

            // Check visibility
            _checkCooldown.Update(Time.deltaTime);
            if (_checkCooldown.IsComplete)
            {
                _checkCooldown.Reset();
                VisibilityHandle();
            }

            this.HealHandle();
        }

        private void HealHandle()
        {
            if (IsTargetable)
            {
                _healCooldown.Reset();
                return;
            }

            _healCooldown.Update(Time.deltaTime);
            if (_healCooldown.IsComplete)
            {
                _healCooldown.Reset();
                if (Stats.Health.IsFull) return;
                CombatController.Instance.Healing(this, _healingAmount);
            }
        }

        protected override void MoveToTaget() => this.DefaultMoveToTaget();

        protected override void Attack()
        {
            ITargetable target = TargetDetection.CurrentTarget;
            float damage = Stats.AttackDamage.FinalValue;

            EffectController.Instance.AddEffect("Decay", this, target as Entity, new EffectData()
            {
                Duration = _decayDuration,
                Strength = _decayStrength,
            });
            CombatController.Instance.DamageDealing(new DamageContainer(this, target as ITakeDamageable)
            {
                Damage = damage,
            });
        }

        private void VisibilityHandle()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _checkDistance, TargetDetection.TargetLayer);
            float nearest = float.MaxValue;

            foreach (Collider2D collider in colliders)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < nearest)
                    nearest = distance;
            }

            if (nearest < _showDistance) Show();
            else Hide();
        }

        private void Hide()
        {
            _targetAlpha = 0f;
            if (!string.IsNullOrEmpty(_untargetableStatId)) return;
            _untargetableStatId = Effects.Untargetable.Add().Id;
        }

        private void Show()
        {
            _targetAlpha = 1f;
            Effects.Untargetable.RemoveById(_untargetableStatId);
            _untargetableStatId = string.Empty;
        }

        protected override void OnBeforeSave(EnemySaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("UntargetableStatId", _untargetableStatId);
            data.SetCustom("ShowDistance", _showDistance);
            data.SetCustom("HealAmount", _healingAmount);
            data.SetCustom("DecayDuration", _decayDuration);
            data.SetCustom("DecayStrength", _decayStrength);
        }

        protected override void OnAfterLoad(EnemySaveData data)
        {
            base.OnAfterLoad(data);
            _untargetableStatId = data.GetCustom<string>("UntargetableStatId");
            _showDistance = data.GetCustom<float>("ShowDistance");
            _healingAmount = data.GetCustom<float>("HealAmount");
            _decayDuration = data.GetCustom<float>("DecayDuration");
            _decayStrength = data.GetCustom<float>("DecayStrength");
        }

    }
}
