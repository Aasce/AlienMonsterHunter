using Asce.Game.Combats;
using Asce.Game.Entities;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.VFXs;
using Asce.Managers.Attributes;
using UnityEngine;

namespace Asce.Game.Guns
{
    public class ScatterGun : Gun
    {
        [SerializeField] private float _distance = 20f;
        [SerializeField, Range(0f, 90f)] private float _spreadAngle = 15f; // total cone angle
        [SerializeField, Readonly] private int _bulletPerShoot = 4;
        private readonly RaycastHit2D[] _cacheHits = new RaycastHit2D[8];

        [Header("VFXs")]
        [SerializeField] private string _bulletLineVFXName;

        public override void Initialize()
        {
            base.Initialize();
            _bulletPerShoot = (int)Information.GetCustomValue("BulletPerShoot");
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _bulletPerShoot = (int)Information.GetCustomValue("BulletPerShoot");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);

            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("BulletPerShoot", out LevelModification bulletPerShootModification))
            {
                _bulletPerShoot += (int)bulletPerShootModification.Value;
            }
        }

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
                    CombatController.Instance.DamageDealing(new DamageContainer(Owner as ISendDamageable, target as ITakeDamageable)
                    {
                        Damage = Damage,
                        Penetration = Penetration,
                    });
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

        protected override void OnBeforeSave(GunSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("BulletPerShoot", _bulletPerShoot);
        }

        protected override void OnAfterLoad(GunSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _bulletPerShoot = data.GetCustom<int>("BulletPerShoot");
        }

    }
}
