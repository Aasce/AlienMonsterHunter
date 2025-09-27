using Asce.Game.Entities.FOVs;
using Asce.Game.Guns;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Entities
{
    [RequireComponent(typeof(Rigidbody2D))] 
    public class Character : Entity
    {
        [Header("Character")]
        [SerializeField] private CircleCollider2D _collider;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private CharacterFOV _fov;
        [SerializeField] private Gun _gun;

        [Space]
        [SerializeField] private Vector2 _moveDirection = Vector2.zero;
        [SerializeField] private Vector2 _lookPosition = Vector2.zero;


        public CircleCollider2D Collider => _collider;
        public Rigidbody2D Rigidbody => _rigidbody;
        public CharacterFOV Fov => _fov;
        public Gun Gun => _gun;

        public Vector2 MoveDirection => _moveDirection;
        public Vector2 LookPosition => _lookPosition;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _collider);
            this.LoadComponent(out _rigidbody);
            this.LoadComponent(out _fov);
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

        public void Move(Vector2 direction)
        {
            _moveDirection = direction;
        }

        public void LookAt(Vector2 lookPosition)
        {
            _lookPosition = lookPosition;
        }

        public void Shoot()
        {
            if (Gun == null) return;
            Vector2 lookDirection = _lookPosition - (Vector2)transform.position;
            Gun.Shoot(lookDirection);
        }
    }
}