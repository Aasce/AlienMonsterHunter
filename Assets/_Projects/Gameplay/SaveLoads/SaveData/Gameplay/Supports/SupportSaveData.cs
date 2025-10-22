using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class SupportSaveData : SaveData
    {
        public string id;
        public string nameId;
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
                position = supportData.position;
                rotation = supportData.rotation;
                callPosition = supportData.callPosition;
            }
        }
    }
}