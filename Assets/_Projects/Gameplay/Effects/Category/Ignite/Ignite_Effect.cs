using Asce.Game.Combats;
using Asce.Game.SaveLoads;
using Asce.Game.Stats;
using Asce.Managers.Utils;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
                CombatController.Instance.DamageDealing(new DamageContainer(Sender, Receiver)
                {
                    Damage = Strength
                });
			}
		}

        public override void Unpply()
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
