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

        public int level;

        public float cooldown;
        public float remainCooldown;


        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is SupportContainerSaveData supportContainerData)
            {
                id = supportContainerData.id;
                supportKey = supportContainerData.supportKey;
                currentSupportId = supportContainerData.currentSupportId;

                level = supportContainerData.level;

                cooldown = supportContainerData.cooldown;
                remainCooldown = supportContainerData.remainCooldown;
            }
        }
    }
}