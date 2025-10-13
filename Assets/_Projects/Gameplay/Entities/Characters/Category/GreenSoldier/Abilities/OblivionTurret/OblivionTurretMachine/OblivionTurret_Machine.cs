using Asce.Game.Abilities;
using Asce.Game.AIs;
using Asce.Game.FOVs;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    /// <summary>
    /// Represents an automated turret that detects, tracks, and attacks enemies using rockets.
    /// </summary>
    public class OblivionTurret_Machine : Machine
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
        [SerializeField, Min(0)] private int _maxAmmo = 2;
        [SerializeField, Min(0)] private int _currentAmmo = 2;
        [SerializeField] private Cooldown _attackCooldown = new(1f);
        [SerializeField] private string _bulletAbilityName = "Oblivion Turret Bullet";

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
            _maxAmmo = (int)Information.Stats.GetCustomStat("MaxAmmo");
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
            OblivionTurret_Bullet_Ability bullet =
                AbilityController.Instance.Spawn(_bulletAbilityName, gameObject) as OblivionTurret_Bullet_Ability;
            if (bullet == null) return;

            Vector2 shootPos = barrel != null ? barrel.position : transform.position;
            Vector2 direction = target.transform.position - transform.position;

            bullet.DamageDeal = Information.Stats.GetCustomStat("Damage");
            bullet.ExplosionRadius = Information.Stats.GetCustomStat("ExplosionRadius");
            bullet.gameObject.SetActive(true);
            bullet.Fire(shootPos, direction);
            bullet.OnActive();
        }

        #endregion
    }
}
