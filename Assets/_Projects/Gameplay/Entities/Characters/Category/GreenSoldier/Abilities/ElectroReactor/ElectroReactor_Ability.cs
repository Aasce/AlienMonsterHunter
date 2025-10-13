using Asce.Game.Entities.Machines;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class ElectroReactor_Ability : CharacterAbility, IControlMachineAbility
    {
        [SerializeField] private ElectroReactor_Machine _machine;

        public ElectroReactor_Machine Machine => _machine;
        Machine IControlMachineAbility.Machine => _machine;


        protected override void Start()
        {
            base.Start();
            Machine.Initialize();
            Machine.OnDead+= Machine_OnDead;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            Machine.ResetStatus();
        }

        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);
        }

        private void Machine_OnDead()
        {
            this.DespawnTime.ToComplete();
        }
    }
}
