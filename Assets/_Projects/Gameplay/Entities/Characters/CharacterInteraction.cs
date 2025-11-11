using Asce.Game.Interactions;
using Asce.Managers;
using Asce.Managers.Attributes;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    public class CharacterInteraction : GameComponent
    {
        [SerializeField, Readonly] private Character _character;

        [Header("Interaction Settings")]
        [SerializeField] private float _interactionSearchRadius = 2f;
        [SerializeField] private LayerMask _interactionLayerMask;

        [Header("Runtime")]
        [SerializeField, Readonly] private InteractiveObject _focusInteractiveObject = null;

        // Cached results to avoid GC alloc each frame
        private readonly Collider2D[] _overlapResults = new Collider2D[8];
        private ContactFilter2D _contactFilter;

        public event Action<InteractiveObject> OnFocusNewObject;
        public event Action<InteractiveObject> OnInteracted;

        public Character Character
        {
            get => _character;
            set => _character = value;
        }

        public InteractiveObject FocusInteractiveObject
        {
            get => _focusInteractiveObject;
            protected set
            {
                if (_focusInteractiveObject == value) return;
                _focusInteractiveObject = value;
                OnFocusNewObject?.Invoke(_focusInteractiveObject);
            }
        }

        public void Initialize()
        {
            _contactFilter = new ContactFilter2D
            {
                layerMask = _interactionLayerMask,
                useLayerMask = true
            };
        }

        private void Update()
        {
            this.Focus(_character.LookPosition);
        }

        /// <summary>
        ///     Finds the nearest valid InteractiveObject around the given world position.
        ///     Uses OverlapCircleNonAlloc to avoid GC allocations.
        /// </summary>
        /// <param name="position">The world position to search from.</param>
        private void Focus(Vector2 position)
        {
            int count = Physics2D.OverlapCircle(
                position,
                _interactionSearchRadius,
                _contactFilter,
                _overlapResults
            );

            InteractiveObject closestInteractiveObject = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Collider2D collider = _overlapResults[i];
                if (collider == null || !collider.enabled) continue;
                if (!collider.TryGetComponent(out InteractiveObject interactiveObject)) continue;
                if (!interactiveObject.IsInteractable) continue;

                float characterToObjectDistance = Vector2.Distance(
                    _character.transform.position,
                    collider.ClosestPoint(_character.transform.position)
                );

                if (characterToObjectDistance > interactiveObject.InteractDistance) continue;

                Vector2 closestPointOnCollider = collider.ClosestPoint(position);
                float positionToObjectDistance = Vector2.Distance(position, closestPointOnCollider);
                if (positionToObjectDistance < closestDistance)
                {
                    closestDistance = positionToObjectDistance;
                    closestInteractiveObject = interactiveObject;
                }
            }

            FocusInteractiveObject = closestInteractiveObject;
        }

        /// <summary>
        ///     Attempts to interact with the currently focused InteractiveObject.
        ///     Calls Focus() first to update the target if necessary.
        /// </summary>
        public void Interact()
        {
            if (FocusInteractiveObject == null) return;
            FocusInteractiveObject.Interact(_character.gameObject);
            OnInteracted?.Invoke(FocusInteractiveObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _interactionSearchRadius);
        }
#endif
    }
}
