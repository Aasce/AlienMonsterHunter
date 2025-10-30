using Asce.Game.Entities.Characters;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Interactions
{
    [RequireComponent(typeof(ExpOrb_InteractiveObject))]
    public class ExpOrb_Attraction : GameComponent
    {
        [Header("Settings")]
        [SerializeField, Min(0f)] private float _detectRadius = 5f;
        [SerializeField, Min(0f)] private float _moveSpeed = 5f;
        [SerializeField, Min(0f)] private float _stopDistance = 0.2f;
        [SerializeField] private Cooldown _findTargetCooldown = new(1f);

        [Space]
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private LayerMask _obstacleLayer;

        [Header("References")]
        [SerializeField, Readonly] private ExpOrb_InteractiveObject _expOrb;
        private readonly RaycastHit2D[] _cacheHits = new RaycastHit2D[10];

        private Transform _target;

        public Rigidbody2D Rigidbody => _expOrb.Rigidbody;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _expOrb);
        }

        private void FixedUpdate()
        {
            if (_target == null || !_target.gameObject.activeInHierarchy)
            {
                _findTargetCooldown.Update(Time.fixedDeltaTime);
                if (_findTargetCooldown.IsComplete)
                {
                    FindTarget();
                    _findTargetCooldown.Reset();
                }
                return;
            }

            if (!CanSeeTarget())
            {
                _target = null;
                return;
            }

            MoveTowardTarget();
        }

        private void FindTarget()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _detectRadius, _targetLayer);
            if (hits.Length == 0) return;

            _target = hits[0].transform;
        }

        private bool CanSeeTarget()
        {
            if (_target == null) return false;

            Vector2 dir = (_target.transform.position - transform.position).normalized;
            float dist = Vector2.Distance(transform.position, _target.transform.position);

            int count = Physics2D.RaycastNonAlloc(transform.position, dir, _cacheHits, dist, _targetLayer | _obstacleLayer);
            for (int i = 0; i < count; i++)
            {
                RaycastHit2D hit = _cacheHits[i];
                if (LayerUtils.IsInLayerMask(hit.collider.gameObject.layer, _obstacleLayer))
                    return false;

                if (hit.collider.TryGetComponent(out Character _))
                    return true;
            }

            return false;
        }
        private void MoveTowardTarget()
        {
            if (_target == null) return;

            Vector2 dir = (_target.transform.position - transform.position);
            float distance = dir.magnitude;

            if (distance < _stopDistance)
            {
                _expOrb.Rigidbody.linearVelocity = Vector2.zero;
                return;
            }

            dir.Normalize();
            float speed = Mathf.Lerp(_moveSpeed * 0.5f, _moveSpeed * 2f, 1f - (distance / _detectRadius));
            Vector2 targetVelocity = dir * speed;

            // Lerp velocity for smooth movement
            _expOrb.Rigidbody.linearVelocity = Vector2.Lerp(
                _expOrb.Rigidbody.linearVelocity,
                targetVelocity,
                0.1f // Smooth factor, adjust between 0.05–0.2 for desired smoothness
            );
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectRadius);
        }
#endif
    }
}
