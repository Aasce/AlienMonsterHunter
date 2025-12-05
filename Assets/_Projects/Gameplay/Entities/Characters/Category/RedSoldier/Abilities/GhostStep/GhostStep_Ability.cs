using Asce.Game.Combats;
using Asce.Game.Effects;
using Asce.Game.Entities;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class GhostStep_Ability : CharacterAbility
    {
        [Header("Buff")]
        [SerializeField, Readonly] private float _healAmount = 5f;
        [SerializeField, Readonly] private float _duration = 2f;
        [SerializeField, Readonly] private float _speedBuff = 0.4f;

        [Header("Runtime")]
        [SerializeField, Readonly] private Entity _ownerEntity;

        public override void Initialize()
        {
            base.Initialize();
            _healAmount = Information.GetCustomValue("HealAmount");
            _duration = Information.GetCustomValue("Duration");
            _speedBuff = Information.GetCustomValue("SpeedBuff");
        }

        public override void ResetStatus()
        {
            base.ResetStatus();

        }

        public override void OnActive()
        {
            base.OnActive();
            _ownerEntity = Owner != null ? Owner.GetComponent<Entity>() : null;
            if (_ownerEntity == null) return;

            CombatController.Instance.Healing(_ownerEntity, _healAmount);
            EffectController.Instance.AddEffect("Ghost Step", _ownerEntity, _ownerEntity, new()
            {
                Duration = _duration,
                Strength = _speedBuff,
            });
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _healAmount = Information.GetCustomValue("HealAmount");
            _duration = Information.GetCustomValue("Duration");
            _speedBuff = Information.GetCustomValue("SpeedBuff");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);

            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("HealAmount", out LevelModification healAmountModification))
            {
                _healAmount += healAmountModification.Value;
            }

            if (modificationGroup.TryGetModification("Duration", out LevelModification durationModification))
            {
                _duration += durationModification.Value;
            }

            if (modificationGroup.TryGetModification("SpeedBuff", out LevelModification speedBuffModification))
            {
                _speedBuff += speedBuffModification.Value;
            }
        }

        protected override void OnBeforeSave(AbilitySaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("HealAmount", _healAmount);
            data.SetCustom("Duration", _duration);
            data.SetCustom("SpeedBuff", _speedBuff);
        }

        protected override void OnAfterLoad(AbilitySaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _healAmount = data.GetCustom<float>("HealAmount");
            _duration = data.GetCustom<float>("Duration");
            _speedBuff = data.GetCustom<float>("SpeedBuff");
        }
    }
}
