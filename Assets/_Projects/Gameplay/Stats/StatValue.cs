using Asce.Managers;
using System;
using UnityEngine;

namespace Asce.Game.Stats
{
    [System.Serializable]
    public struct StatValue : IIdentifiable
    {
        public const string PREFIX_ID = "stat";

        [SerializeField] private string _id;
        [SerializeField] private float _value;
        [SerializeField] private StatValueType _type;

        public readonly string Id => _id;
        public readonly float Value => _value;
        public readonly StatValueType Type => _type;
        string IIdentifiable.Id 
        { 
            readonly get => Id; 
            set => _id = value; 
        }

        /// <summary> Create new stat with unique ID. </summary>
        public StatValue(float value, StatValueType type = StatValueType.Flat)
        {
            _id = IdGenerator.NewId(PREFIX_ID);
            _value = value;
            _type = type;
        }

        /// <summary> Create stat with predefined ID </summary>
        public StatValue(string id, float value, StatValueType type = StatValueType.Flat)
        {
            _id = string.IsNullOrEmpty(id) ? IdGenerator.NewId(PREFIX_ID) : id;
            _value = value;
            _type = type;
        }

        public override readonly string ToString()
        {
            return $"StatValue(Id={_id}, Value={_value}, Type={_type})";
        }
    }
}
