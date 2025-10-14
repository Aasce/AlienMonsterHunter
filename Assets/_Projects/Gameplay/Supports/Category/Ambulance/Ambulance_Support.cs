using Asce.Game.Abilities;
using Asce.Game.Entities.Machines;
using UnityEngine;

namespace Asce.Game.Supports
{
    public class Ambulance_Support : Support, IControlMachineAbility
    {
        [SerializeField] private Ambulance_Machine _machine;

        public Ambulance_Machine Machine => _machine;
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

        public override void OnActive()
        {
            base.OnActive();
            Machine.Agent.Warp(transform.position);
            Machine.Agent.SetDestination(CallPosition);
        }

        public override void Recall()
        {
            base.Recall();
            Machine.Agent.SetDestination(CallPosition);
        }

        private void Machine_OnDead()
        {
            SupportController.Instance.Despawn(this);
        }
    }
}
