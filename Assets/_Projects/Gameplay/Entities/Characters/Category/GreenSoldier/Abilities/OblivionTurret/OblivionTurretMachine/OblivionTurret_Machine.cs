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
        [SerializeField] private float _damage = 10f;
        [SerializeField, Min(0f)] private float _explosionRadius = 2f;
        [SerializeField, Min(0)] private int _maxAmmo = 2;
        [SerializeField, Min(0)] private int _currentAmmo = 2;
        [SerializeField] private Cooldown _attackCooldown = new(1f);
        [SerializeField] private string _bulletAbilityName = "Oblivion Turret Bullet";

        public event Action<int> OnCurrentAmmoChanged;

        public SingleTargetDetection TargetDetection => _targetDetection;

        public float Damage => _damage;
        public float ExplosionRadius => _explosionRadius;
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
            _explosionRadius = Information.Stats.GetCustomStat("ExplosionRadius");
            _maxAmmo = (int)Information.Stats.GetCustomStat("MaxAmmo");
            _attackCooldown.SetBaseTime(Information.Stats.GetCustomStat("AttackSpeed"));
            _targetDetection.ViewRadius = Information.Stats.GetCustomStat("ViewRadius");
            _targetDetection.ViewAngle = Information.Stats.GetCustomStat("ViewAngle");

            if (_fov != null)
            {
                _fov.ViewRadius = _targetDetection.ViewRadius;
                _fov.ViewAngle = _targetDetection.ViewAngle;
            }
        }

        private void Update()
        {
            _targetDetection.UpdateDetection();
            RotateTowardsTarget();
            HandleAttack();
        }

        private void LateUpdate()
        {
            if (_fov != null) _fov.DrawFieldOfView();
            if (_fovSelf != null) _fovSelf.DrawFieldOfView();
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            CurrentAmmo = _maxAmmo;
            _attackCooldown.SetCurrentByRatio(0.5f);
            _targetDetection.ResetTarget();
        }

        #region Rotation

        private void RotateTowardsTarget()
        {
            var target = _targetDetection.CurrentTarget;
            if (target == null) return;

            Vector2 direction = target.transform.position - transform.position;
            Rotate(direction);
        }

        /// <summary>
        /// Rotates the turret's weapon and FOV towards the given direction.
        /// </summary>
        public void Rotate(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            if (_weapon != null)
                _weapon.rotation = Quaternion.Euler(0f, 0f, angle);

            if (_fov != null)
                _fov.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        }

        #endregion

        #region Attack

        private void HandleAttack()
        {
            _attackCooldown.Update(Time.deltaTime);
            if (!_attackCooldown.IsComplete) return;

            var target = _targetDetection.CurrentTarget;
            if (target == null || CurrentAmmo <= 0) return;

            _attackCooldown.Reset();

            FireRocket(_barrelL, target);
            FireRocket(_barrelR, target);

            CurrentAmmo--;
        }

        private void FireRocket(Transform barrel, ITargetable target)
        {
            if (target == null) return;

            OblivionTurret_Bullet_Ability bullet =
                AbilityController.Instance.Spawn(_bulletAbilityName, gameObject) as OblivionTurret_Bullet_Ability;

            if (bullet == null) return;

            Vector2 shootPos = barrel != null ? barrel.position : transform.position;
            Vector2 direction = target.transform.position - transform.position;

            bullet.DamageDeal = _damage;
            bullet.ExplosionRadius = _explosionRadius;
            bullet.gameObject.SetActive(true);
            bullet.Fire(shootPos, direction);
        }

        #endregion
    }
}
