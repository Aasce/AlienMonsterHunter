using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class EntitySaveData : SaveDataWithCustoms
    {
        public string id;
        public string name;

        public Vector2 position;
        public float rotation;

        public LevelingSaveData leveling;
        public StatsSaveData stats;
        public EffectsSaveData effects;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is EntitySaveData entityData)
            {
                id = entityData.id;
                name = entityData.name;
                position = entityData.position;
                rotation = entityData.rotation;

                leveling = new LevelingSaveData();
                leveling.CopyFrom(entityData.leveling);

                stats = new StatsSaveData();
                stats.CopyFrom(entityData.stats);

                effects = new EffectsSaveData();
                effects.CopyFrom(entityData.effects);
            }
                
        }
    }
}