using System;
using UnityEngine;

namespace Asce.Game.Stats
{
    [System.Serializable]
    public struct StatValue
    {
        [SerializeField] private string _id;
        [SerializeField] private float _value;
        [SerializeField] private StatValueType _type;

        public readonly string Id => _id;
        public readonly float Value => _value;
        public readonly StatValueType Type => _type;

        // -------- Constructors --------

        /// <summary> Create new stat with unique ID. </summary>
        public StatValue(float value, StatValueType type = StatValueType.Flat)
        {
            _id = StatIdGenerator.NewId();
            _value = value;
            _type = type;
        }

        /// <summary> Create stat with predefined ID </summary>
        public StatValue(string id, float value, StatValueType type = StatValueType.Flat)
        {
            _id = string.IsNullOrEmpty(id) ? StatIdGenerator.NewId() : id;
            _value = value;
            _type = type;
        }

        public override readonly string ToString()
        {
            return $"StatValue(Id={_id}, Value={_value}, Type={_type})";
        }
    }

    /// <summary> Centralized ID generator for all StatValues.  </summary>
    public static class StatIdGenerator
    {
        /// <summary>  Generate globally unique, JSON-safe ID. </summary>
        public static string NewId()
        {
            return $"stat_{Guid.NewGuid():N}";
        }
    }
}
