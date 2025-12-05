using Asce.Game.Abilities;
using Asce.Game.AIs;
using Asce.Game.FOVs;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class HellFlamethrower_Machine : Machine
    {
        [Header("References")]
        [SerializeField] private FieldOfView _fov;
        [SerializeField] private FieldOfView _fovSelf;
        [SerializeField] private Transform _weapon;
        [SerializeField] private Transform _barrel;

        [Space]
        [SerializeField] private SingleTargetDetection _targetDetection;

        [Header("Attack Settings")]
        [SerializeField, Readonly] private float _damage = 10f;
        [SerializeField, Readonly] private Cooldown _attackCooldown = new(.4f);
        [SerializeField] private string _fireAbilityName = "Hell Flamethrower Fire";

        [Header("Overheat")]
        [SerializeField, Readonly] private Cooldown _overheatRecoveryCooldown = new(3f);
        [SerializeField, Readonly] private int _attackToOverheat = 12;
        [SerializeField, Readonly] private int _currentAttack = 0;
        [SerializeField, Readonly] private bool _isOverheat = false;


        public SingleTargetDetection TargetDetection => _targetDetection;
        public Cooldown OverheatRecoveryCooldown => _overheatRecoveryCooldown;
        public int AttackToOverheat => _attackToOverheat;
        public int CurrentAttack => _currentAttack;
        public bool IsOverheat => _isOverheat;

        public override void Initialize()
        {
            base.Initialize();
            _damage = Information.Stats.GetCustomStat("Damage");
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
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _damage = Information.Stats.GetCustomStat("Damage");
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
            _weapon.rotation = Quaternion.Euler(0f, 0f, smoothAngle);
            _fov.transform.rotation = Quaternion.Euler(0f, 0f, smoothAngle + 90f);
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
            HellFlamethrower_Fire_Ability fire =
                AbilityController.Instance.Spawn(_fireAbilityName, gameObject) as HellFlamethrower_Fire_Ability;
            if (fire == null) return;

            Vector2 shootPos = _barrel != null ? _barrel.position : transform.position;
            Vector2 direction = _weapon.transform.up;

            fire.Leveling.SetLevel(Leveling.CurrentLevel);
            fire.gameObject.SetActive(true);
            fire.Set(_damage, shootPos, direction);
            fire.OnActive();
        }

        public void Rotate(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            _weapon.rotation = Quaternion.Euler(0f, 0f, angle);
            _fov.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        }

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("Angle", _weapon.eulerAngles.z);
            data.SetCustom("Damage", _damage);
            data.SetCustom("OverheatRecoveryTime", _overheatRecoveryCooldown.BaseTime);
            data.SetCustom("OverheatRecoveryCooldown", _overheatRecoveryCooldown.CurrentTime);

            data.SetCustom("AttackToOverheat", _attackToOverheat);
            data.SetCustom("CurrentAttack", _currentAttack);
            data.SetCustom("AttackCooldown", _attackCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            _weapon.eulerAngles = new Vector3(0f, 0f, data.GetCustom<float>("Angle"));
            _damage = data.GetCustom<float>("Damage");
            _overheatRecoveryCooldown.BaseTime = data.GetCustom<float>("OverheatRecoveryTime");
            _overheatRecoveryCooldown.CurrentTime = data.GetCustom<float>("OverheatRecoveryCooldown");

            _attackToOverheat = data.GetCustom<int>("AttackToOverheat");
            _currentAttack = data.GetCustom<int>("CurrentAttack");
            _attackCooldown.CurrentTime = data.GetCustom<float>("AttackCooldown");
        }
    }
}
