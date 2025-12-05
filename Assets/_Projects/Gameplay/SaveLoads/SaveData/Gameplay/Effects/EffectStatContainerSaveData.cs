using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class EffectStatContainerSaveData
    {
        [SerializeField] private string _name;
        [SerializeField] private EffectStatSaveData _effectStat;

        public string Name => _name;
        public EffectStatSaveData EffectStat => _effectStat;

        public EffectStatContainerSaveData(string name, EffectStatSaveData stat)
        {
            _name = name;
            _effectStat = stat;
        }
    }
}