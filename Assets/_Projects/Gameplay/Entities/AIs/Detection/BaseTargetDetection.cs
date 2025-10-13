using Asce.Managers;
using Asce.Managers.Utils;
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Asce.Game.AIs
{
    /// <summary>
    ///     Base class for all target detection systems.
    ///     Provides core field-of-view and line-of-sight logic.
    /// </summary>
    public abstract class BaseTargetDetection : GameComponent
    {
        [Header("Detection Settings")]
        [SerializeField] protected LayerMask _targetLayer;
        [SerializeField] protected LayerMask _obstacleLayer;
        [SerializeField, Min(0f)] protected float _viewRadius = 8f;
        [SerializeField, Range(0f, 360f)] protected float _viewAngle = 70f;
        [SerializeField] protected Transform _origin;
        [SerializeField] protected Transform _forwardReference;

        [SerializeField] protected Cooldown _detectCooldown = new(0.1f);

        private readonly RaycastHit2D[] _hitsCache = new RaycastHit2D[16];

        public LayerMask TargetLayer => _targetLayer;
        public LayerMask ObstacleLayer => _obstacleLayer;
        public LayerMask SeeLayer => _targetLayer | _obstacleLayer;

        public float ViewRadius
        {
            get => _viewRadius;
            set => _viewRadius = Mathf.Max(value, 0f);
        }

        public float ViewAngle
        {
            get => _viewAngle;
            set => _viewAngle = Mathf.Clamp(value, 0f, 360f);
        }

        public Transform Origin
        {
            get => _origin;
            set => _origin = value;
        }

        public Transform ForwardReference
        {
            get => _forwardReference;
            set => _forwardReference = value;
        }

        public Vector2 OriginPosition => _origin != null ? _origin.position : transform.position;
        public Vector2 ForwardDirection => _forwardReference != null ? _forwardReference.up : transform.up;
        public abstract bool HasTarget { get; } 

        public virtual void UpdateDetection()
        {
            _detectCooldown.Update(Time.deltaTime);
            if (!_detectCooldown.IsComplete) return;
            _detectCooldown.Reset();

            this.InternalUpdate();
        }

        protected abstract void InternalUpdate();
        public abstract void ResetTarget();

        /// <summary>
        ///     Determines whether a target position is within the field of view.
        /// </summary>
        public virtual bool IsInViewAngle(Vector2 position)
        {
            Vector2 directionToTarget = (position - OriginPosition).normalized;
            float angleToTarget = Vector2.Angle(ForwardDirection, directionToTarget);
            return angleToTarget <= _viewAngle * 0.5f;
        }

        /// <summary>
        ///     Checks if there is a clear line of sight to the target using cached ray hits.
        /// </summary>
        public virtual bool HasLineOfSight(Transform target)
        {
            if (target == null) return false;

            Vector2 targetPos = target.position;
            Vector2 direction = targetPos - OriginPosition;
            float distance = direction.magnitude;
            direction.Normalize();

            int hitCount = Physics2D.RaycastNonAlloc(OriginPosition, direction, _hitsCache, distance, SeeLayer);

            for (int i = 0; i < hitCount; i++)
            {
                var hit = _hitsCache[i];
                if (hit.collider == null) continue;

                // If an obstacle is hit before target, so target is blocked
                if (LayerUtils.IsInLayerMask(hit.collider, _obstacleLayer)) return false;

                // If we reach the target first, so target is visible
                if (hit.collider.transform == target) return true;
            }

            return false; // No line of sight found
        }


#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            float halfAngle = _viewAngle / 2f;
            Vector3 normal = Vector3.forward;

            Handles.color = new Color(0f, 1f, 1f, 0.02f);
            Handles.DrawSolidArc(
                OriginPosition,
                normal,
                Quaternion.Euler(0, 0, -halfAngle) * ForwardDirection,
                _viewAngle,
                _viewRadius
            );

            // Draw forward
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(OriginPosition, OriginPosition + (ForwardDirection * _viewRadius));

            // Draw Boundary
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(OriginPosition, OriginPosition + (Vector2)(Quaternion.Euler(0, 0, halfAngle) * ForwardDirection) * _viewRadius);
            Gizmos.DrawLine(OriginPosition, OriginPosition + (Vector2)(Quaternion.Euler(0, 0, -halfAngle) * ForwardDirection) * _viewRadius);
            Gizmos.DrawWireSphere(OriginPosition, _viewRadius);
        }
#endif
    }
}
