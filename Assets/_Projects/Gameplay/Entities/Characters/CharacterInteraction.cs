using Asce.Game.Interactions;
using Asce.Managers;
using Asce.Managers.Attributes;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    public class CharacterInteraction : GameComponent
    {
        [SerializeField, Readonly] private Character _character;

        [Header("Interaction Settings")]
        [SerializeField] private float _interactionSearchRadius = 2f;
        [SerializeField] private LayerMask _interactionLayerMask;

        public Character Character
        {
            get => _character;
            set => _character = value;
        }

        public void Initialize() { }

        /// <summary>
        ///     Attempts to interact with the nearest valid InteractiveObject
        ///     around the specified world position.
        ///     Only objects within their own InteractDistance range
        ///     relative to the character can be interacted with.
        /// </summary>
        /// <param name="position">The world position where the player attempts to interact.</param>
        public void Interact(Vector2 position)
        {
            Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(
                position,
                _interactionSearchRadius,
                _interactionLayerMask
            );

            InteractiveObject closestInteractiveObject = null;
            float closestDistance = float.MaxValue;

            foreach (Collider2D collider in nearbyColliders)
            {
                if (!collider.enabled) continue;
                if (!collider.TryGetComponent(out InteractiveObject interactiveObject)) continue;
                if (!interactiveObject.IsInteractable) continue;

                // Calculate the actual contact point between the given position and this collider
                Vector2 closestPointOnCollider = collider.ClosestPoint(position);

                // Ensure the character is close enough to interact
                float characterToObjectDistance = Vector2.Distance(
                    _character.transform.position,
                    closestPointOnCollider
                );

                if (characterToObjectDistance > interactiveObject.InteractDistance) continue;
                float positionToObjectDistance = Vector2.Distance(position, closestPointOnCollider);
                if (positionToObjectDistance < closestDistance)
                {
                    closestDistance = positionToObjectDistance;
                    closestInteractiveObject = interactiveObject;
                }
            }

            if (closestInteractiveObject != null)
            {
                closestInteractiveObject.Interact(_character.gameObject);
            }
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
