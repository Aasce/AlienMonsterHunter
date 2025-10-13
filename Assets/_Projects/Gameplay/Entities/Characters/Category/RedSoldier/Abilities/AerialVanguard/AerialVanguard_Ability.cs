using Asce.Game.Entities.Characters;
using Asce.Game.Entities.Machines;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class AerialVanguard_Ability : CharacterAbility, IControlMachineAbility
    {
        [SerializeField] private AerialVanguard_Machine _machine;

        public AerialVanguard_Machine Machine => _machine;
        Machine IControlMachineAbility.Machine => _machine;


        protected override void Start()
        {
            base.Start();
            Machine.Initialize();
            Machine.OnDead += MachineHealth_OnDead;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            Machine.Owner = Owner.GetComponent<Character>();
            Machine.ResetStatus();
        }

        public override void SetPosition(Vector2 position)
        {
            Machine.Agent.Warp(FindValidPosition(position));
        }

        private void MachineHealth_OnDead()
        {
            this.DespawnTime.ToComplete();
        }
    }
}
