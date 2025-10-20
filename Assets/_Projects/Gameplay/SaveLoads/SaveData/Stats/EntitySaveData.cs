using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class EntitySaveData : SaveData
    {
        public string id;
        public string name;

        public Vector2 position;
        public float rotation;

        public StatsSaveData stats;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is EntitySaveData entityData)
            {
                id = entityData.id;
                name = entityData.name;
                position = entityData.position;
                rotation = entityData.rotation;

                stats = new StatsSaveData();
                stats.CopyFrom(entityData.stats);
            }
                
        }
    }
}