using Asce.SaveLoads;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class EffectStatSaveData : SaveData
    {
        public List<string> ids = new();

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is EffectStatSaveData statData)
            {
                ids.Clear();
                ids.AddRange(statData.ids);
            }
        }
    }
}