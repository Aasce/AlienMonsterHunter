using Asce.Game.Effects;
using Asce.Game.Entities;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.VFXs;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class ElectricTrap_Ability : CharacterAbility
    {
        [SerializeField, Readonly] private CircleCollider2D _collider;
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;
        [SerializeField, Readonly] private Vector2 _moveDirection;

        [Header("Force Settings")]
        [Tooltip("X = MinDistance, Y = MaxDistance")]
        [SerializeField] private Vector2 _distanceRange = new(5f, 10f);

        [Tooltip("X = MinForce, Y = MaxForce")]
        [SerializeField] private Vector2 _forceRange = new(8f, 18f);
        [SerializeField] private float _torque = 10f;

        [Header("Catch")]
        [SerializeField, Readonly] private float _catchRadius = 5f;
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField, Readonly] private float _detentionDuration = 2f;

        [Header("Runtime")]
        [SerializeField, Readonly] private bool _catched = false;
        [SerializeField, Readonly] private Cooldown _catchCooldown = new(2f);

        [Space]
        [SerializeField] private string _electricVFXName = "Electric Trap Electric";


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _collider);
            this.LoadComponent(out _rigidbody);
        }

        public override void Initialize()
        {
            base.Initialize();
            _catchRadius = Information.GetCustomValue("CatchRadius");
            _detentionDuration = Information.GetCustomValue("DetentionDuration");
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            this.SetCatched(false);
            _catchCooldown.Reset(); 
        }

        public override void OnActive()
        {
            base.OnActive();

            float force;
            float distance = _moveDirection.magnitude;

            if (distance <= _distanceRange.x) force = _forceRange.x;
            else if (distance >= _distanceRange.y) force = _forceRange.y;
            else
            {
                float t = Mathf.InverseLerp(_distanceRange.x, _distanceRange.y, distance);
                force = Mathf.Lerp(_forceRange.x, _forceRange.y, t);
            }

            // Apply movement
            _rigidbody.AddForce(_moveDirection.normalized * force, ForceMode2D.Impulse);
            _rigidbody.AddTorque(_torque, ForceMode2D.Impulse);
        }

        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);
            if (_owner == null) return;

            Vector2 direction = position - (Vector2)_owner.transform.position;
            _moveDirection = direction;
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _catchRadius = Information.GetCustomValue("CatchRadius");
            _detentionDuration = Information.GetCustomValue("DetentionDuration");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);

            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("CatchRadius", out LevelModification catchRadiusModification))
            {
                _catchRadius += catchRadiusModification.Value;
            }

            if (modificationGroup.TryGetModification("DetentionDuration", out LevelModification detentionDurationModification))
            {
                _detentionDuration += detentionDurationModification.Value;
            }

        }

        private void Update()
        {
            if (_catched) return;
            _catchCooldown.Update(Time.deltaTime);
            if (_catchCooldown.IsComplete)
            {
                this.SetCatched(true);
                this.Catching();
                this.SpawnVFX();
            }
        }

        private void Catching()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _catchRadius, _targetLayer);
            if (colliders.Length <= 0) return;

            Entity ownerEntity = Owner != null ? Owner.GetComponent<Entity>() : null;
            foreach (Collider2D collider in colliders)
            {
                if (!collider.enabled) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;
                if (target is not Entity entity) continue;

                EffectController.Instance.AddEffect("Electric Detention", ownerEntity, entity, new EffectData()
                {
                    Duration = _detentionDuration
                });
            }
        }

        private void SetCatched(bool isCatched)
        {
            _catched = isCatched;
            if (_catched)
            {
                _collider.isTrigger = true;

                _rigidbody.linearVelocity = Vector2.zero;
                _rigidbody.angularVelocity = 0f;
                _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            }
            else
            {
                _collider.isTrigger = false;
                _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        private void SpawnVFX()
        {
            VFXController.Instance.Spawn(_electricVFXName, transform.position);
        }

        protected override void OnBeforeSave(AbilitySaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("Catched", _catched);
            data.SetCustom("CatchCooldown", _catchCooldown.CurrentTime);
            data.SetCustom("MoveDirection", _moveDirection);
            data.SetCustom("LinearVelocity", _rigidbody.linearVelocity);

            data.SetCustom("CatchRadius", _catchRadius);
            data.SetCustom("DetentionDuration", _detentionDuration);
        }

        protected override void OnAfterLoad(AbilitySaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            this.SetCatched(data.GetCustom<bool>("Catched"));
            _catchCooldown.CurrentTime = data.GetCustom<float>("CatchCooldown");
            _moveDirection = data.GetCustom<Vector2>("MoveDirection");
            _rigidbody.linearVelocity = data.GetCustom<Vector2>("LinearVelocity");

            _catchRadius = data.GetCustom<float>("CatchRadius");
            _detentionDuration = data.GetCustom<float>("DetentionDuration");
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _catchRadius);
        }
#endif
    }
}
