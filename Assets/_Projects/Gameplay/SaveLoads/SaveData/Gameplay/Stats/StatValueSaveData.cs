using Asce.Game.Stats;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class StatValueSaveData : SaveData
    {
        public string id;
        public float value;
        public StatValueType type;
        public StatSourceType sourceType;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is StatValueSaveData otherStatValue)
            {
                id = otherStatValue.id;
                value = otherStatValue.value;
                type = otherStatValue.type;
                sourceType = otherStatValue.sourceType;
            }
        }
    }
}