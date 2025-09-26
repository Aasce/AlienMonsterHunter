using Asce.Game.Entities.FOVs;
using UnityEngine;

namespace Asce.Game.Entities
{
    [RequireComponent(typeof(Rigidbody2D))] 
    public class Character : Entity
    {
        [SerializeField] private CircleCollider2D _collider;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private CharacterFOV _fov;

        [Space]
        [SerializeField] private Vector2 _moveDirection = Vector2.zero;
        [SerializeField] private Vector2 _lookPosition = Vector2.zero;


        public CircleCollider2D Collider => _collider;
        public Rigidbody2D Rigidbody => _rigidbody;
        public CharacterFOV Fov => _fov;

        public Vector2 MoveDirection => _moveDirection;
        public Vector2 LookPosition => _lookPosition;


        public void Move(Vector2 direction)
        {
            _moveDirection = direction;
        }

        public void LookAt(Vector2 lookPosition)
        {
            _lookPosition = lookPosition;
        }

        private void FixedUpdate()
        {
            if (Rigidbody == null) return;

            // Moving
            float speed = 5f;
            Vector2 deltaPosition = _moveDirection.normalized * speed * Time.fixedDeltaTime;
            Rigidbody.MovePosition(Rigidbody.position + deltaPosition);

            // Looking
            Vector2 lookDirection = _lookPosition - Rigidbody.position;
            if (lookDirection.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
                Rigidbody.MoveRotation(targetRotation);
            }
        }

        private void LateUpdate()
        {
            if (Fov == null) return;
            Fov.DrawFieldOfView();
        }
    }
}