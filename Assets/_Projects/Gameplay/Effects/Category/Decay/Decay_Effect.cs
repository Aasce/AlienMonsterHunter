using Asce.Game.SaveLoads;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class Decay_Effect : Effect
    {
        [SerializeField] private string _statId = string.Empty;
        protected override void InternalApply()
        {
            _statId = Receiver.Stats.Health.Add(-Strength, Stats.StatValueType.Ratio).Id;
            foreach(Effect effect in Receiver.Effects.Effects)
            {
                if (effect is Decay_Effect decay)
                {
                    decay.Duration.SetBaseTime(Duration.BaseTime, isReset: true);
                }
            }
        }

        protected override void InternalUnapply()
        {
            Receiver.Stats.Health.RemoveById(_statId);
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
