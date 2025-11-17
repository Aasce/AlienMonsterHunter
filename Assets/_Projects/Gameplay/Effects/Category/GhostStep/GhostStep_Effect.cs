using Asce.Game.Entities;
using Asce.Game.SaveLoads;
using Asce.Managers.Attributes;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class GhostStep_Effect : Effect
    {
        [SerializeField, Readonly] private string _speedStatId = string.Empty;
        [SerializeField, Readonly] private string _untargetableStatId = string.Empty;

        protected override void InternalApply()
        {
            _speedStatId = Receiver.Stats.Speed.Add(Strength, Stats.StatValueType.Ratio).Id;
            _untargetableStatId = Receiver.Effects.Untargetable.Add().Id;

            float targetAlpha = Receiver.IsControlByPlayer() ? 0.5f : 0f;
            Receiver.View.SetAlpha(targetAlpha);
        }

        protected override void InternalUnapply()
        {
            Receiver.Stats.Speed.RemoveById(_speedStatId);
            Receiver.Effects.Untargetable.RemoveById(_untargetableStatId);
            if (!Receiver.Effects.Untargetable.IsAffect)
            {
                Receiver.View.SetAlpha(1f);
            }
        }


        protected override void OnBeforeSave(EffectSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("SpeedStatId", _speedStatId);
            data.SetCustom("UntargetableStatId", _untargetableStatId);
        }

        protected override void OnAfterLoad(EffectSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _speedStatId = data.GetCustom<string>("SpeedStatId");
            _untargetableStatId = data.GetCustom<string>("UntargetableStatId");
        }
    }
}
