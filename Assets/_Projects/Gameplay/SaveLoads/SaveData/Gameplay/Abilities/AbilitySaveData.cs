using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class AbilitySaveData : SaveDataWithCustoms
    {
        public string id;
        public string name;
        public string ownerId;
        public float despawnTime;
        public Vector2 position;
        public float rotation;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is AbilitySaveData abilityData)
            {
                id = abilityData.id;
                name = abilityData.name;
                ownerId = abilityData.ownerId;
                despawnTime = abilityData.despawnTime;
                position = abilityData.position;
                rotation = abilityData.rotation;
            }
        }
    }
}