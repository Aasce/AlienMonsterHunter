using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public struct StatContainerSaveData
    {
        [SerializeField] private string _name;
        [SerializeField] private StatSaveData _stat;

        public readonly string Name => _name;
        public readonly StatSaveData Stat => _stat;

        public StatContainerSaveData(string name, StatSaveData stat)
        {
            _name = name;
            _stat = stat;
        }
    }
}