using UnityEngine;

namespace Asce.Game.SaveLoads
{
    public class CurrentGameConfigData : SaveData
    {
        public bool hasSave;
        public float playTime;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is CurrentGameConfigData configData)
            {
                hasSave = configData.hasSave;
                playTime = configData.playTime;
            }
        }

    }
}
