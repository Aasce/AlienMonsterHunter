using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class Decay_Effect : Effect
    {
        [SerializeField, Readonly] private string _statId = string.Empty;
        protected override void InternalApply()
        {
            _statId = Receiver.Stats.Health.Add(-Strength, Stats.StatValueType.Ratio).Id;
        }

        protected override void InternalUnapply()
        {
            Receiver.Stats.Health.RemoveById(_statId);
        }

        public override void ApplyStack(EffectData data)
        {
            Strength *= (1 + data.Strength);
            Strength = Mathf.Clamp(Strength, 0, 0.99f);
            Receiver.Stats.Health.ModifyValue(_statId, -Strength);

            float largestDuration = Mathf.Max(Duration.BaseTime, data.Duration);
            Duration.SetBaseTime(largestDuration);

            base.ApplyStack(data);
        }


        protected override void OnBeforeSave(EffectSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("DecayStatId", _statId);
        }

        protected override void OnAfterLoad(EffectSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _statId = data.GetCustom<string>("DecayStatId");
        }
    }
}
