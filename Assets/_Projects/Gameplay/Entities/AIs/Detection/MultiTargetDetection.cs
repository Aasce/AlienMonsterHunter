using Asce.Game.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Asce.Game.AIs
{
    /// <summary>
    ///     Handles detection of multiple visible targets within range.
    /// </summary>
    public class MultiTargetDetection : BaseTargetDetection
    {
        [SerializeField] private List<ITargetable> _visibleTargets = new();
        private ReadOnlyCollection<ITargetable> _targetsReadonly;

        public ReadOnlyCollection<ITargetable> VisibleTargets => _targetsReadonly ??= _visibleTargets.AsReadOnly();

        /// <summary> Updates the list of visible targets. </summary>
        protected override void InternalUpdate()
        {
            this.FindTargets();
        }

        private void FindTargets()
        {
            this.ResetTarget();
            if (_origin == null) return;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(_origin.position, _viewRadius, _targetLayer);
            foreach (Collider2D collider in colliders)
            {
                if (collider == null) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;
                if (!IsInViewAngle(target.transform.position)) continue;
                if (!HasLineOfSight(target.transform)) continue;

                _visibleTargets.Add(target);
            }
        }

        public override void ResetTarget() => _visibleTargets.Clear();

#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.color = Color.red;
            foreach (var target in _visibleTargets)
            {
                if (target == null || target.transform == null)
                    continue;

                Vector3 targetPos = target.transform.position;
                Gizmos.DrawLine(OriginPosition, targetPos);
                Gizmos.DrawSphere(targetPos, 0.1f);
            }

            Handles.color = Color.red;
            Handles.Label(OriginPosition + Vector2.up, $"Visible Targets: {_visibleTargets.Count}");
        }
#endif
    }
}
