using Asce.Game.SaveLoads;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class Machine : Entity, ISaveable<MachineSaveData>, ILoadable<MachineSaveData>
    {
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
