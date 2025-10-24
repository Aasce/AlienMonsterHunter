using Asce.Game.Abilities;
using Asce.Game.Entities.Machines;
using Asce.Game.SaveLoads;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Supports
{
    public class Ambulance_Support : Support, IControlMachineAbility
    {
        [SerializeField] private Ambulance_Machine _machine;

        public Ambulance_Machine Machine => _machine;
        Machine IControlMachineAbility.Machine => _machine;

        public override void Initialize()
        {
            base.Initialize();

            Machine.Initialize();
            Machine.OnDead += Machine_OnDead;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            Machine.ResetStatus();
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

        public override void OnLoad()
        {
            base.OnLoad();
            Machine.Agent.SetDestination(CallPosition);
        }

        private void Machine_OnDead()
        {
            SupportController.Instance.Despawn(this);
        }

        protected override void OnBeforeSave(SupportSaveData data)
        {
            base.OnBeforeSave(data);
            MachineSaveData machineData = (Machine as ISaveable<MachineSaveData>).Save();
            data.SetCustom("Machine", machineData);
        }

        protected override void OnAfterLoad(SupportSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            MachineSaveData machineData = data.GetCustom<MachineSaveData>("Machine");
            ((ILoadable<MachineSaveData>)Machine).Load(machineData);
        }
    }
}
