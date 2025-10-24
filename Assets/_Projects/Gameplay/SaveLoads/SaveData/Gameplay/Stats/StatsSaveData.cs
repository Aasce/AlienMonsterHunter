using Asce.SaveLoads;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class StatsSaveData : SaveData
    {
        public List<StatContainerSaveData> stats = new();
        //public List<ResourceStatContainerSaveData> resourceStats = new();

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is StatsSaveData statData)
            {
                stats.Clear();
                stats.AddRange(statData.stats);

                //resourceStats.Clear();
                //resourceStats.AddRange(statData.resourceStats);
            }
        }
    }
}