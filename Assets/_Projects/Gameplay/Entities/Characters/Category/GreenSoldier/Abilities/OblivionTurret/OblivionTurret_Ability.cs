using Asce.Game.Entities.Machines;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class OblivionTurret_Ability : CharacterAbility, IControlMachineAbility
    {
        [SerializeField] private OblivionTurret_Machine _machine;

        public OblivionTurret_Machine Machine => _machine;
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
            Machine.ResetStatus();
        }

        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);
            if (_owner == null) return;

            Vector2 direction = position - (Vector2)_owner.transform.position;
            Machine.Rotate(direction);
        }

        private void Machine_OnDead()
        {
            this.DespawnTime.ToComplete();
        }
    }
}
