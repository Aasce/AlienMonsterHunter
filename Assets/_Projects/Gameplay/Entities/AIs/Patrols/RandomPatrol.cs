using Asce.Game.Entities.Enemies;
using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.AIs
{
    /// <summary>
    ///     Makes an enemy patrol randomly within a given radius using NavMesh, controlled by Cooldown.
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

        private NavMeshAgent _agent;
        private Transform _origin;


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

            // If reached destination, start waiting
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
            Vector2 destination = GetRandomNavMeshPoint(_origin.position, _patrolRadius);
            _agent.SetDestination(destination);
        }

        /// <summary>
        /// Finds a random valid NavMesh position within the radius.
        /// </summary>
        private Vector3 GetRandomNavMeshPoint(Vector2 center, float radius)
        {
            Vector2 randomPos = center + Random.insideUnitCircle * radius;
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return center;
        }
    }
}
