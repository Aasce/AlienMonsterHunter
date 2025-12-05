using Asce.Core.Attributes;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class CharacterAbility : Ability
    {
        [Header("Range Settings")]
        [SerializeField, Readonly] protected float _useRangeRadius = 10f;

        [Header("Raycast Settings")]
        [SerializeField] protected LayerMask _obstacleMask;   // Layers considered as obstacles
        [SerializeField] protected float _raycastRadius = 0.1f; // Radius for circle cast
        [SerializeField] protected bool _useCircleCast = true;  // Use CircleCast instead of LineCast

        public float UseRangeRadius => _useRangeRadius;

        public override void Initialize()
        {
            base.Initialize();
            _useRangeRadius = Information.UseRangeRadius;
        }

        public virtual void SetPosition(Vector2 position)
        {
            transform.position = FindValidPosition(position);
        }

        /// <summary> Finds the closest valid position within range and not blocked by obstacles. </summary>
        protected virtual Vector2 FindValidPosition(Vector2 position)
        {
            if (_owner == null) return position;

            Vector2 ownerPos = _owner.transform.position;
            Vector2 direction = position - ownerPos;
            float distance = Mathf.Min(direction.magnitude, _useRangeRadius);

            // Perform raycast or circle cast to detect obstacles
            RaycastHit2D hit;
            if (_useCircleCast) hit = Physics2D.CircleCast(ownerPos, _raycastRadius, direction.normalized, distance, _obstacleMask);
            else hit = Physics2D.Raycast(ownerPos, direction.normalized, distance, _obstacleMask);

            if (hit.collider != null)
            {
                // Hit an obstacle, clamp position slightly before the hit point
                return hit.point - direction.normalized * 0.1f;
            }

            // No obstacle, position within range
            return ownerPos + direction.normalized * distance;
        }
    }
}
