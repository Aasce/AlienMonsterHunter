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
				CombatController.Instance.DamageDealing(Receiver, maxDamage);
			}
		}

        public override void Unpply()
        {
			
        }
    }
}
