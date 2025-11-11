using Asce.SaveLoads;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class EffectsSaveData : SaveData
    {
        public List<EffectSaveData> effects = new();
        public List<EffectStatContainerSaveData> effectStats = new();

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is EffectsSaveData effectsData)
            {
                effects.Clear();
                effects.AddRange(effectsData.effects);

                effectStats.Clear();
                effectStats.AddRange(effectsData.effectStats);
            }
        }
    }
}
