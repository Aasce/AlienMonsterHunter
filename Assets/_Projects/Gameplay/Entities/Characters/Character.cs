using Asce.Game.Guns;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.Stats;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    [RequireComponent(typeof(Rigidbody2D))] 
    public class Character : Entity, IUsableGun, ISaveable<CharacterSaveData>, ILoadable<CharacterSaveData>
    {
        [Header("Character")]
        [SerializeField, Readonly] private CircleCollider2D _collider;
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;
        [SerializeField, Readonly] private CharacterFOV _fov;
        [SerializeField, Readonly] private CharacterAbilities _abilities;
        [SerializeField, Readonly] private CharacterInteraction _interaction;
        [SerializeField, Readonly] private Gun _gun;

        [Space]
        [SerializeField] private Transform _weaponSlot;

        [Header("Realtime")]
        [SerializeField, Readonly] private Vector2 _moveDirection = Vector2.zero;
        [SerializeField, Readonly] private Vector2 _lookPosition = Vector2.zero;

        public event Action<Gun> OnGunChanged;

        public new SO_CharacterInformation Information => base.Information as SO_CharacterInformation;
        public new CharacterStats Stats => base.Stats as CharacterStats;
        public new ExpLeveling Leveling => base.Leveling as ExpLeveling;

        public CircleCollider2D Collider => _collider;
        public Rigidbody2D Rigidbody => _rigidbody;
        public CharacterFOV Fov => _fov;
        public CharacterAbilities Abilities => _abilities;
        public CharacterInteraction Interaction => _interaction;

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
            this.LoadComponent(out _abilities);
            this.LoadComponent(out _collider);
            this.LoadComponent(out _rigidbody);
            this.LoadComponent(out _fov);
            if (this.LoadComponent(out _interaction)) 
            {
                _interaction.Character = this;
            }
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

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            Stats.SelfViewRadius.Clear(StatSourceType.Levelup);
            Stats.ViewRadius.Clear(StatSourceType.Levelup);
            Stats.ViewAngle.Clear(StatSourceType.Levelup);
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("SelfViewRadius", out LevelModification selfViewRadiusModification))
            {
                Stats.SelfViewRadius.Add(selfViewRadiusModification.Value, selfViewRadiusModification.Type.ToStatType(), StatSourceType.Levelup);
            }

            if (modificationGroup.TryGetModification("ViewRadius", out LevelModification viewRadiusModification))
            {
                Stats.ViewRadius.Add(viewRadiusModification.Value, viewRadiusModification.Type.ToStatType(), StatSourceType.Levelup);
            }

            if (modificationGroup.TryGetModification("ViewAngle", out LevelModification viewAngleModification))
            {
                Stats.ViewAngle.Add(viewAngleModification.Value, viewAngleModification.Type.ToStatType(), StatSourceType.Levelup);
            }
        }

        private void FixedUpdate()
        {
            // Moving
            float speed = Stats.Speed.FinalValue;
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
            Abilities.Use(index, position);
        }

        public void Interact()
        {
            Interaction.Interact(_lookPosition);
        }

        CharacterSaveData ISaveable<CharacterSaveData>.Save()
        {
            EntitySaveData baseData = ((ISaveable<EntitySaveData>)this).Save();
            CharacterSaveData saveData = new();
            saveData.CopyFrom(baseData);

            if (Gun is ISaveable<GunSaveData> gunSaveable)
            {
                saveData.gun = gunSaveable.Save();
            }

            if (Abilities is ISaveable<CharacterAbilitiesSaveData> abilitiesSaveable)
            {
                saveData.abilities = abilitiesSaveable.Save();
            }

            return saveData;
        }

        void ILoadable<CharacterSaveData>.Load(CharacterSaveData data)
        {
            if (data == null) return;
            ((ILoadable<EntitySaveData>)this).Load(data);

            if (Gun is ILoadable<GunSaveData> gunLoadable)
            {
                gunLoadable.Load(data.gun);
            }

            if (Abilities is ILoadable<CharacterAbilitiesSaveData> abilitiesLoadable)
            {
                abilitiesLoadable.Load(data.abilities);
            }
        }
    }
}