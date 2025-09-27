using UnityEngine;

namespace Asce.Game.Stats
{
    [System.Serializable]
    public struct StatValue
    {
        [SerializeField] private float _value;
        [SerializeField] private StatValueType _type;

        public readonly float Value => _value;
        public readonly StatValueType Type => _type;

        public StatValue(float value, StatValueType type = StatValueType.Flat)
        {
            _value = value;
            _type = type;
        }
    }
}