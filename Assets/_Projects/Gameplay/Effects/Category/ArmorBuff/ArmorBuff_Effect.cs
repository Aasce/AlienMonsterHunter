using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class ArmorBuff_Effect : Effect
    {
        [SerializeField, Readonly] private string _statId = string.Empty;
        protected override void InternalApply()
        {
            _statId = Receiver.Stats.Armor.Add(Strength, Stats.StatValueType.Flat).Id;
        }

        protected override void InternalUnapply()
        {
            Receiver.Stats.Armor.RemoveById(_statId);
        }
        public override void Reapply(EffectData data)
        {
            base.Reapply(data);
        }

        protected override void OnBeforeSave(EffectSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("ArmorBuffStatId", _statId);
        }

        protected override void OnAfterLoad(EffectSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _statId = data.GetCustom<string>("ArmorBuffStatId");
        }
    }
}
