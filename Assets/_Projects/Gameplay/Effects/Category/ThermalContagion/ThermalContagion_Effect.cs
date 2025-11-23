using Asce.Game.Combats;
using Asce.Game.Entities;
using Asce.Game.SaveLoads;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class ThermalContagion_Effect : Effect
    {
        [SerializeField, Readonly] private Cooldown _igniteCooldown = new(1f);
        [SerializeField, Readonly] private float _maxStrengthAddPerStack = 5f;

        public override void Initialize()
        {
            base.Initialize();
            _igniteCooldown.SetBaseTime(Information.GetCustomValue("IgniteInterval"));
            _maxStrengthAddPerStack = Information.GetCustomValue("MaxStrengthAddPerStack");
        }

        protected override void InternalApply()
        {

        }

        public override void ApplyStack(EffectData data)
        {
            Strength += Mathf.Min(data.Strength, _maxStrengthAddPerStack);
            float largestDuration = Mathf.Max(Duration.BaseTime, data.Duration);
            Duration.SetBaseTime(largestDuration);

            base.ApplyStack(data);
        }

        protected virtual void Update()
        {
            _igniteCooldown.Update(Time.deltaTime);
            if (_igniteCooldown.IsComplete)
            {
                _igniteCooldown.Reset();
                CombatController.Instance.DamageDealing(new DamageContainer(Sender, Receiver)
                {
                    Damage = Strength
                });
            }
        }

        protected override void InternalUnapply()
        {

        }

        protected override void OnBeforeSave(EffectSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("IgniteCooldown", _igniteCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(EffectSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _igniteCooldown.CurrentTime = data.GetCustom<float>("IgniteCooldown");
        }

    }
}
