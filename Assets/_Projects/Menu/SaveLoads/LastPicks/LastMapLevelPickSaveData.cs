using Asce.SaveLoads;

namespace Asce.MainMenu.SaveLoads
{
    [System.Serializable]
    public class LastMapLevelPickSaveData : SaveData
    {
        public string mapName;
        public int level;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is LastMapLevelPickSaveData lastPickData)
            {
                mapName = lastPickData.mapName;
                level = lastPickData.level;
            }
        }
    }
}