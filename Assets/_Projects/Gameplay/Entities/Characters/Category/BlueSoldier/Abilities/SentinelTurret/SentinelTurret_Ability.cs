using Asce.Game.Entities.Machines;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class SentinelTurret_Ability : CharacterAbility, IControlMachineAbility
    {
        [SerializeField] private SentinelTurret_Machine _machine;

        public SentinelTurret_Machine Machine => _machine;
        Machine IControlMachineAbility.Machine => _machine;

        protected override void Start()
        {
            base.Start();
            Machine.Initialize();
            Machine.OnDead += Machine_OnDead;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            if (Machine != null) Machine.ResetStatus();
        }

        private void Machine_OnDead()
        {
            this.DespawnTime.ToComplete();
        }
    }
}
