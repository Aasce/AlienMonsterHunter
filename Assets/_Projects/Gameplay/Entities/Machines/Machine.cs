using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class Machine : Entity, IHasEntityUI, ISaveable<MachineSaveData>, ILoadable<MachineSaveData>
    {
        [SerializeField, Readonly] protected MachineUI _ui;

        public new SO_MachineInformation Information => base.Information as SO_MachineInformation;
        public MachineUI UI => _ui;

        EntityUI IHasEntityUI.UI => this.UI;

        protected override void RefReset()
        {
            base.RefReset();
            if (this.LoadComponent(out _ui))
            {
                _ui.Owner = this;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            UI.Initialize();
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            UI.ResetStatus();
        }

        public MachineSaveData Save()
        {
            EntitySaveData baseData = ((ISaveable<EntitySaveData>)this).Save();
            MachineSaveData machineData = new();
            machineData.CopyFrom(baseData);
            OnBeforeSave(machineData);
            return machineData;
        }

        public void Load(MachineSaveData data)
        {
            ((ILoadable<EntitySaveData>)this).Load(data);
            OnAfterLoad(data);
        }

        protected virtual void OnBeforeSave(MachineSaveData data) { }
        protected virtual void OnAfterLoad(MachineSaveData data) { }
    }
}
