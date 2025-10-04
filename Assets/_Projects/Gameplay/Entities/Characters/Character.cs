using Asce.Game.Guns;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    [RequireComponent(typeof(Rigidbody2D))] 
    public class Character : Entity
    {
        [Header("Character")]
        [SerializeField] private CircleCollider2D _collider;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private CharacterFOV _fov;
        [SerializeField] private CharacterAbilities _abilities;
        [SerializeField] private Gun _gun;

        [Space]
        [SerializeField] private Transform _weaponSlot;

        [Space]
        [SerializeField] private Vector2 _moveDirection = Vector2.zero;
        [SerializeField] private Vector2 _lookPosition = Vector2.zero;

        public event Action<Gun> OnGunChanged;


        public CircleCollider2D Collider => _collider;
        public Rigidbody2D Rigidbody => _rigidbody;
        public CharacterFOV Fov => _fov;
        public CharacterAbilities Abilities => _abilities;
        public Gun Gun
        {
            get => _gun;
            set
            {
                if (_gun == value) return;
                _gun = value;
                if (_gun != null && _weaponSlot != null)
                {
                    _gun.transform.SetParent(_weaponSlot);
                    _gun.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
                }
                OnGunChanged?.Invoke(_gun);
            }
        }

        public Transform WeaponSlot => _weaponSlot;

        public Vector2 MoveDirection => _moveDirection;
        public Vector2 LookPosition => _lookPosition;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _collider);
            this.LoadComponent(out _rigidbody);
            this.LoadComponent(out _fov);
        }

        protected override void Start()
        {
            base.Start();
            if (Gun != null) Gun.Initialize();
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

        public void Reload()
        {
            if (Gun == null) return;
            Gun.Reload();
        }
    }
}