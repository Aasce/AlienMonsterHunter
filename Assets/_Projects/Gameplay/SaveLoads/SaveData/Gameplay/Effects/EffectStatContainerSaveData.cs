using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public struct EffectStatContainerSaveData
    {
        [SerializeField] private string _name;
        [SerializeField] private EffectStatSaveData _effectStat;

        public readonly string Name => _name;
        public readonly EffectStatSaveData EffectStat => _effectStat;

        public EffectStatContainerSaveData(string name, EffectStatSaveData stat)
        {
            _name = name;
            _effectStat = stat;
        }
    }
}