using Asce.Game.Entities;
using Asce.Game.Stats;
using Asce.Game.VFXs;
using UnityEngine;

namespace Asce.Game.Guns
{
    public class ScatterGun : Gun
    {
        [SerializeField] private float _distance = 20f;
        [SerializeField] private int _bulletPerShoot = 4;
        [SerializeField, Range(0f, 90f)] private float _spreadAngle = 15f; // total cone angle
        private readonly RaycastHit2D[] _cacheHits = new RaycastHit2D[8];

        [Header("VFXs")]
        [SerializeField] private string _bulletLineVFXName;

        protected override void Shooting(Vector2 direction)
        {
            CurrentAmmo--;
            direction.Normalize();
            // If only 1 bullet, fire straight
            if (_bulletPerShoot <= 1)
            {
                ShootSingle(direction);
                return;
            }

            float halfSpread = _spreadAngle * 0.5f;
            for (int i = 0; i < _bulletPerShoot; i++)
            {
                float t = _bulletPerShoot == 1 ? 0.5f : (float)i / (_bulletPerShoot - 1);
                float angleOffset = Mathf.Lerp(-halfSpread, halfSpread, t);
                Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * direction;
                this.ShootSingle(spreadDirection);
            }
        }

        private void ShootSingle(Vector2 direction)
        {
            // Get all hits along the ray
            int count = Physics2D.RaycastNonAlloc(BarrelPosition, direction, _cacheHits, _distance, _hitLayer);
            if (count == 0)
            {
                Vector2 hitNothingPosition = BarrelPosition + direction.normalized * _distance;
                this.SpawnVFX(BarrelPosition, hitNothingPosition);
                this.Hit(hitNothingPosition, direction);
                return;
            }

            Vector2 endPoint = BarrelPosition + direction.normalized * _distance;
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
            SpawnVFX(BarrelPosition, endPoint);
            this.Hit(endPoint, direction);
        }

        private void SpawnVFX(Vector2 startPoint, Vector2 endPoint)
        {
            LineVFXObject line = VFXController.Instance.Spawn(_bulletLineVFXName, startPoint) as LineVFXObject;
            if (line == null || line.LineRenderer == null)
                return;

            line.LineRenderer.positionCount = 2;
            line.LineRenderer.SetPosition(0, startPoint);
            line.LineRenderer.SetPosition(1, endPoint);
        }
    }
}
