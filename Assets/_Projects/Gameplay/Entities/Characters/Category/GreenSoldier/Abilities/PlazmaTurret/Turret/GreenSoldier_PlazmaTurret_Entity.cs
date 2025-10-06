using Asce.Game.Abilities;
using Asce.Game.FOVs;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class GreenSoldier_PlazmaTurret_Entity : Entity
    {
        [SerializeField] private FieldOfView _fov;
        [SerializeField] private FieldOfView _fovSelf;

        [Space]
        [SerializeField] private Transform _weapon;
        [SerializeField] private Transform _barrielL;
        [SerializeField] private Transform _barrielR;

        [Header("Detection")]
        [SerializeField] private LayerMask _enemylayer;
        [SerializeField] private LayerMask _seelayer;
        [SerializeField] private float _viewRadius = 8f;
        [SerializeField] private float _viewAngle = 70f;
        [SerializeField] private Cooldown _detectCooldown = new(0.1f);
        [SerializeField] private ITargetable _target;

        [Header("Attack")]
        [SerializeField] private float _damage = 10f;
        [SerializeField, Min(0f)] private float _explosionRadius = 2f;
        [SerializeField, Min(0)] private int _maxAmmo = 2;
        [SerializeField, Min(0)] private int _currentAmmo = 2;
        [SerializeField] private Cooldown _attackCooldown = new(1f);
        [SerializeField] private string _bulletAbilityName = "Greeb Soldier Plazma Turret Bullet";

        public event Action<int> OnCurrentAmmoChanged;

        public int CurrentAmmo
        {
            get => _currentAmmo;
            set
            {
                _currentAmmo = value;
                OnCurrentAmmoChanged?.Invoke(_currentAmmo);
            }
        }

        protected override void Start()
        {
            base.Start();

            if (_fov != null)
            {
                _fov.ViewRadius = _viewRadius;
                _fov.ViewAngle = _viewAngle;
            }
        }

        private void Update()
        {
            UpdateDetection();
            RotatingHandle();
            AttackTargetHandle();
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
            _target = null;
        }

        #region Detection Logic

        private void UpdateDetection()
        {
            _detectCooldown.Update(Time.deltaTime);
            if (!_detectCooldown.IsComplete) return;

            _detectCooldown.Reset();

            if (_target != null)
            {
                if (!IsTargetValid(_target))
                    _target = null;
                return;
            }

            _target = FindVisibleTarget();
        }

        private ITargetable FindVisibleTarget()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _viewRadius, _enemylayer);
            ITargetable nearestTarget = null;
            float nearestDist = float.MaxValue;

            foreach (Collider2D collider in colliders)
            {
                if (collider == null) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;

                if (!IsInViewAngle(collider.transform.position)) continue;
                if (!HasLineOfSight(collider.transform)) continue;

                float dist = Vector2.Distance(transform.position, collider.transform.position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearestTarget = target;
                }
            }

            return nearestTarget;
        }

        private bool IsTargetValid(ITargetable target)
        {
            if (target == null) return false;

            Vector2 direction = target.transform.position - transform.position;

            // 1. In view radius
            if (direction.magnitude > _viewRadius)
                return false;

            // 2. In view angle
            if (!IsInViewAngle(target.transform.position))
                return false;

            // 3. Has line of sight
            if (!HasLineOfSight(target.transform))
                return false;

            return true;
        }

        private bool IsInViewAngle(Vector2 position)
        {
            if (_weapon == null) return true;

            Vector2 directionToTarget = (position - (Vector2)transform.position).normalized;
            float angleToTarget = Vector2.Angle(_weapon.up, directionToTarget);

            return angleToTarget <= _viewAngle / 2f;
        }

        private bool HasLineOfSight(Transform target)
        {
            Vector2 direction = target.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, _viewRadius, _seelayer);

            return hit.transform == target;
        }

        #endregion

        #region Rotation & Attack

        private void RotatingHandle()
        {
            if (_target != null)
            {
                Vector2 direction = _target.transform.position - transform.position;
                this.Rotate(direction);
            }

        }

        private void AttackTargetHandle()
        {
            _attackCooldown.Update(Time.deltaTime);
            if (!_attackCooldown.IsComplete) return;

            _attackCooldown.Reset();
            if (_target == null) return;
            if (CurrentAmmo <= 0) return;

            FireRocket(_barrielL);
            FireRocket(_barrielR);

            CurrentAmmo--;
        }

        private void FireRocket(Transform barriel)
        {
            GreenSoldier_PlazmaTurret_Bullet_Ability bullet =
                AbilityController.Instance.Spawn(_bulletAbilityName, gameObject) as GreenSoldier_PlazmaTurret_Bullet_Ability;

            if (bullet == null) return;

            Vector2 shootPosition = barriel != null ? barriel.transform.position : transform.position;
            Vector2 direction = _target.transform.position - transform.position;

            bullet.DamageDeal = _damage;
            bullet.ExplosionRadius = _explosionRadius;
            bullet.gameObject.SetActive(true);
            bullet.Fire(shootPosition, direction);
        }

        public void Rotate(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            if (_weapon != null) _weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            if (_fov != null) _fov.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90);
        }
        #endregion
    }
}
