using System;
using System.Collections.Generic;

namespace Asce.SaveLoads
{
    [System.Serializable]
    public class GameProgressSaveData : SaveData
    {
        public int openGamesCount;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is GameProgressSaveData otherData)
            {
                openGamesCount = otherData.openGamesCount;
            }
        }
    }
}