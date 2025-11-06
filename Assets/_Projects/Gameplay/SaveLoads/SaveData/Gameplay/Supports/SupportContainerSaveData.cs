using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class SupportContainerSaveData : SaveData
    {
        public string id;
        public string supportKey;
        public string currentSupportId;
        public float cooldown;


        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is SupportContainerSaveData abilityData)
            {
                id = abilityData.id;
                supportKey = abilityData.supportKey;
                currentSupportId = abilityData.currentSupportId;
                cooldown = abilityData.cooldown;
            }
        }
    }
}