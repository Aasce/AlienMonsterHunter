using Asce.Game.Combats;
using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class Toxic_Effect : Effect
    {
		[SerializeField, Readonly] private Cooldown _damageCooldown = new(1f);

		public override void Initialize() 
		{
			base.Initialize();
			_damageCooldown.SetBaseTime(Information.GetCustomValue("DamageInterval"));
		}

        protected override void InternalApply()
        {
			
        }
		
		protected virtual void Update() 
		{
			_damageCooldown.Update(Time.deltaTime);
			if (_damageCooldown.IsComplete) 
			{
				_damageCooldown.Reset();
				float currentHealth = Receiver.Stats.Health.CurrentValue;
				if (currentHealth <= 1f) return;
				float maxDamage = (currentHealth > Strength) ? Strength : currentHealth - 1f;
				CombatController.Instance.DamageDealing(new DamageContainer(Sender, Receiver)
				{
					Damage = maxDamage
                });
			}
		}

        protected override void InternalUnapply()
        {

        }

        protected override void OnBeforeSave(EffectSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("DamageCooldown", _damageCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(EffectSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _damageCooldown.CurrentTime = data.GetCustom<float>("DamageCooldown");
        }
    }
}
