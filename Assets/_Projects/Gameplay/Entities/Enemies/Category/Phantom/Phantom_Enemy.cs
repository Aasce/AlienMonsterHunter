using Asce.Game.Combats;
using Asce.Game.Effects;
using Asce.Game.Levelings;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
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

        [Header("Decay")]
        [SerializeField, Readonly] private float _decayDuration = 5f;
        [SerializeField, Readonly] private float _decayStrength = .05f;

        public override void Initialize()
        {
            base.Initialize();

            _showDistance = Information.Stats.GetCustomStat("ShowDistance");
            _decayDuration = Information.Stats.GetCustomStat("DecayDuration");
            _decayStrength = Information.Stats.GetCustomStat("DecayStrength");
            Agent.stoppingDistance = Stats.AttackRange.FinalValue * 0.9f;
            Stats.AttackRange.OnFinalValueChanged += (oldValue, newValue) =>
            {
                Agent.stoppingDistance = newValue;
            };
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _showDistance = Information.Stats.GetCustomStat("ShowDistance");
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
                HandleVisibility();
            }

            // Rotate
            if (Agent.velocity.magnitude > 0.02f)
            {
                float angle = Mathf.Atan2(Agent.velocity.y, Agent.velocity.x) * Mathf.Rad2Deg - 90f;
                float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.z, angle, Time.deltaTime * 10f);
                transform.rotation = Quaternion.Euler(0f, 0f, smoothAngle);
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

        private void HandleVisibility()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _checkDistance, TargetDetection.TargetLayer);
            float nearest = float.MaxValue;

            foreach (Collider2D collider in colliders)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < nearest)
                    nearest = distance;
            }

            if (nearest < _showDistance)
                Show();
            else
                Hide();
        }

        private void Hide()
        {
            IsTargetable = false;
            _targetAlpha = 0f;
        }

        private void Show()
        {
            IsTargetable = true;
            _targetAlpha = 1f;
        }
    }
}
