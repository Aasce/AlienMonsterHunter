using Asce.Game.Entities;
using Asce.Game.Stats;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class HorizontBreaker_Light_Ability : Ability
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private LineRenderer _lineFOV;

        [Header("Attack Settings")]
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private Cooldown _dealDamageCooldown = new(0.25f);

        [Space]
        [SerializeField, Min(0f)] private float _width = 2f;
        [SerializeField, Min(0f)] private float _distance = 15f;
        [SerializeField] private Vector2 _direction;

        public LayerMask TargetLayer => _targetLayer;
        public float Damage
        {
            get => _damage;
            set => _damage = value;
        }

        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                this.SetLineWidth();
            }
        }

        public float Distance
        {
            get => _distance;
            set => _distance = value;
        }

        public Vector2 Direction
        {
            get => _direction;
            set => _direction = value;
        }

        public override void OnActive()
        {
            base.OnActive();
            Vector2 endPosition = (Vector2)transform.position + _direction.normalized * _distance;
            this.SetLine(startPosition: transform.position, _direction.normalized);
            this.Fire();
        }

        private void Update()
        {
            if (Owner == null) this.DespawnTime.ToComplete();
            if (!Owner.activeInHierarchy) this.DespawnTime.ToComplete();
            else this.DespawnTime.Reset();

            _dealDamageCooldown.Update(Time.deltaTime);
            if (_dealDamageCooldown.IsComplete)
            {
                this.Fire();
                _dealDamageCooldown.Reset();
            }
        }

        private void Fire()
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll(
                transform.position,
                new Vector2(_width, 0.1f),
                0f,
                _direction,
                _distance,
                _targetLayer
            );

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null) continue;
                if (!hit.collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;

                CombatController.Instance.DamageDealing(target as ITakeDamageable, _damage);
            }
        }

        private void SetLineWidth()
        {
            if (_lineRenderer != null)
            {
                _lineRenderer.startWidth = _width * 1.5f;
                _lineRenderer.endWidth = _width * 1.5f;
            }
            if (_lineFOV != null)
            {
                _lineFOV.startWidth = _width * 2f;
                _lineFOV.endWidth = _width * 2f;
            }
        }

        private void SetLine(Vector2 startPosition, Vector2 direction)
        {
            if (_lineRenderer != null)
            {
                Vector2 endPosition = startPosition + direction * _distance;
                _lineRenderer.positionCount = 2;
                _lineRenderer.SetPosition(0, startPosition);
                _lineRenderer.SetPosition(1, endPosition);
            }
            if (_lineFOV != null)
            {
                Vector2 endPosition = startPosition + direction * _distance;
                _lineFOV.positionCount = 2;
                _lineFOV.SetPosition(0, startPosition);
                _lineFOV.SetPosition(1, endPosition);
            }
        }

    }
}
