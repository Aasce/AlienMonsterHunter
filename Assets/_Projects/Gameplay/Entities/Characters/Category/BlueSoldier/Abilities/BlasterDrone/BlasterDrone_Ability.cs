using Asce.Game.Entities.Machines;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class BlasterDrone_Ability : CharacterAbility, IControlMachineAbility
    {
        [SerializeField] private BlasterDrone_Machine _machine;

        public BlasterDrone_Machine Machine => _machine;
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
            Machine.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
        }

        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);
            if (_owner == null) return;

            Vector2 direction = position - (Vector2)_owner.transform.position;
            Machine.MoveDirection = direction;
        }

        private void Machine_OnDead()
        {
            this.DespawnTime.ToComplete();
        }
    }
}
