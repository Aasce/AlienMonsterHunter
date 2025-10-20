using UnityEngine;

namespace Asce.Game.Stats
{
    [System.Serializable]
    public struct StatContainer
    {
        [SerializeField] private string _name;
        [SerializeField] private Stat _stat;

        public readonly string Name => _name;
        public readonly Stat Stat => _stat;

        public StatContainer(string name, Stat stat)
        {
            _name = name;
            _stat = stat;
        } 
    }
}