using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class MapSaveData : SaveData
    {
        public string mapName;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is MapSaveData mapData)
            {
                this.mapName = mapData.mapName;
            }
        }
    }
}