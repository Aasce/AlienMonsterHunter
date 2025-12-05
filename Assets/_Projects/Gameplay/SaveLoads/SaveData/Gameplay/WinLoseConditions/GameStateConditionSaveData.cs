using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    public class GameStateConditionSaveData : SaveDataWithCustoms
    {
        public string name;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is GameStateConditionSaveData conditionData)
            {
                name = conditionData.name;
            }
        }

    }
}
