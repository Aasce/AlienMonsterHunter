using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.VFXs
{
    [RequireComponent(typeof(VFXObject))]
    public class MoveToDestinationVFXObject : GameComponent
    {
        [SerializeField, Readonly] private VFXObject _vfx;

        [Space]
        [SerializeField] private bool _destinationIsTransform = false;
        [SerializeField] private Transform _destination;
        [SerializeField] private Vector2 _toPosition;

        [Space]
        [SerializeField] private float _speed = 10f;

        [Space]
        [SerializeField] private bool _isDespawnOnReachDestination = true;

        /// <summary>Invoked when object reaches final destination.</summary>
        public event Action OnReachingDestination;

        private bool _isMoving;


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _vfx);
        }


        private void LateUpdate()
        {
            if (!_isMoving) return;

            Vector2 current = transform.position;
            Vector2 target;

            // Determine destination
            if (_destinationIsTransform)
            {
                if (_destination == null) return;
                target = _destination.position;
            }
            else target = _toPosition;

            // Move
            transform.position = Vector2.MoveTowards(
                current,
                target,
                _speed * Time.deltaTime
            );

            // Check reaching destination
            if (Vector2.Distance(transform.position, target) <= 0.01f)
            {
                _isMoving = false;
                OnReachingDestination?.Invoke();
                OnReachingDestination = null;

                if (_isDespawnOnReachDestination)
                {
                    _vfx.DespawnCooldown.ToComplete();
                }
            }
        }


        /// <summary> Move from start position to a target Transform. </summary>
        public void StartMoveToDestination(Vector2 from, Transform destination)
        {
            transform.position = from;

            _destinationIsTransform = true;
            _destination = destination;
            _isMoving = true;
        }

        /// <summary> Move from start position to a fixed world position. </summary>
        public void StartMoveTo(Vector2 from, Vector2 to)
        {
            transform.position = from;

            _destinationIsTransform = false;
            _toPosition = to;
            _isMoving = true;
        }
    }
}
