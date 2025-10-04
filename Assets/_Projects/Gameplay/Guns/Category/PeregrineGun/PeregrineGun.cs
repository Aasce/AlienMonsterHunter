using Asce.Game.Entities.Enemies;
using Asce.Game.Stats;
using Asce.Game.VFXs;
using UnityEngine;

namespace Asce.Game.Guns
{
    public class PeregrineGun : Gun
    {
        [SerializeField] private float _distance = 100f;

        [Header("VFXs")]
        [SerializeField] private string _bulletLineVFXName;

        protected override void Shooting(Vector2 direction)
        {
            CurrentAmmo--;

            // Get all hits along the ray
            RaycastHit2D[] hits = Physics2D.RaycastAll(BarrelPosition, direction, _distance, _hitLayer);
            if (hits.Length == 0)
            {
                // No hit, spawn VFX till max distance
                SpawnVFX(BarrelPosition, BarrelPosition + direction.normalized * _distance);
                return;
            }

            Vector2 endPoint = BarrelPosition + direction.normalized * _distance;

            foreach (var hit in hits)
            {
                // Check if this collider is an Enemy
                if (hit.transform.TryGetComponent(out Enemy enemy))
                {
                    CombatController.Instance.DamageDealing(enemy, Damage);
                    continue; // Continue to next hit (piercing)
                }

                // If not an Enemy, stop here
                endPoint = hit.point;
                break;
            }

            // Spawn the bullet trail from gun barrel to end point
            SpawnVFX(BarrelPosition, endPoint);
        }

        private void SpawnVFX(Vector2 startPoint, Vector2 endPoint)
        {
            LineVFXObject line = VFXController.Instance.Spawn(_bulletLineVFXName, startPoint) as LineVFXObject;
            if (line == null || line.LineRenderer == null) return;

            line.LineRenderer.positionCount = 2;
            line.LineRenderer.SetPosition(0, startPoint);
            line.LineRenderer.SetPosition(1, endPoint);
        }
    }
}
