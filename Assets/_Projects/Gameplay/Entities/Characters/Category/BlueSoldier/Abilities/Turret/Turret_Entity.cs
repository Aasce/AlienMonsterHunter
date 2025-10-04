using Asce.Game.Entities.Enemies;
using Asce.Game.FOVs;
using Asce.Game.Stats;
using Asce.Game.VFXs;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class Turret_Entity : Entity
    {
        [SerializeField] private FieldOfView _fov;

        [Space]
        [SerializeField] private Transform _weapon;
        [SerializeField] private Transform _barriel;


        [Header("Detection")]
        [SerializeField] private LayerMask _enemylayer;
        [SerializeField] private LayerMask _seelayer;
        [SerializeField] private float _viewRadius = 8f;
        [SerializeField] private Cooldown _detectCooldown = new(0.1f);
        [SerializeField] private ITargetable _target;

        [Header("Attack")]
        [SerializeField] private float _damage = 10f;
        [SerializeField] private Cooldown _attackCooldown = new(1f);

        [Header("VFXs")]
        [SerializeField] private string _bulletLineVFXName;

        protected override void Start()
        {
            base.Start();
            if (_fov != null) _fov.ViewRadius = _viewRadius;
        }

        private void Update()
        {
            this.FindTargetHandle();
            this.AttackTargetHandle();
        }


        private void LateUpdate()
        {
            if (_fov != null)
            {
                _fov.DrawFieldOfView();
            }
        }

        private void FindTargetHandle()
        {
            _detectCooldown.Update(Time.deltaTime);
            if (_detectCooldown.IsComplete)
            {
                _detectCooldown.Reset();
                if (_target != null)
                {
                    Vector2 direction = _target.transform.position - transform.position;
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, _viewRadius, _seelayer);
                    if (hit.transform != _target.transform) _target = null;

                }
                else
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _viewRadius, _enemylayer);
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider == null) continue;
                        Vector2 direction = collider.transform.position - transform.position;
                        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, _viewRadius, _seelayer);
                        if (hit.collider != collider) continue;
                        if (!collider.TryGetComponent(out Enemy enemy)) continue;

                        _target = enemy;
                        break;
                    }
                }
            }
        }

        private void AttackTargetHandle()
        {
            _attackCooldown.Update(Time.deltaTime);
            if (_attackCooldown.IsComplete)
            {
                _attackCooldown.Reset();
                if (_target == null) return;
                Vector2 direction = _target.transform.position - transform.position;

                if (_weapon != null)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                    _weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                }

                Vector2 shootPosition = _barriel != null ? _barriel.transform.position : transform.position;
                RaycastHit2D hit = Physics2D.Raycast(shootPosition, direction.normalized, _viewRadius, _seelayer);
                this.SpawnVFX(shootPosition, hit.collider != null ? hit.point : shootPosition + direction.normalized * _viewRadius);

                if (hit.collider == null) return;
                if (hit.transform != _target.transform) return;

                CombatController.Instance.DamageDealing(_target as ITakeDamageable, _damage);
            }
        }

        private void SpawnVFX(Vector2 startPoint, Vector2 endPoint)
        {
            LineVFXObject line = VFXController.Instance.Spawn(_bulletLineVFXName, startPoint) as LineVFXObject;
            if (line == null) return;
            if (line.LineRenderer == null) return;

            line.LineRenderer.positionCount = 2;
            line.LineRenderer.SetPosition(0, startPoint);
            line.LineRenderer.SetPosition(1, endPoint);
        }
    }
}
