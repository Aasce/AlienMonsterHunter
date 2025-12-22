using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Entities.Characters;
using UnityEngine;

namespace Asce.Game.Interactions
{
    [RequireComponent(typeof(ExpOrb_InteractiveObject))]
    public class ExpOrb_Attraction : GameComponent
    {
        [Header("Settings")]
        [SerializeField, Min(0f)] private float _detectRadius = 25f;
        [SerializeField, Min(0f)] private float _moveSpeed = 5f;
        [SerializeField, Min(0f)] private float _stopDistance = 0.2f;
        [SerializeField] private Cooldown _findTargetCooldown = new(1f);

        [Space]
        [SerializeField] private LayerMask _targetLayer;

        [Header("References")]
        [SerializeField, Readonly] private ExpOrb_InteractiveObject _expOrb;
        private Transform _target;
        private ContactFilter2D _filter;
        private Collider2D[] _overlapResults = new Collider2D[8];

        public Rigidbody2D Rigidbody => _expOrb.Rigidbody;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _expOrb);
        }

        private void Start()
        {
            _filter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = _targetLayer,
                useTriggers = true,
            };
        }

        private void Update()
        {
            if (_target == null || !_target.gameObject.activeInHierarchy)
            {
                _findTargetCooldown.Update(Time.deltaTime);
                if (_findTargetCooldown.IsComplete)
                {
                    this.FindTarget();
                    _findTargetCooldown.Reset();
                }
                return;
            }

            this.MoveTowardTarget();
        }

        private void FindTarget()
        {
            int count = Physics2D.OverlapCircle(transform.position, _detectRadius, _filter, _overlapResults);
            if (count <= 0) return;

            for(int i = 0; i < count; i++)
            {
                if (_overlapResults[i].transform.TryGetComponent(out Character character))
                {
                    _target = _overlapResults[i].transform;
                }
            }
        }

        private void MoveTowardTarget()
        {
            if (_target == null)
            {
                _expOrb.Rigidbody.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 dir = (_target.transform.position - transform.position);
            float distance = dir.magnitude;

            if (distance < _stopDistance)
            {
                _expOrb.Rigidbody.linearVelocity = Vector2.zero;
                return;
            }

            dir.Normalize();
            float speed = Mathf.Lerp(_moveSpeed * 0.5f, _moveSpeed * 2f, 1f - (distance / _detectRadius));
            Vector2 targetVelocity = dir * speed;

            // Lerp velocity for smooth movement
            _expOrb.Rigidbody.linearVelocity = Vector2.Lerp(
                _expOrb.Rigidbody.linearVelocity,
                targetVelocity,
                0.1f // Smooth factor, adjust between 0.05–0.2 for desired smoothness
            );
        }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectRadius);
        }
#endif
    }
}
