using Asce.SaveLoads;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class LevelingSaveData : SaveData
    {
        public int level;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is LevelingSaveData levelingData)
            {
                level = levelingData.level;
            }
        }
    }
}