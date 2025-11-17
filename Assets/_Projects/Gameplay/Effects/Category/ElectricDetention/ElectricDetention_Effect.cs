using Asce.Game.SaveLoads;
using Asce.Managers.Attributes;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class ElectricDetention_Effect : Effect
    {
        [SerializeField, Readonly] private string _unmoveableStatId = string.Empty;
        [SerializeField, Readonly] private string _unattackableStatId = string.Empty;

        protected override void InternalApply()
        {
            _unmoveableStatId = Receiver.Effects.Unmoveable.Add().Id;
            _unattackableStatId = Receiver.Effects.Unattackable.Add().Id;
        }

        protected override void InternalUnapply()
        {
            Receiver.Effects.Unmoveable.RemoveById(_unmoveableStatId);
            Receiver.Effects.Unattackable.RemoveById(_unattackableStatId);
        }


        protected override void OnBeforeSave(EffectSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("UnmoveableStatId", _unmoveableStatId);
            data.SetCustom("UnattackableStatId", _unattackableStatId);
        }

        protected override void OnAfterLoad(EffectSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _unmoveableStatId = data.GetCustom<string>("UnmoveableStatId");
            _unattackableStatId = data.GetCustom<string>("UnattackableStatId");
        }
    }
}
