using Asce.Game.SaveLoads;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class Root_Effect : Effect
    {
        [SerializeField] private string _statId = string.Empty;

        protected override void InternalApply()
        {
            _statId = Receiver.Effects.Unmoveable.Add().Id;
        }

        protected override void InternalUnapply()
        {
            Receiver.Effects.Unmoveable.RemoveById(_statId);
        }


        protected override void OnBeforeSave(EffectSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("RootStatId", _statId);
        }

        protected override void OnAfterLoad(EffectSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _statId = data.GetCustom<string>("RootStatId");
        }
    }
}
