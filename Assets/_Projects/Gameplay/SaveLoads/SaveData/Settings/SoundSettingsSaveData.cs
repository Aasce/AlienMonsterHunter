using System;
using System.Collections.Generic;

namespace Asce.SaveLoads
{
    [System.Serializable]
    public class SoundSettingsSaveData : SaveData
    {
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is SoundSettingsSaveData otherData)
            {
                masterVolume = otherData.masterVolume;
                musicVolume = otherData.musicVolume;
                sfxVolume = otherData.sfxVolume;
            }
        }
    }
}