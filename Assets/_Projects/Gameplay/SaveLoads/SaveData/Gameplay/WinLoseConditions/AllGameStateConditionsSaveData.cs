using Asce.SaveLoads;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    public class AllGameStateConditionsSaveData : SaveData
    {
        public List<GameStateConditionSaveData> winConditions = new();
        public List<GameStateConditionSaveData> loseConditions = new();

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is AllGameStateConditionsSaveData conditionsData)
            {
                winConditions.Clear();
                winConditions.AddRange(conditionsData.loseConditions);

                loseConditions.Clear();
                loseConditions.AddRange(conditionsData.loseConditions);
            }
        }

    }
}
