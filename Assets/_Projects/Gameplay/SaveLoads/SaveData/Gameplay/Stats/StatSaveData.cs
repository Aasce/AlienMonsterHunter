using Asce.SaveLoads;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class StatSaveData : SaveData
    {
        public List<StatValueSaveData> values = new();

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is StatSaveData statData)
            {
                values.Clear();
                values.AddRange(statData.values);
            }
        }
    }
}