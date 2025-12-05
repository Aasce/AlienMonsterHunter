using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class SupportSaveData : SaveDataWithCustoms
    {
        public string id;
        public string nameId;
        
        public LevelingSaveData level;

        public Vector2 position;
        public float rotation;
        public Vector2 callPosition;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is SupportSaveData supportData)
            {
                id = supportData.id;
                nameId = supportData.nameId;

                level = new();
                level.CopyFrom(supportData.level);

                position = supportData.position;
                rotation = supportData.rotation;
                callPosition = supportData.callPosition;
            }
        }
    }
}