using Asce.Game.Entities.Enemies;
using Asce.Core;
using Asce.Core.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.AIs
{
    /// <summary>
    ///     Makes an enemy patrol randomly within a given radius using NavMesh,
    ///     ensuring destinations are visible and accessible via a NavMeshQueryFilter.
    /// </summary>
    [AddComponentMenu("Asce/AI/Random Patrol")]
    public class RandomPatrol : GameComponent
    {
        [Header("References")]
        [SerializeField] private Enemy _enemy;

        [Header("Patrol Settings")]
        [SerializeField, Min(0f)] private float _patrolRadius = 8f;

        [Space]
        [SerializeField, Min(0f)] private Vector2 _waitTimeRange = Vector2.one;
        [SerializeField] private Cooldown _waitCooldown = new();
        [SerializeField] private bool _isWaiting;

        [Header("NavMesh Settings")]
        [Tooltip("Filter which NavMesh areas this enemy can walk on.")]
        [SerializeField] private int _navMeshAreaMask = NavMesh.AllAreas;

        [Header("Visibility Settings")]
        [Tooltip("How high above ground the visibility ray starts (prevents ground hits).")]
        [SerializeField] private float _eyeHeight = 0.5f;

        [Tooltip("Number of attempts to find a valid visible point.")]
        [SerializeField, Range(1, 10)] private int _maxSearchAttempts = 5;

        [Tooltip("How far before an obstacle the enemy should stop when the destination is blocked.")]
        [SerializeField, Range(0.1f, 1f)] private float _wallOffset = 0.3f;

        private NavMeshAgent _agent;
        private Transform _origin;
        private NavMeshQueryFilter _navFilter;

        public float RandomWaitTime => Random.Range(_waitTimeRange.x, _waitTimeRange.y);

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _enemy);
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _agent = _enemy.Agent;
            _origin = _enemy.transform;

            _navFilter = new NavMeshQueryFilter
            {
                agentTypeID = _agent.agentTypeID,
                areaMask = _navMeshAreaMask
            };
        }

        private void Update()
        {
            if (_enemy.TargetDetection.HasTarget) return;
            this.PatrolUpdate();
        }

        /// <summary>
        /// Handles random patrol logic frame by frame.
        /// </summary>
        private void PatrolUpdate()
        {
            if (_isWaiting)
            {
                _waitCooldown.Update(Time.deltaTime);
                if (_waitCooldown.IsComplete)
                {
                    _isWaiting = false;
                    MoveToRandomPoint();
                }
                return;
            }

            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                _isWaiting = true;
                _waitCooldown.SetBaseTime(RandomWaitTime);
            }
        }

        /// <summary>
        /// Sets a random patrol destination within the patrol radius.
        /// </summary>
        private void MoveToRandomPoint()
        {
            if (_enemy.Effects.Unmoveable.IsAffect) return;
            Vector3 destination = GetVisibleNavMeshPoint(_origin.position, _patrolRadius);
            _agent.SetDestination(destination);
        }

        /// <summary>
        /// Finds a random valid and visible NavMesh position within the radius.
        /// If the target point is blocked, moves to the position before the obstacle.
        /// </summary>
        private Vector3 GetVisibleNavMeshPoint(Vector3 center, float radius)
        {
            for (int i = 0; i < _maxSearchAttempts; i++)
            {
                Vector3 randomPos = center + (Vector3)(Random.insideUnitCircle * radius);

                if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, radius, _navFilter))
                {
                    if (TryGetVisibleAdjustedPosition(center, hit.position, out Vector3 adjusted))
                        return adjusted;
                }
            }

            // fallback: stay at current position
            return center;
        }

        /// <summary>
        /// Checks if the destination is visible; if blocked, returns the hit point before the wall.
        /// </summary>
        private bool TryGetVisibleAdjustedPosition(Vector3 from, Vector3 to, out Vector3 result)
        {
            Vector3 origin = from + Vector3.up * _eyeHeight;
            Vector3 target = to + Vector3.up * _eyeHeight;
            Vector3 direction = (target - origin).normalized;
            float distance = Vector3.Distance(origin, target);

            if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
            {
                // Blocked by something — adjust position before wall
                if (!hit.collider.CompareTag("Ground"))
                {
                    Vector3 adjustedPoint = hit.point - direction * _wallOffset;

                    // Try sample adjusted point on NavMesh
                    if (NavMesh.SamplePosition(adjustedPoint, out NavMeshHit adjustedHit, 0.5f, _navFilter))
                    {
                        result = adjustedHit.position;
                        return true;
                    }

                    // Could not sample adjusted point, fallback to hit point
                    result = hit.point;
                    return true;
                }
            }

            // No obstacle: target is directly visible
            result = to;
            return true;
        }
    }
}
