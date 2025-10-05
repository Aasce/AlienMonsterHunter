using Asce.Game.Abilities;
using Asce.Game.FOVs;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class BlueSoldier_Drone_Entity : Entity
    {
        [SerializeField] private FieldOfView _fov;
        [SerializeField] private Rigidbody2D _rigidbody;

        [Header("Detection")]
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private LayerMask _seeLayer;
        [SerializeField] private float _viewRadius = 8f;

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

        protected override void Start()
        {
            base.Start();

            if (_fov != null) _fov.ViewRadius = _viewRadius;
        }

        private void Update()
        {
            AttackTargetHandle();
        }

        private void FixedUpdate()
        {
            MoveHandle();
        }

        private void LateUpdate()
        {
            if (_fov != null)
                _fov.DrawFieldOfView();
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

                float randomZ = UnityEngine.Random.Range(-10f, 10f); // Add slight randomization to avoid infinite bouncing
                _moveDirection = Quaternion.Euler(0f, 0f, randomZ) * _moveDirection;
            }
        }

        private void AttackTargetHandle()
        {
            _attackCooldown.Update(Time.deltaTime);
            if (!_attackCooldown.IsComplete) return;
            _attackCooldown.Reset();

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _viewRadius, _enemyLayer);
            foreach (Collider2D collider in colliders)
            {
                if (collider == null) continue;

                Vector2 direction = collider.transform.position - transform.position;
                float distance = direction.magnitude;
                direction.Normalize();

                int hitCount = Physics2D.RaycastNonAlloc(transform.position, direction, _hitsCache, distance, _seeLayer);

                bool canSeeTarget = false;
                for (int i = 0; i < hitCount; i++)
                {
                    var hit = _hitsCache[i];
                    if (hit.collider == null) continue;

                    if (LayerUtils.IsInLayerMask(hit.collider, _obstacleLayer)) break;
                    if (hit.collider == collider) 
                    { 
                        canSeeTarget = true; 
                        break; 
                    }
                }

                if (!canSeeTarget) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;

                this.Attack(target);
            }
        }

        private void Attack(ITargetable target)
        {
            BlueSoldier_Drone_Bullet_Ability bullet = AbilityController.Instance.Spawn(_bulletAbilityName, gameObject) as BlueSoldier_Drone_Bullet_Ability;
            if (bullet == null) return;

            Vector2 direction = target.transform.position - transform.position;

            bullet.DamageDeal =_damage;
            bullet.gameObject.SetActive(true);
            bullet.Fire(transform.position, direction);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)_moveDirection * _moveCheckDistance);
            Gizmos.DrawWireSphere(transform.position, _viewRadius);
        }
#endif
    }
}
