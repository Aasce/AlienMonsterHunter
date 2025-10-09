using Asce.Game.Entities;
using System;
using UnityEngine;

namespace Asce.Game.AIs
{
    /// <summary>
    ///     Handles single target detection and tracking logic.
    /// </summary>
    public class SingleTargetDetection : BaseTargetDetection
    {
        [Header("Runtime State")]
        [SerializeField] private ITargetable _currentTarget;

        public event Action<ITargetable> OnTargetChanged;

        public ITargetable CurrentTarget
        {
            get => _currentTarget;
            set
            {
                if (_currentTarget == value) return;
                _currentTarget = value;
                OnTargetChanged?.Invoke(_currentTarget);
            }
        }

        /// <summary> Updates target detection periodically. </summary>
        protected override void InternalUpdate()
        {
            // Validate current target
            if (_currentTarget != null)
            {
                if (!IsTargetValid(_currentTarget)) CurrentTarget = null;
                return;
            }

            // Find new target
            ITargetable newTarget = FindVisibleTarget();
            if (newTarget != null) CurrentTarget = newTarget;
        }

        /// <summary> Clears the current target. </summary>
        public override void ResetTarget() => CurrentTarget = null;

        protected virtual ITargetable FindVisibleTarget()
        {
            if (_origin == null) return null;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(_origin.position, _viewRadius, _targetLayer);
            ITargetable nearestTarget = null;
            float nearestDist = float.MaxValue;

            foreach (Collider2D collider in colliders)
            {
                if (collider == null) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;
                if (!IsInViewAngle(target.transform.position)) continue;
                if (!HasLineOfSight(target.transform)) continue;

                float dist = Vector2.Distance(_origin.position, target.transform.position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearestTarget = target;
                }
            }

            return nearestTarget;
        }

        protected virtual bool IsTargetValid(ITargetable target)
        {
            if (target == null) return false;
            if (!target.IsTargetable) return false;
            if (_origin == null) return false;

            Vector2 direction = target.transform.position - _origin.position;

            if (direction.magnitude > _viewRadius)
                return false;

            if (!IsInViewAngle(target.transform.position))
                return false;

            if (!HasLineOfSight(target.transform))
                return false;

            return true;
        }
    }
}
