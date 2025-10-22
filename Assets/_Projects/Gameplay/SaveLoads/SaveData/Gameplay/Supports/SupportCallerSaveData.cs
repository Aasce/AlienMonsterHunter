using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class SupportCallerSaveData : SaveData
    {
        public string id;
        public List<SupportContainerSaveData> supports = new();

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is SupportCallerSaveData supportCallerData)
            {
                id = supportCallerData.id;

                supports.Clear();
                supports.AddRange(supportCallerData.supports);
            }
        }
    }
}