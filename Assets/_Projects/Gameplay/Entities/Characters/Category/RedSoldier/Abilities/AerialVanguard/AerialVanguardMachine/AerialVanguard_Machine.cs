using Asce.Game.AIs;
using Asce.Game.Entities.Characters;
using Asce.Game.FOVs;
using Asce.Game.Stats;
using Asce.Game.VFXs;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Entities.Machines
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AerialVanguard_Machine : Machine
    {
        [Header("References")]
        [SerializeField] private FieldOfView _fov;
        [SerializeField] private SingleTargetDetection _targetDetection;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Character _owner;

        [Space]
        [SerializeField] private List<Transform> _barrels = new();
        private int _barrelIndex = 0;

        [Header("Movement Settings")]
        [SerializeField] private float _orbitRadius = 3f;
        [SerializeField] private float _maxFollowDistance = 10f;
        [SerializeField, Readonly] private float _targetOrbitDistance = 3f;

        [Header("Attack Settings")]
        [SerializeField, Readonly] private float _damage = 10f;
        [SerializeField] private Cooldown _attackCooldown = new(.2f);
        [SerializeField] private float _bulletDistance = 10f;
        private readonly RaycastHit2D[] _cacheHits = new RaycastHit2D[16];

        [Header("VFXs")]
        [SerializeField] private string _bulletLineVFXName;

        private Vector2 _ownerFocusPosition;

        // Properties
        public SingleTargetDetection TargetDetection => _targetDetection;
        public Cooldown AttackCooldown => _attackCooldown;
        public NavMeshAgent Agent => _agent;

        public Character Owner
        {
            get => _owner;
            set => _owner = value;
        }

        private Vector2 BarrelPosition
        {
            get
            {
                if (_barrels.Count <= 0) return transform.position;
                Transform barrel = _barrels[_barrelIndex];
                _barrelIndex = (_barrelIndex + 1) % _barrels.Count;
                return barrel != null ? (Vector2)barrel.position : (Vector2)transform.position;
            }
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _agent);
            if (this.LoadComponent(out _targetDetection))
            {
                _targetDetection.Origin = transform;
                _targetDetection.ForwardReference = transform;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            _damage = Information.Stats.GetCustomStat("Damage");
            _targetOrbitDistance = Information.Stats.GetCustomStat("TargetDistance");
            AttackCooldown.SetBaseTime(Information.Stats.GetCustomStat("AttackSpeed"));
            TargetDetection.ViewRadius = Information.Stats.GetCustomStat("AttackRange");
            _fov.ViewRadius = Information.Stats.GetCustomStat("ViewRadius");

            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            _agent.speed = Stats.Speed.FinalValue;

            Stats.Speed.OnFinalValueChanged += (oldValue, newValue) =>
            {
                _agent.speed = newValue;
            };
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            TargetDetection.ResetTarget();
        }

        private void Update()
        {
            TargetDetection.UpdateDetection();

            if (!TargetDetection.HasTarget)
                this.HandleOrbitAroundOwner();
            else
                this.HandleOrbitAroundTarget();

            this.AttackHandle();
        }

        private void LateUpdate()
        {
            _fov.DrawFieldOfView();
        }

        /// <summary>
        ///     Handles orbiting movement around the owner.
        ///     If far from owner, move directly toward them; otherwise orbit in a circle.
        /// </summary>
        private void HandleOrbitAroundOwner()
        {
            if (Owner == null || !Owner.gameObject.activeInHierarchy) return;

            // Update focus position based on owner's look direction
            Vector2 lookDirection = Owner.LookPosition - (Vector2)Owner.transform.position;

            if (lookDirection.magnitude > _maxFollowDistance)
                _ownerFocusPosition = (Vector2)Owner.transform.position + lookDirection.normalized * _maxFollowDistance;
            else _ownerFocusPosition = Owner.LookPosition;

            float distance = Vector2.Distance(transform.position, _ownerFocusPosition);

            if (distance > _orbitRadius)
            {
                // Too far, move toward focus position
                _agent.SetDestination(_ownerFocusPosition);
                RotateTowardVelocity();
            }
            else
            {
                // Within optimal distance, stop movement
                _agent.ResetPath();
            }
        }

        /// <summary>
        ///     Handles orbiting movement around the current target.
        ///     If far from target, move directly toward it; otherwise orbit in a circle.
        ///     While orbiting, always face toward the target.
        /// </summary>
        private void HandleOrbitAroundTarget()
        {
            ITargetable target = TargetDetection.CurrentTarget;
            if (target == null || !target.transform.gameObject.activeInHierarchy)
            {
                TargetDetection.ResetTarget();
                return;
            }

            Vector2 targetPos = target.transform.position;
            Vector2 currentPos = transform.position;
            Vector2 toTarget = targetPos - currentPos;
            float distance = toTarget.magnitude;

            // Always face toward target
            if (distance > 0.001f)
            {
                float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 10f);
            }

            if (distance > _targetOrbitDistance)
            {
                // Too far, move closer
                _agent.SetDestination(targetPos);
            }
            else if (distance < _targetOrbitDistance * 0.8f)
            {
                // Too close, move backward along the opposite direction
                Vector2 retreatPos = currentPos - toTarget.normalized * (_targetOrbitDistance - distance);
                if (NavMesh.SamplePosition(retreatPos, out NavMeshHit navHit, _targetOrbitDistance, NavMesh.AllAreas))
                    _agent.SetDestination(navHit.position);
            }
            else
            {
                // Within ideal range, stop movement
                _agent.ResetPath();
            }
        }



        /// <summary>
        ///     Rotates the machine toward its current movement direction.
        /// </summary>
        private void RotateTowardVelocity()
        {
            Vector2 velocity = _agent.velocity;
            if (velocity.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 10f);
            }
        }

        private void AttackHandle()
        {
            AttackCooldown.Update(Time.deltaTime);
            if (!AttackCooldown.IsComplete) return;
            if (!TargetDetection.HasTarget) return;

            AttackCooldown.Reset();

            Vector2 barrelPosition = BarrelPosition;
            Vector2 direction = (Vector2)TargetDetection.CurrentTarget.transform.position - barrelPosition;

            // Get all hits along the ray
            int count = Physics2D.RaycastNonAlloc(barrelPosition, direction, _cacheHits, _bulletDistance, TargetDetection.SeeLayer);
            if (count == 0)
            {
                SpawnVFX(barrelPosition, barrelPosition + direction.normalized * _bulletDistance);
                return;
            }

            float currentDamage = _damage;
            Vector2 endPoint = barrelPosition + direction.normalized * _bulletDistance;
            for (int i = 0; i < count; i++)
            {
                RaycastHit2D hit = _cacheHits[i];
                if (hit.transform.TryGetComponent(out ITargetable target))
                {
                    if (target.IsTargetable)
                    {
                        CombatController.Instance.DamageDealing(target as ITakeDamageable, currentDamage);
                    }
                    continue; // Continue to next hit (piercing)
                }

                // If not an Target, stop here
                endPoint = hit.point;
                break;
            }

            // Spawn the bullet trail from gun barrel to end point
            SpawnVFX(barrelPosition, endPoint);
        }

        private void SpawnVFX(Vector2 startPoint, Vector2 endPoint)
        {
            LineVFXObject line = VFXController.Instance.Spawn(_bulletLineVFXName, startPoint) as LineVFXObject;
            if (line == null || line.LineRenderer == null) return;

            line.LineRenderer.positionCount = 2;
            line.LineRenderer.SetPosition(0, startPoint);
            line.LineRenderer.SetPosition(1, endPoint);
        }
    }
}
