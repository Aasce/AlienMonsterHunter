using Asce.Game.Abilities;
using Asce.Game.Entities.Machines;
using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Supports
{
    public class LineFireLauncher_Support : Support, IControlMachineAbility
    {
        [SerializeField] private LineFireLauncher_Machine _machine;

        public LineFireLauncher_Machine Machine => _machine;
        Machine IControlMachineAbility.Machine => _machine;

        public override void Initialize()
        {
            base.Initialize();

            Machine.Initialize();
            Leveling.OnLevelChanged += Leveling_OnLevelChanged;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            Machine.ResetStatus();
        }

        public override void OnActive()
        {
            base.OnActive();
            Machine.transform.position = transform.position;
        }

        public override void Recall()
        {
            base.Recall();
            Machine.Destination = CallPosition;
            Machine.StartFire();
        }

        public override void OnLoad()
        {
            base.OnLoad();
        }

        private void Leveling_OnLevelChanged(int newLevel)
        {
            Machine.Leveling.SetLevel(newLevel);
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
