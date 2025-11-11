using Asce.SaveLoads;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class ExpLevelingSaveData : LevelingSaveData
    {
        public int exp;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is ExpLevelingSaveData expLevelingData)
            {
                exp = expLevelingData.exp;
            }
        }
    }
}