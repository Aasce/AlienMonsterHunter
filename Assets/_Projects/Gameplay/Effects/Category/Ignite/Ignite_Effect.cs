using Asce.Game.Stats;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Effects
{
    public class Ignite_Effect : Effect
    {
		[SerializeField] private Cooldown _igniteCooldown = new(0.5f);
		public override void Initialize() 
		{
			base.Initialize();
			_igniteCooldown.SetBaseTime(Information.GetCustomValue("IgniteInterval"));
		}
		
        public override void Apply()
        {
			
        }
		
		protected virtual void Update() 
		{
			_igniteCooldown.Update(Time.deltaTime);
			if (_igniteCooldown.IsComplete) 
			{
				_igniteCooldown.Reset();
				CombatController.Instance.DamageDealing(Receiver, Strength);
			}
		}

        public override void Unpply()
        {
			
        }
    }
}
