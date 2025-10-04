using Asce.Game.Entities.Enemies;
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

        [Header("VFXs")]
        [SerializeField] private string _bulletLineVFXName;

        protected override void Shooting(Vector2 direction)
        {
            CurrentAmmo--;

            // Normalize the base direction
            direction.Normalize();


            // If only 1 bullet, fire straight
            if (_bulletPerShoot <= 1)
            {
                ShootSingle(direction);
                return;
            }

            // Calculate half of spread to distribute evenly around the main direction
            float halfSpread = _spreadAngle * 0.5f;

            // Fire multiple bullets
            for (int i = 0; i < _bulletPerShoot; i++)
            {
                // t in range [0,1]
                float t = _bulletPerShoot == 1 ? 0.5f : (float)i / (_bulletPerShoot - 1);
                float angleOffset = Mathf.Lerp(-halfSpread, halfSpread, t);

                // Rotate direction by offset
                Vector2 spreadDir = Quaternion.Euler(0, 0, angleOffset) * direction;

                ShootSingle(spreadDir);
            }
        }

        private void ShootSingle(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(BarrelPosition, direction, _distance, _hitLayer);

            // Spawn VFX for this bullet
            SpawnVFX(
                BarrelPosition,
                hit.collider != null ? hit.point : BarrelPosition + direction * _distance
            );

            if (hit.collider == null) return;
            if (!hit.transform.TryGetComponent(out Enemy enemy)) return;
            CombatController.Instance.DamageDealing(enemy, Damage);
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
