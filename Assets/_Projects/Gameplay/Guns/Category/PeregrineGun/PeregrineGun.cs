using Asce.Game.Entities;
using Asce.Game.Stats;
using Asce.Game.VFXs;
using UnityEngine;

namespace Asce.Game.Guns
{
    public class PeregrineGun : Gun
    {
        [SerializeField] private float _distance = 100f;
        [SerializeField] private float _damageLosePerHit = 0.9f;
        private readonly RaycastHit2D[] _cacheHits = new RaycastHit2D[32];

        [Header("Peregrine Gun")]
        [SerializeField] private GunScope _scope;

        [Header("VFXs")]
        [SerializeField] private string _bulletLineVFXName;

        public GunScope Scope
        {
            get => _scope; 
            set
            {
                _scope = value;
                if (_scope != null) _scope.Gun = this;
            }
        }

        public override void AltFire(Vector2 direction)
        {
            base.AltFire(direction);
            _scope.Toggle();
        }

        protected override void Shooting(Vector2 direction)
        {
            CurrentAmmo--;

            // Get all hits along the ray
            int count = Physics2D.RaycastNonAlloc(BarrelPosition, direction, _cacheHits, _distance, _hitLayer);
            if (count == 0)
            {
                SpawnVFX(BarrelPosition, BarrelPosition + direction.normalized * _distance);
                return;
            }

            float currentDamage = Damage;
            Vector2 endPoint = BarrelPosition + direction.normalized * _distance;
            for(int i = 0; i < count; i++)
            {
                RaycastHit2D hit = _cacheHits[i];
                if (hit.transform.TryGetComponent(out ITargetable target))
                {
                    if (target.IsTargetable)
                    {
                        CombatController.Instance.DamageDealing(target as ITakeDamageable, currentDamage);
                        currentDamage *= _damageLosePerHit;
                    }
                    continue; // Continue to next hit (piercing)
                }

                // If not an Target, stop here
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
            line.LineRenderer.startWidth = .5f;
            line.LineRenderer.endWidth = .5f;
        }
    }
}
