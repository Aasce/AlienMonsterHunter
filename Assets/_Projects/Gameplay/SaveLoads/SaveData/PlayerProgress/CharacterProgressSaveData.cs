using System;
using System.Collections.Generic;

namespace Asce.SaveLoads
{
    [System.Serializable]
    public class CharacterProgressSaveData : SaveData
    {
        public string name;
        public bool isUnlocked;
        public int level;
        public int exp;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is CharacterProgressSaveData otherData)
            {
                name = otherData.name;
                isUnlocked = otherData.isUnlocked;
                level = otherData.level;
                exp = otherData.exp;
            }
        }
    }
}