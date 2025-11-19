using System;
using System.Collections.Generic;

namespace Asce.SaveLoads
{
    [System.Serializable]
    public class GunProgressSaveData : SaveData
    {
        public string name;
        public bool isUnlocked;
        public int level;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is GunProgressSaveData otherData)
            {
                name = otherData.name;
                isUnlocked = otherData.isUnlocked;
                level = otherData.level;
            }
        }
    }
}