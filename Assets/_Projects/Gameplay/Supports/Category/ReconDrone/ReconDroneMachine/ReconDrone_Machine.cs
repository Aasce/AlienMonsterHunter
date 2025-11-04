using Asce.Game.FOVs;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.VFXs;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class ReconDrone_Machine : Machine
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private FieldOfView _fovSelf;
        [SerializeField] private ParticleSystem _checkRadiusVFX;

        [Header("Runtime")]
        [SerializeField, Readonly] private Vector2 _destination;
        [SerializeField] private float _slowDownRadius = 2f;
        [SerializeField] private float _idleRotationSpeed = 30f; // degrees per second

        [Header("Check")]
        [SerializeField, Readonly] private Cooldown _checkCooldown = new(2f);
        [SerializeField, Readonly] private float _checkRadius = 10f;
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private string _checkVFXName = "";

        public Rigidbody2D Rigidbody => _rigidbody;
        public FieldOfView FovSelf => _fovSelf; 
        public Vector2 Destination
        {
            get => _destination;
            set => _destination = value;
        }
        public float CheckRadius
        {
            get => _checkRadius;
            protected set
            {
                _checkRadius = value;
                var mainModule = _checkRadiusVFX.main;
                mainModule.startSize = _checkRadius * 2f;
                _checkRadiusVFX.Clear();
                _checkRadiusVFX.Play();
            }
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _rigidbody);
            this.LoadComponent(out _fovSelf);
        }

        public override void Initialize()
        {
            base.Initialize();
            CheckRadius = Information.Stats.GetCustomStat("CheckRadius");
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            CheckRadius = Information.Stats.GetCustomStat("CheckRadius");
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            CheckRadius = Information.Stats.GetCustomStat("CheckRadius");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("CheckRadius", out LevelModification checkRadiusModification))
            {
                CheckRadius += checkRadiusModification.Value;
            }
        }

        private void Update()
        {
            _checkCooldown.Update(Time.deltaTime);
            if (_checkCooldown.IsComplete)
            {
                _checkCooldown.Reset();
                this.Checking();
            }
        }

        private void FixedUpdate()
        {
            Vector2 currentPosition = _rigidbody.position;
            Vector2 direction = _destination - currentPosition;
            float distance = direction.magnitude;

            float baseSpeed = Stats.Speed.FinalValue;

            // Rotation
            if (distance > 0.1f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                _rigidbody.MoveRotation(angle);
            }
            else
            {
                _rigidbody.MoveRotation(_rigidbody.rotation + _idleRotationSpeed * Time.fixedDeltaTime);
            }

            // Movement
            if (distance > 0.1f)
            {
                float speedMultiplier = Mathf.Clamp01(distance / _slowDownRadius);
                float adjustedSpeed = baseSpeed * speedMultiplier;

                _rigidbody.linearVelocity = direction.normalized * adjustedSpeed;
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

        private void Checking()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, CheckRadius, _targetLayer);
            foreach (Collider2D collider in colliders)
            {
                if (!collider.enabled) continue;
                // if (!collider.TryGetComponent(out ITargetable target)) continue;

                VFXController.Instance.Spawn(_checkVFXName, collider.transform.position);
            }
        }

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("Destination", _destination);
            data.SetCustom("CheckRadius", CheckRadius);
            data.SetCustom("CheckCooldown", _checkCooldown.CurrentTime);

        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _destination = data.GetCustom<Vector2>("Destination");
            CheckRadius = data.GetCustom<float>("CheckRadius");
            _checkCooldown.CurrentTime = data.GetCustom<float>("CheckCooldown");
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, CheckRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_destination, 0.2f);
            Gizmos.DrawLine(transform.position, _destination);
        }
#endif
    }
}
