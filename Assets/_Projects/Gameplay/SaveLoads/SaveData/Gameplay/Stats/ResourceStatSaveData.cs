using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class ResourceStatSaveData : StatSaveData
    {
        public float currentValue;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is ResourceStatSaveData statData)
            {
                currentValue = statData.currentValue;
            }
        }
    }
}