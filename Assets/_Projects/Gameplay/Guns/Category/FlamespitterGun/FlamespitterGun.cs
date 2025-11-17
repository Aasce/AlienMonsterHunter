using Asce.Game.Combats;
using Asce.Game.Effects;
using Asce.Game.Entities;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;
using UnityEngine.VFX;

namespace Asce.Game.Guns
{
    public class FlamespitterGun : Gun
    {
        [SerializeField] VisualEffect _flameEffect;

        [Header("Flame Settings")]
        [SerializeField] private float _fireRadius = 10f;
        [SerializeField] private float _fireAngle = 30f;
        [SerializeField] private LayerMask _targetLayer;

        private readonly Collider2D[] _overlapResults = new Collider2D[16];
        private ContactFilter2D _contactFilter;

        [Header("Ignite Settings")]
        [SerializeField, Readonly] private float _igniteDuration = 5f;
        [SerializeField, Readonly] private float _igniteStrength = 3f;

        public override void Initialize()
        {
            base.Initialize();
            _igniteDuration = Information.GetCustomValue("IgniteDuration");
            _igniteStrength = Information.GetCustomValue("IgniteStrength");
            _contactFilter = new ContactFilter2D
            {
                layerMask = _targetLayer,
                useLayerMask = true,
                useTriggers = true,
            };
            
            _flameEffect.Stop();

            OnFireStart += FlamespitterGun_OnFireStart;
            OnFireEnd += FlamespitterGun_OnFireEnd;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
        }

        public override void OnDeactive()
        {
            base.OnDeactive();
            _flameEffect.Stop();
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _igniteDuration = Information.GetCustomValue("IgniteDuration");
            _igniteStrength = Information.GetCustomValue("IgniteStrength");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);

            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("IgniteDuration", out LevelModification igniteDurationModification))
            {
                _igniteDuration += igniteDurationModification.Value;
            }

            if (modificationGroup.TryGetModification("IgniteStrength", out LevelModification igniteStrengthModification))
            {
                _igniteStrength += igniteStrengthModification.Value;
            }
        }

        private void OnEnable()
        {
            _flameEffect.Stop();
        }

        protected override void Update()
        {
            base.Update();
            if (CurrentAmmo <= 0 || IsReloading)
            {
                _flameEffect.Stop();
            }
        }

        private void Ignite()
        {
            Vector3 origin = _barrel != null ? _barrel.position : transform.position;
            Vector2 forward = transform.up;

            int count = Physic2DUtils.OverlapConeNonAlloc(origin, _fireRadius, forward, _fireAngle, _contactFilter, _overlapResults);
            for (int i = 0; i < count; i++)
            {
                Collider2D collider = _overlapResults[i];
                if (collider == null) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;
                if (!target.IsTargetable) continue;

                CombatController.Instance.DamageDealing(new DamageContainer(Owner as ISendDamageable, target as ITakeDamageable)
                {
                    Damage = (target as ITakeDamageable).Health.FinalValue * Damage,
                    DamageType = DamageType.TrueDamage,
                });

                EffectController.Instance.AddEffect("Ignite", Owner as Entity, target as Entity, new EffectData()
                {
                    Duration = _igniteDuration,
                    Strength = _igniteStrength,
                });
            }
        }

        protected override void Shooting(Vector2 direction)
        {
            CurrentAmmo--;
            this.Ignite();
        }

        private void FlamespitterGun_OnFireStart()
        {
            _flameEffect.Play();
        }

        private void FlamespitterGun_OnFireEnd()
        {
            _flameEffect.Stop();
        }


        protected override void OnBeforeSave(GunSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("IgniteDuration", _igniteDuration);
            data.SetCustom("IgniteStrength", _igniteStrength);

        }

        protected override void OnAfterLoad(GunSaveData data)
        {
            base.OnAfterLoad(data);
            _igniteDuration = data.GetCustom<float>("IgniteDuration");
            _igniteStrength = data.GetCustom<float>("IgniteStrength");
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            Vector3 origin = _barrel != null ? _barrel.position : transform.position;
            Vector3 forward = transform.up.normalized;

            // Helper to draw spread edge lines for a given angle and distance
            void DrawSpreadEdges(float angleDegrees, float distance, Color col)
            {
                float angleLeft = -angleDegrees;
                float angleRight = angleDegrees;

                Vector3 leftDir = Quaternion.Euler(0f, 0f, angleLeft) * forward;
                Vector3 rightDir = Quaternion.Euler(0f, 0f, angleRight) * forward;

                Gizmos.color = col;
                Gizmos.DrawLine(origin, origin + leftDir * distance);
                Gizmos.DrawLine(origin, origin + rightDir * distance);
            }

            DrawSpreadEdges(_fireAngle, _fireRadius, Color.red);
            Gizmos.DrawWireSphere(origin, _fireRadius);
        }
#endif
    }
}
