using Asce.Game.Entities.Enemies;
using Asce.Game.Stats;
using Asce.Game.VFXs;
using UnityEngine;

namespace Asce.Game.Guns
{
    public class DefaultGun : Gun
    {
        [SerializeField] private float _distance = 20f;

        [Header("VFXs")]
        [SerializeField] private string _bulletLineVFXName;

        protected override void Shooting(Vector2 direction)
        {
            CurrentAmmo--;

            RaycastHit2D hit = Physics2D.Raycast(BarrelPosition, direction, _distance, _hitLayer);
            this.SpawnVFX(BarrelPosition, hit.collider != null ? hit.point : BarrelPosition + direction.normalized * _distance);
            if (hit.collider == null) return;
            if (!hit.transform.TryGetComponent(out Enemy enemy)) return;
            
            CombatController.Instance.DamageDealing(enemy, Damage);
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
