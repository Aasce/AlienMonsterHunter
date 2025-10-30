using Asce.Game.Entities.Machines;
using Asce.Game.SaveLoads;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class SentinelTurret_Ability : CharacterAbility, IControlMachineAbility
    {
        [SerializeField] private SentinelTurret_Machine _machine;

        public SentinelTurret_Machine Machine => _machine;
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

        private void Machine_OnDead(Combats.DamageContainer container)
        {
            this.DespawnTime.ToComplete();
        }

        protected override void OnBeforeSave(AbilitySaveData data)
        {
            base.OnBeforeSave(data);
            MachineSaveData machineData = ((ISaveable<MachineSaveData>)Machine).Save();
            data.SetCustom("Machine", machineData);
        }

        protected override void OnAfterLoad(AbilitySaveData data)
        {
            base.OnAfterLoad(data);
            MachineSaveData machineData = data.GetCustom<MachineSaveData>("Machine");
            ((ILoadable<MachineSaveData>)Machine).Load(machineData);
        }
    }
}
