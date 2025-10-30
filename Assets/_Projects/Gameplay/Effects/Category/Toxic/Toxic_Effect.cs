using Asce.Game.Combats;
using Asce.Game.SaveLoads;
using Asce.Game.Stats;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class Toxic_Effect : Effect
    {
		[SerializeField] private Cooldown _damageCooldown = new(1f);
		public override void Initialize() 
		{
			base.Initialize();
			_damageCooldown.SetBaseTime(Information.GetCustomValue("DamageInterval"));
		}
		
        public override void Apply()
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

        public override void Unpply()
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
