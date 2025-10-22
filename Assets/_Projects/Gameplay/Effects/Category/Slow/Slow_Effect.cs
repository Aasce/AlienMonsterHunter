using Asce.Game.SaveLoads;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class Slow_Effect : Effect
    {
        [SerializeField] private string _statId = string.Empty;
        public override void Apply()
        {
            _statId = Receiver.Stats.Speed.Add(-Strength, Stats.StatValueType.Ratio).Id;
        }

        public override void Unpply()
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
