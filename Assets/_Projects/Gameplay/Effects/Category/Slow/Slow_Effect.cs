using Asce.Game.SaveLoads;
using Asce.Managers.Attributes;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class Slow_Effect : Effect
    {
        [SerializeField, Readonly] private string _statId = string.Empty;

        protected override void InternalApply()
        {
            _statId = Receiver.Stats.Speed.Add(-Strength, Stats.StatValueType.Ratio).Id;
        }

        protected override void InternalUnapply()
        {
            Receiver.Stats.Speed.RemoveById(_statId);
        }


        protected override void OnBeforeSave(EffectSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("SpeedStatId", _statId);
        }

        protected override void OnAfterLoad(EffectSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _statId = data.GetCustom<string>("SpeedStatId");
        }
    }
}
