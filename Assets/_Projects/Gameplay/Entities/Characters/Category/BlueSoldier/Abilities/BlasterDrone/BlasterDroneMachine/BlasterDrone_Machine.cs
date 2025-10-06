using Asce.Game.Abilities;
using Asce.Game.AIs;
using Asce.Game.FOVs;
using Asce.Managers;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class BlasterDrone_Machine : Machine
    {
        [Header("Reference")]
        [SerializeField] private FieldOfView _fov;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private MultiTargetDetection _targetDetection;

        [Header("Attack")]
        [SerializeField] private float _damage = 10f;
        [SerializeField] private Cooldown _attackCooldown = new(1f);
        [SerializeField] private string _bulletAbilityName = "Blue Soldier Drone Bullet";

        [Header("Movement")]
        [SerializeField] private float _moveCheckDistance = 0.5f;
        [SerializeField] private LayerMask _obstacleLayer;

        private Vector2 _moveDirection = Vector2.right;
        private readonly RaycastHit2D[] _hitsCache = new RaycastHit2D[16];

        public Vector2 MoveDirection
        {
            get => _moveDirection;
            set => _moveDirection = value.normalized;
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _rigidbody);
            this.LoadComponent(out _targetDetection);
        }

        protected override void Start()
        {
            base.Start();

            if (_fov != null) _fov.ViewRadius = _targetDetection.ViewRadius;
        }

        private void Update()
        {
            _targetDetection.UpdateDetection();
            this.AttackVisibleTargets();
        }

        private void FixedUpdate()
        {
            this.MoveHandle();
        }

        private void LateUpdate()
        {
            _fov?.DrawFieldOfView();
        }

        /// <summary>
        ///     Handles drone movement, wall reflection, and rotation toward movement direction.
        /// </summary>
        private void MoveHandle()
        {
            float speed = Stats.Speed.FinalValue;
            if (speed <= 0f) return;

            // Move forward
            Vector2 currentPosition = _rigidbody.position;
            Vector2 nextPosition = currentPosition + _moveDirection * speed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(nextPosition);

            // Rotate drone to face move direction
            if (_moveDirection.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg - 90f;
                _rigidbody.MoveRotation(angle);
            }

            // Check for wall collision ahead
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, _moveDirection, _moveCheckDistance, _obstacleLayer);
            if (hit.collider != null)
            {
                // Reflect movement direction based on the wall normal
                _moveDirection = Vector2.Reflect(_moveDirection, hit.normal).normalized;

                // Add slight randomization to avoid infinite bouncing
                float randomZ = UnityEngine.Random.Range(-10f, 10f);
                _moveDirection = Quaternion.Euler(0f, 0f, randomZ) * _moveDirection;
            }
        }

        /// <summary>
        ///     Attacks all visible enemies detected by MultiTargetDetection.
        /// </summary>
        private void AttackVisibleTargets()
        {
            _attackCooldown.Update(Time.deltaTime);
            if (!_attackCooldown.IsComplete) return;
            _attackCooldown.Reset();

            if (_targetDetection == null) return;

            IReadOnlyList<ITargetable> targets = _targetDetection.VisibleTargets;
            if (targets.Count == 0) return;

            foreach (var target in targets)
            {
                if (target == null) continue;
                Vector2 direction = target.transform.position - transform.position;
                this.FireBullet(direction);
            }
        }

        private void FireBullet(Vector2 direction)
        {
            BlasterDrone_Bullet_Ability bullet =
                AbilityController.Instance.Spawn(_bulletAbilityName, gameObject) as BlasterDrone_Bullet_Ability;
            if (bullet == null) return;

            bullet.DamageDeal = _damage;
            bullet.gameObject.SetActive(true);
            bullet.Fire(transform.position, direction);
        }

    }
}
