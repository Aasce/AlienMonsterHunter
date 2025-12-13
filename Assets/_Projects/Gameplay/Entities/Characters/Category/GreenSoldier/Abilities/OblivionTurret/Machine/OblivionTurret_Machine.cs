using Asce.Game.Abilities;
using Asce.Game.AIs;
using Asce.Game.FOVs;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class OblivionTurret_Machine : Machine, IMachineFireable
    {
        [Header("References")]
        [SerializeField] private FieldOfView _fov;
        [SerializeField] private FieldOfView _fovSelf;
        [SerializeField] private Transform _weapon;
        [SerializeField] private Transform _barrelL;
        [SerializeField] private Transform _barrelR;

        [Space]
        [SerializeField] private SingleTargetDetection _targetDetection;

        [Header("Attack Settings")]
        [SerializeField, Readonly] private float _damage = 10f;
        [SerializeField, Readonly] private int _maxAmmo = 2;
        [SerializeField, Readonly] private Cooldown _attackCooldown = new(1f);
        [SerializeField] private string _bulletAbilityName = "Oblivion Turret Bullet";

        [Header("Runtime")]
        [SerializeField, Readonly] private int _currentAmmo = 2;


        public event Action<Vector2, Vector2> OnFired;
        public event Action<int> OnCurrentAmmoChanged;

        public SingleTargetDetection TargetDetection => _targetDetection;

        public int MaxAmmo => _maxAmmo;
        public int CurrentAmmo
        {
            get => _currentAmmo;
            protected set
            {
                _currentAmmo = value;
                OnCurrentAmmoChanged?.Invoke(_currentAmmo);
            }
        }
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
            _maxAmmo = (int)Information.Stats.GetCustomStat("MaxAmmo");
            CurrentAmmo = _maxAmmo;
            _attackCooldown.SetBaseTime(Information.Stats.GetCustomStat("AttackSpeed"));
            TargetDetection.ViewRadius = Information.Stats.GetCustomStat("ViewRadius");
            TargetDetection.ViewAngle = Information.Stats.GetCustomStat("ViewAngle");

            _fov.ViewRadius = TargetDetection.ViewRadius;
            _fov.ViewAngle = TargetDetection.ViewAngle;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            TargetDetection.ResetTarget();
            CurrentAmmo = _maxAmmo;
            _attackCooldown.SetCurrentByRatio(0.5f);
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _damage = Information.Stats.GetCustomStat("Damage");
            _maxAmmo = (int)Information.Stats.GetCustomStat("MaxAmmo");
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

            if (modificationGroup.TryGetModification("ViewRadius", out LevelModification viewRadiusModification))
            {
                _targetDetection.ViewRadius += viewRadiusModification.Value;
                _fov.ViewRadius = _targetDetection.ViewRadius;
            }

            if (modificationGroup.TryGetModification("AttackSpeed", out LevelModification attackSpeedModification))
            {
                _attackCooldown.BaseTime += attackSpeedModification.Value;
                _attackCooldown.Reset();
            }

            if (modificationGroup.TryGetModification("MaxAmmo", out LevelModification maxAmmoModification))
            {
                _maxAmmo += Mathf.RoundToInt(maxAmmoModification.Value);
                CurrentAmmo = _maxAmmo;
            }
        }

        private void Update()
        {
            TargetDetection.UpdateDetection();
            RotateTowardsTarget();
            HandleAttack();
        }

        private void LateUpdate()
        {
            _fov.DrawFieldOfView();
            _fovSelf.DrawFieldOfView();
        }

        #region Rotation

        private void RotateTowardsTarget()
        {
            if (!TargetDetection.HasTarget) return;
            ITargetable target = TargetDetection.CurrentTarget;

            Vector2 direction = target.transform.position - transform.position;
            this.Rotate(direction);
        }

        /// <summary>
        /// Rotates the turret's weapon and FOV towards the given direction.
        /// </summary>
        public void Rotate(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            _weapon.rotation = Quaternion.Euler(0f, 0f, angle);
            _fov.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        }

        #endregion

        #region Attack

        private void HandleAttack()
        {
            _attackCooldown.Update(Time.deltaTime);
            if (!_attackCooldown.IsComplete) return;

            if (!TargetDetection.HasTarget || CurrentAmmo <= 0) return;
            ITargetable target = TargetDetection.CurrentTarget;

            _attackCooldown.Reset();

            this.FireRocket(_barrelL, target);
            this.FireRocket(_barrelR, target);

            CurrentAmmo--;
        }

        private void FireRocket(Transform barrel, ITargetable target)
        {
            Vector2 shootPos = barrel != null ? barrel.position : transform.position;
            Vector2 direction = target.transform.position - transform.position;

            this.Fire(shootPos, direction);
        }

        public void Fire(Vector2 position, Vector2 direction)
        {
            OblivionTurret_Bullet_Ability bullet =
                AbilityController.Instance.Spawn(_bulletAbilityName, gameObject) as OblivionTurret_Bullet_Ability;
            if (bullet == null) return;

            bullet.Leveling.SetLevel(Leveling.CurrentLevel);
            bullet.Set(_damage, position, direction);
            bullet.gameObject.SetActive(true);
            bullet.OnActive();

            OnFired?.Invoke(position, direction);
        }

        #endregion

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("Angle", _weapon.eulerAngles.z);
            data.SetCustom("Damage", _damage);
            data.SetCustom("CurrentAmmo", _currentAmmo);
            data.SetCustom("AttackSpeed", _attackCooldown.BaseTime);
            data.SetCustom("AttackCooldown", _attackCooldown.CurrentTime);
            data.SetCustom("ViewRadius", _targetDetection.ViewRadius);
        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            _weapon.eulerAngles = new Vector3(0f, 0f, data.GetCustom<float>("Angle"));
            _damage = data.GetCustom<float>("Damage");
            _currentAmmo = data.GetCustom<int>("CurrentAmmo");
            _attackCooldown.BaseTime = data.GetCustom<float>("AttackSpeed");
            _attackCooldown.CurrentTime = data.GetCustom<float>("AttackCooldown");
            _targetDetection.ViewRadius = data.GetCustom<float>("ViewRadius");
            _fov.ViewRadius = _targetDetection.ViewRadius;
        }
    }
}
