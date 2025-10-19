using Asce.Game.Guns;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    [RequireComponent(typeof(Rigidbody2D))] 
    public class Character : Entity
    {
        [Header("Character")]
        [SerializeField, Readonly] private CircleCollider2D _collider;
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;
        [SerializeField, Readonly] private CharacterFOV _fov;
        [SerializeField, Readonly] private CharacterAbilities _abilities;
        [SerializeField, Readonly] private Gun _gun;

        [Space]
        [SerializeField] private Transform _weaponSlot;

        [Header("Realtime")]
        [SerializeField, Readonly] private Vector2 _moveDirection = Vector2.zero;
        [SerializeField, Readonly] private Vector2 _lookPosition = Vector2.zero;

        public event Action<Gun> OnGunChanged;

        public new SO_CharacterInformation Information => base.Information as SO_CharacterInformation;
        public new CharacterStats Stats => base.Stats as CharacterStats;

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
                if (_gun != null)
                {
                    _gun.Owner = this;
                    if (_weaponSlot != null)
                    {
                        _gun.transform.SetParent(_weaponSlot);
                        _gun.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
                    }
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

        public override void ResetStatus()
        {
            base.ResetStatus();
            Abilities.ResetStatus();
            if (Gun != null) Gun.ResetStatus();
        }

        public override void Initialize()
        {
            base.Initialize();
            Abilities.Initialize(this);
            if (Gun != null) Gun.Initialize();

            Fov.FovSelf.ViewRadius = Stats.SelfViewRadius.FinalValue;
            Fov.Fov.ViewRadius = Stats.ViewRadius.FinalValue;
            Fov.Fov.ViewAngle = Stats.ViewAngle.FinalValue;

            Stats.SelfViewRadius.OnFinalValueChanged += (oldValue, newValue) =>
            {
                Fov.FovSelf.ViewRadius = newValue;
            };

            Stats.ViewRadius.OnFinalValueChanged += (oldValue, newValue) =>
            {
                Fov.Fov.ViewRadius = newValue;
            };

            Stats.ViewAngle.OnFinalValueChanged += (oldValue, newValue) =>
            {
                Fov.Fov.ViewAngle = newValue;
            };
        }

        private void FixedUpdate()
        {
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

        public void Fire()
        {
            if (Gun == null) return;
            Vector2 lookDirection = _lookPosition - (Vector2)transform.position;
            Gun.Fire(lookDirection);
        }

        public void AltFire()
        {
            if (Gun == null) return;
            Vector2 lookDirection = _lookPosition - (Vector2)transform.position;
            Gun.AltFire(lookDirection);
        }

        public void Reload()
        {
            if (Gun == null) return;
            Gun.Reload();
        }

        public void UseAbility(int index, Vector2 position)
        {
            if (Abilities == null) return;
            Abilities.Use(index, position);
        }
    }
}