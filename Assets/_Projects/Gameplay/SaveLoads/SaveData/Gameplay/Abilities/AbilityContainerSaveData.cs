using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class AbilityContainerSaveData : SaveData
    {
        public string id;
        public string name;
        public float cooldown;
        public string abilityInstanceId;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is AbilityContainerSaveData abilityData)
            {
                id = abilityData.id;
                name = abilityData.name;
                cooldown = abilityData.cooldown;
                abilityInstanceId = abilityData.abilityInstanceId;
            }
        }
    }
}