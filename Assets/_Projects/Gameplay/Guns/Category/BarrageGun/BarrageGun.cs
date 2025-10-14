using Asce.Game.Entities;
using Asce.Game.Stats;
using Asce.Game.VFXs;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Guns
{
    public class BarrageGun : Gun
    {
        [SerializeField] private float _distance = 20f;
        private readonly RaycastHit2D[] _cacheHits = new RaycastHit2D[16];

        [Header("Barrage Gun")]
        [SerializeField] private List<Transform> _barrels = new();
        [SerializeField, Readonly] private int _barrelIndex = 0;

        [Space]
        [SerializeField] private Cooldown _firingCooldown = new(1f); 
        [SerializeField] private float _fireAccelerationRate = 0.1f;
        [SerializeField] private float _maxFireSpeed = 0.1f;

        [Header("VFXs")]
        [SerializeField] private string _bulletLineVFXName;

        public new Vector2 BarrelPosition
        {
            get
            {
                if (_barrels.Count <= 0) return base.BarrelPosition;
                Transform barrel = _barrels[_barrelIndex];
                _barrelIndex = (_barrelIndex + 1) % _barrels.Count;
                return barrel != null ? (Vector2)barrel.position : base.BarrelPosition;
            }
        }

        protected override void Update()
        {
            base.Update();
            _firingCooldown.Update(Time.deltaTime);
            if (_firingCooldown.IsComplete)
            {
                ShootCooldown.SetBaseTime(Information.ShootSpeed, isReset: false);
            }
        }

        protected override void Shooting(Vector2 direction)
        {
            CurrentAmmo--;
            _firingCooldown.Reset();
            float newShootSpeed = ShootCooldown.BaseTime * (1f - _fireAccelerationRate);
            newShootSpeed = Mathf.Max(newShootSpeed, _maxFireSpeed);
            ShootCooldown.SetBaseTime(newShootSpeed);
            Vector2 barrelPosition = BarrelPosition;

            // Get all hits along the ray
            int count = Physics2D.RaycastNonAlloc(barrelPosition, direction, _cacheHits, _distance, _hitLayer);
            if (count == 0)
            {
                SpawnVFX(barrelPosition, barrelPosition + direction.normalized * _distance);
                return;
            }

            Vector2 endPoint = barrelPosition + direction.normalized * _distance;
            for (int i = 0; i < count; i++)
            {
                RaycastHit2D hit = _cacheHits[i];
                if (!hit.transform.TryGetComponent(out ITargetable target))
                {
                    endPoint = hit.point; // If not an Target, stop here
                    break;
                }
                if (target.IsTargetable)
                {
                    CombatController.Instance.DamageDealing(target as ITakeDamageable, Damage);
                    endPoint = hit.point;
                    break;
                }
            }

            // Spawn the bullet trail from gun barrel to end point
            SpawnVFX(barrelPosition, endPoint);
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
