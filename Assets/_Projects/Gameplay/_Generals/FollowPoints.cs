using Asce.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game
{
    public class FollowPoints : GameComponent
    {
        [SerializeField] private List<Transform> _points = new();
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _arriveThreshold = 0.1f;

        private int _currentIndex = 0;

        private void Update()
        {
            if (_target == null) return;
            if (_points.Count <= 0) return;

            Transform point = _points[_currentIndex];

            // Move target toward the current point
            _target.position = Vector3.MoveTowards(
                _target.position,
                point.position,
                _speed * Time.deltaTime
            );

            float distance = Vector3.Distance(_target.position, point.position);
            if (distance <= _arriveThreshold)
            {
                _currentIndex = (_currentIndex + 1) % _points.Count;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_points == null || _points.Count == 0) return;

            Gizmos.color = Color.cyan;
            for (int i = 0; i < _points.Count; i++)
            {
                Transform p1 = _points[i];
                Transform p2 = _points[(i + 1) % _points.Count];

                if (p1 != null && p2 != null)  Gizmos.DrawLine(p1.position, p2.position);
                if (p1 != null) Gizmos.DrawSphere(p1.position, 0.1f);
            }
        }
#endif
    }
}
