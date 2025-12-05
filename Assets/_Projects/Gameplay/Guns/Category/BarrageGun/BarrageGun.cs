using Asce.Game.Combats;
using Asce.Game.Entities;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Game.VFXs;
using Asce.Core.Attributes;
using Asce.Core.Utils;
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
        [SerializeField, Readonly] private Cooldown _firingCooldown = new(1f); 
        [SerializeField, Readonly] private float _fireAccelerationRate = 0.1f;
        [SerializeField, Readonly] private float _maxFireSpeed = 0.1f;

        [Header("VFXs")]
        [SerializeField] private string _bulletLineVFXName;

        public override Vector2 BarrelPosition
        {
            get
            {
                if (_barrels.Count <= 0) return base.BarrelPosition;
                Transform barrel = _barrels[_barrelIndex];
                return barrel != null ? (Vector2)barrel.position : base.BarrelPosition;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            _fireAccelerationRate = Information.GetCustomValue("FireAccelerationRate");
            _maxFireSpeed = Information.GetCustomValue("MaxFireSpeed");
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _fireAccelerationRate = Information.GetCustomValue("FireAccelerationRate");
            _maxFireSpeed = Information.GetCustomValue("MaxFireSpeed");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);

            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("FireAccelerationRate", out LevelModification fireAccelerationRateModification))
            {
                _fireAccelerationRate += fireAccelerationRateModification.Value;
            }

            if (modificationGroup.TryGetModification("MaxFireSpeed", out LevelModification maxFireSpeedModification))
            {
                _maxFireSpeed += maxFireSpeedModification.Value;
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

            _barrelIndex = (_barrelIndex + 1) % _barrels.Count;
            Vector2 barrelPosition = BarrelPosition;

            // Get all hits along the ray
            int count = Physics2D.RaycastNonAlloc(barrelPosition, direction, _cacheHits, _distance, _hitLayer);
            if (count == 0)
            {
                Vector2 hitNothingPosition = barrelPosition + direction.normalized * _distance;
                this.SpawnVFX(barrelPosition, hitNothingPosition);
                this.Hit(hitNothingPosition, direction);
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
            SpawnVFX(barrelPosition, endPoint);
            this.Hit(endPoint, direction);
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

        protected override void OnBeforeSave(GunSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("FireAccelerationRate", _fireAccelerationRate);
            data.SetCustom("MaxFireSpeed", _maxFireSpeed);
        }

        protected override void OnAfterLoad(GunSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _fireAccelerationRate = data.GetCustom<float>("FireAccelerationRate");
            _maxFireSpeed = data.GetCustom<float>("MaxFireSpeed");
        }
    }
}
