using Asce.Game.Abilities;
using Asce.Game.FOVs;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class Helicopter_Machine : Machine
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private FieldOfView _fovSelf;
        [SerializeField] private Transform _firePosition;

        [SerializeField, Readonly] private Vector2 _direction;

        [Space]
        [SerializeField] private string _fireAreaAbilityName = string.Empty;
        [SerializeField] private Cooldown _fireCooldown = new(1f);

        public Rigidbody2D Rigidbody => _rigidbody;
        public FieldOfView FovSelf => _fovSelf;
        public Vector2 FirePosition => _firePosition != null ? _firePosition.position : transform.position;
        public Vector2 Direction
        {
            get => _direction;
            set => _direction = value;
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _rigidbody);
            this.LoadComponent(out _fovSelf);
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

        }

        private void Update()
        {
            _fireCooldown.Update(Time.deltaTime);
            if (_fireCooldown.IsComplete)
            {
                _fireCooldown.Reset();
                Ability ability = AbilityController.Instance.Spawn(_fireAreaAbilityName, gameObject);
                if (ability == null) return;

                ability.Leveling.SetLevel(Leveling.CurrentLevel);
                ability.transform.position = FirePosition;
                ability.gameObject.SetActive(true);
                ability.OnActive();
            }
        }

        private void FixedUpdate()
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f;
            _rigidbody.MoveRotation(angle);
            if (Direction != Vector2.zero)
            {
                float speed = Stats.Speed.FinalValue;
                _rigidbody.linearVelocity = Direction.normalized * speed;
            }
            else
            {
                _rigidbody.linearVelocity = Vector2.zero;
            }
        }

        private void LateUpdate()
        {
            _fovSelf.DrawFieldOfView();
        }

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("Direction", _direction);
            data.SetCustom("FireCooldown", _fireCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _direction = data.GetCustom<Vector2>("Direction");
            _fireCooldown.CurrentTime = data.GetCustom<float>("FireCooldown");
        }
    }
}
