using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class EffectSaveData : SaveDataWithCustoms
    {
        public string id;
        public string name;
        public string senderId;
        public float strength;
        public float baseDuration;
        public float duration;
        public int stack;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is EffectSaveData effectData)
            {
                id = effectData.id;
                name = effectData.name;
                strength = effectData.strength;
                senderId = effectData.senderId;
                baseDuration = effectData.baseDuration;
                duration = effectData.duration;
                stack = effectData.stack;
            }
        }
    }
}
