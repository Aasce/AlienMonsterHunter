using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.AIs;
using Asce.Game.Combats;
using Asce.Game.Effects;
using Asce.Game.FOVs;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class HellFlamethrower_Machine : Machine, IMachineRotatable
    {
        [Header("References")]
        [SerializeField] private FieldOfView _fov;
        [SerializeField] private FieldOfView _fovSelf;
        [SerializeField] private Transform _weapon;
        [SerializeField] private Transform _barrel;

        [Space]
        [SerializeField] private SingleTargetDetection _targetDetection;

        [Header("Fire")]
        [SerializeField, Readonly] private float _damage = 10f;
        [SerializeField, Readonly] private Cooldown _attackCooldown = new(.2f);
        [SerializeField] private float _fireRadius = 10f;
        [SerializeField] private float _fireAngle = 30f;
        [SerializeField] private LayerMask _targetLayer;

        private readonly Collider2D[] _overlapResults = new Collider2D[16];
        private ContactFilter2D _contactFilter;

        [Header("Ignite Settings")]
        [SerializeField, Readonly] private float _igniteDuration = 5f;
        [SerializeField, Readonly] private float _igniteStrength = 3f;

        [Header("Overheat")]
        [SerializeField, Readonly] private Cooldown _overheatRecoveryCooldown = new(3f);
        [SerializeField, Readonly] private int _attackToOverheat = 12;
        [SerializeField, Readonly] private int _currentAttack = 0;
        [SerializeField, Readonly] private bool _isOverheat = false;
        private bool _isSpraying = false;

        public event Action<float> OnRotated;
        public event Action<bool> OnSprayStateChanged;

        public SingleTargetDetection TargetDetection => _targetDetection;
        public Cooldown OverheatRecoveryCooldown => _overheatRecoveryCooldown;
        public int AttackToOverheat => _attackToOverheat;
        public int CurrentAttack => _currentAttack;
        public bool IsOverheat => _isOverheat;
        public float Angle => (_weapon != null ? _weapon : transform).eulerAngles.z;

        public override void Initialize()
        {
            base.Initialize();
            _contactFilter = new ContactFilter2D
            {
                layerMask = _targetLayer,
                useLayerMask = true,
                useTriggers = true,
            };

            _damage = Information.Stats.GetCustomStat("Damage");
            _igniteDuration = Information.Stats.GetCustomStat("IgniteDuration");
            _igniteStrength = Information.Stats.GetCustomStat("IgniteStrength");

            _overheatRecoveryCooldown.SetBaseTime(Information.Stats.GetCustomStat("OverheatRecoveryTime"));
            TargetDetection.ViewRadius = Information.Stats.GetCustomStat("ViewRadius");
            _fov.ViewRadius = TargetDetection.ViewRadius;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            TargetDetection.ResetTarget();
            _isOverheat = false;
            _currentAttack = 0;
            _attackCooldown.ToComplete();
            _overheatRecoveryCooldown.ToComplete();
            _isSpraying = false;
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _damage = Information.Stats.GetCustomStat("Damage");
            _igniteDuration = Information.Stats.GetCustomStat("IgniteDuration");
            _igniteStrength = Information.Stats.GetCustomStat("IgniteStrength");
            _overheatRecoveryCooldown.SetBaseTime(Information.Stats.GetCustomStat("OverheatRecoveryTime"));
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

            if (modificationGroup.TryGetModification("IgniteDuration", out LevelModification igniteDurationModification))
            {
                _igniteDuration += igniteDurationModification.Value;
            }

            if (modificationGroup.TryGetModification("IgniteStrength", out LevelModification igniteStrengthModification))
            {
                _igniteStrength += igniteStrengthModification.Value;
            }

            if (modificationGroup.TryGetModification("OverheatRecoveryTime", out LevelModification overheatRecoveryTimeModification))
            {
                _overheatRecoveryCooldown.BaseTime += overheatRecoveryTimeModification.Value;
                _overheatRecoveryCooldown.ToComplete();
            }
        }

        private void Update()
        {
            TargetDetection.UpdateDetection();
            this.RotateTowardsTarget();
            this.OverheatHandle();
        }

        private void OverheatHandle()
        {
            if (_isOverheat)
            {
                this.SetSprayingState(false);
                _overheatRecoveryCooldown.Update(Time.deltaTime);
                if (_overheatRecoveryCooldown.IsComplete)
                {
                    _isOverheat = false;
                    _currentAttack = 0;
                    _attackCooldown.ToComplete();
                }
            }
            else
            {
                this.SetSprayingState(true);
                this.HandleAttack();
                if (_currentAttack >= _attackToOverheat)
                {
                    _isOverheat = true;
                    _overheatRecoveryCooldown.Reset();
                }
            }
        }

        private void LateUpdate()
        {
            _fov.DrawFieldOfView();
            _fovSelf.DrawFieldOfView();
        }
        private void RotateTowardsTarget()
        {
            if (!TargetDetection.HasTarget) return;
            ITargetable target = TargetDetection.CurrentTarget;

            Vector2 direction = target.transform.position - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            // Smooth rotation
            float currentAngle = _weapon.rotation.eulerAngles.z;
            float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * 5f);

            // Apply rotation
            this.Rotate(smoothAngle);
        }


        private void HandleAttack()
        {
            _attackCooldown.Update(Time.deltaTime);
            if (!_attackCooldown.IsComplete) return;

            this.Attack();

            _attackCooldown.Reset();
            _currentAttack++;
        }
        private void Attack()
        {
            Vector3 origin = _barrel != null ? _barrel.position : transform.position;
            Vector2 forward = _barrel.up;

            int count = Physic2DUtils.OverlapConeNonAlloc(origin, _fireRadius, forward, _fireAngle, _contactFilter, _overlapResults);
            for (int i = 0; i < count; i++)
            {
                Collider2D collider = _overlapResults[i];
                if (collider == null) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;

                CombatController.Instance.DamageDealing(new DamageContainer(this, target as ITakeDamageable)
                {
                    Damage = _damage,
                });

                EffectController.Instance.AddEffect("Ignite", this, target as Entity, new EffectData()
                {
                    Duration = _igniteDuration,
                    Strength = _igniteStrength,
                });
            }
        }

        public void Rotate(float angle)
        {
            _weapon.rotation = Quaternion.Euler(0f, 0f, angle);
            _fov.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
            OnRotated?.Invoke(angle);
        }

        private void SetSprayingState(bool state)
        {
            if (_isSpraying == state) return;
            _isSpraying = state;
            OnSprayStateChanged?.Invoke(_isSpraying);
        }

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("Angle", _weapon.eulerAngles.z);
            data.SetCustom("Damage", _damage);
            data.SetCustom("IgniteDuration", _igniteDuration);
            data.SetCustom("IgniteStrength", _igniteStrength);

            data.SetCustom("OverheatRecoveryTime", _overheatRecoveryCooldown.BaseTime);
            data.SetCustom("OverheatRecoveryCooldown", _overheatRecoveryCooldown.CurrentTime);

            data.SetCustom("AttackToOverheat", _attackToOverheat);
            data.SetCustom("CurrentAttack", _currentAttack);
            data.SetCustom("AttackCooldown", _attackCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            float angle = data.GetCustom<float>("Angle");
            this.Rotate(angle);

            _damage = data.GetCustom<float>("Damage");
            _igniteDuration = data.GetCustom<float>("IgniteDuration");
            _igniteStrength = data.GetCustom<float>("IgniteStrength");

            _overheatRecoveryCooldown.BaseTime = data.GetCustom<float>("OverheatRecoveryTime");
            _overheatRecoveryCooldown.CurrentTime = data.GetCustom<float>("OverheatRecoveryCooldown");

            _attackToOverheat = data.GetCustom<int>("AttackToOverheat");
            _currentAttack = data.GetCustom<int>("CurrentAttack");
            _attackCooldown.CurrentTime = data.GetCustom<float>("AttackCooldown");
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            Vector3 origin = _barrel != null ? _barrel.position : transform.position;
            Vector3 forward = _barrel != null ? _barrel.up.normalized : transform.up.normalized;

            // Helper to draw spread edge lines for a given angle and distance
            void DrawSpreadEdges(float angleDegrees, float distance, Color col)
            {
                float angleLeft = -angleDegrees * 0.5f;
                float angleRight = angleDegrees * 0.5f;

                Vector3 leftDir = Quaternion.Euler(0f, 0f, angleLeft) * forward;
                Vector3 rightDir = Quaternion.Euler(0f, 0f, angleRight) * forward;

                Gizmos.color = col;
                Gizmos.DrawLine(origin, origin + leftDir * distance);
                Gizmos.DrawLine(origin, origin + rightDir * distance);
            }

            DrawSpreadEdges(_fireAngle, _fireRadius, Color.red);
            Gizmos.DrawWireSphere(origin, _fireRadius);
        }
#endif
    }
}
