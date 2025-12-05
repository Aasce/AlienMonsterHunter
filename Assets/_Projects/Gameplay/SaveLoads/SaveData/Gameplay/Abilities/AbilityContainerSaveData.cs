using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class AbilityContainerSaveData : SaveData
    {
        public string id;
        public string name;
        public int level;
        public float cooldown;
        public float remainCooldown;

        public string abilityInstanceId;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is AbilityContainerSaveData abilityData)
            {
                id = abilityData.id;
                name = abilityData.name;
                level = abilityData.level;
                cooldown = abilityData.cooldown;
                remainCooldown = abilityData.remainCooldown;
                abilityInstanceId = abilityData.abilityInstanceId;
            }
        }
    }
}