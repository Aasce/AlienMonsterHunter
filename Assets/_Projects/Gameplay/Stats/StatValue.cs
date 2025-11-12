using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Stats
{
    [System.Serializable]
    public struct StatValue : IIdentifiable, ISaveable<StatValueSaveData>
    {
        public const string PREFIX_ID = "stat";

        [SerializeField] private string _id;
        [SerializeField] private float _value;
        [SerializeField] private StatValueType _type;
        [SerializeField] private StatSourceType _sourceType;

        public readonly string Id => _id;
        public readonly float Value => _value;
        public readonly StatValueType Type => _type;
        public readonly StatSourceType SourceType => _sourceType;
        string IIdentifiable.Id 
        { 
            readonly get => Id; 
            set => _id = value; 
        }

        /// <summary> Create new stat with unique ID. </summary>
        public StatValue(float value, StatValueType type = StatValueType.Flat, StatSourceType sourceType = StatSourceType.Default)
        {
            _id = IdGenerator.NewId(PREFIX_ID);
            _value = value;
            _type = type;
            _sourceType = sourceType;
        }

        /// <summary> Create stat with predefined ID </summary>
        public StatValue(string id, float value, StatValueType type = StatValueType.Flat, StatSourceType sourceType = StatSourceType.Default)
        {
            _id = string.IsNullOrEmpty(id) ? IdGenerator.NewId(PREFIX_ID) : id;
            _value = value;
            _type = type;
            _sourceType = sourceType;
        }

        public override readonly string ToString()
        {
            return $"StatValue(Id={_id}, Value={_value}, Type={_type})";
        }
        public readonly StatValue CopyWith(
            float? value = null,
            StatValueType? type = null,
            StatSourceType? sourceType = null)
        {
            return new StatValue(
                _id,
                value ?? _value,
                type ?? _type,
                sourceType ?? _sourceType
            );
        }


        readonly StatValueSaveData ISaveable<StatValueSaveData>.Save()
        {
            return new StatValueSaveData()
            {
                id = _id,
                value = _value,
                type = _type,
                sourceType = _sourceType,
            };
        }

        public static StatValue Create(StatValueSaveData data)
        {
            if (data == null) return default;
            StatValue statValue = new ()
            {
                _id = data.id,
                _value = data.value,
                _type = data.type,
                _sourceType = data.sourceType
            };

            return statValue;
        }
    }
}
