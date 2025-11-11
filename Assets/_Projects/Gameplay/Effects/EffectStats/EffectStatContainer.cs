using UnityEngine;

namespace Asce.Game.Effects
{
    [System.Serializable]
    public struct EffectStatContainer
    {
        [SerializeField] private string _name;
        [SerializeField] private EffectStat _stat;

        public readonly string Name => _name;
        public readonly EffectStat EffectStat => _stat;

        public EffectStatContainer(string name, EffectStat stat)
        {
            _name = name;
            _stat = stat;
        }
    }
}