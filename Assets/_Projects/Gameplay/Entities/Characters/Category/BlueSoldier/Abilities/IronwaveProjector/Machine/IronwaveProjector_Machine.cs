using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Effects;
using Asce.Game.FOVs;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class IronwaveProjector_Machine : Machine
    {
        [Header("References")]
        [SerializeField] private FieldOfView _fov;
        [SerializeField] private ParticleSystem _buffRadiusVFX;

        [Header("Armor Buff")]
        [SerializeField, Readonly] private float _armorAmount = 50f;
        [SerializeField, Readonly] private float _buffRadius = 0f;
        [SerializeField] private LayerMask _buffTargetLayer;
        [SerializeField] private Cooldown _buffCooldown = new(0.25f);

        private readonly Collider2D[] _overlapResults = new Collider2D[16];
        private ContactFilter2D _contactFilter;

        public float BuffRadius
        {
            get => _buffRadius;
            protected set
            {
                _buffRadius = value;

                var mainModule = _buffRadiusVFX.main;
                mainModule.startSize = _buffRadius * 2f;
                _buffRadiusVFX.Clear();
                _buffRadiusVFX.Play();

                _fov.ViewRadius = _buffRadius + 1;
            }
        }

        protected override void RefReset()
        {
            base.RefReset();
        }

        public override void Initialize()
        {
            base.Initialize();
            _contactFilter = new ContactFilter2D
            {
                layerMask = _buffTargetLayer,
                useLayerMask = true,
                useTriggers = true,
            };
            _armorAmount = Information.Stats.GetCustomStat("ArmorAmount");
            BuffRadius = Information.Stats.GetCustomStat("BuffRadius");
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _armorAmount = Information.Stats.GetCustomStat("ArmorAmount");
            BuffRadius = Information.Stats.GetCustomStat("BuffRadius");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("ArmorAmount", out LevelModification armorAmountModification))
            {
                _armorAmount += armorAmountModification.Value;
            }

            if (modificationGroup.TryGetModification("BuffRadius", out LevelModification buffRadiusModification))
            {
                BuffRadius += buffRadiusModification.Value;
            }

        }

        private void Update()
        {
            _buffCooldown.Update(Time.deltaTime);
            if (_buffCooldown.IsComplete)
            {
                _buffCooldown.Reset();
                this.BuffHandle();
            }
        }


        private void LateUpdate()
        {
            _fov.DrawFieldOfView();
        }


        private void BuffHandle()
        {
            int count = Physics2D.OverlapCircle(transform.position, _buffRadius, _contactFilter, _overlapResults);
            for (int i = 0; i < count; i++)
            {
                Collider2D collider = _overlapResults[i];
                if (collider == null) continue;
                if (collider.transform == this.transform) continue;
                if (!collider.TryGetComponent(out ITargetable target)) continue;

                EffectController.Instance.AddEffect("Armor Buff", this, target as Entity, new EffectData()
                {
                    Duration = 3f,
                    Strength = _armorAmount,
                });
            }
        }

        protected override void OnBeforeSave(MachineSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("ArmorAmount", _armorAmount);
            data.SetCustom("BuffRadius", BuffRadius);
        }

        protected override void OnAfterLoad(MachineSaveData data)
        {
            base.OnAfterLoad(data);
            _armorAmount = data.GetCustom<float>("ArmorAmount");
            BuffRadius = data.GetCustom<float>("BuffRadius");
        }
    }
}
