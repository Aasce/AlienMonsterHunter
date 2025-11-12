using Asce.Game.SaveLoads;
using Asce.SaveLoads;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Stats
{
    [System.Serializable]
    public class Stat : ISaveable<StatSaveData>, ILoadable<StatSaveData>
    {
        [SerializeField] protected List<StatValue> _statValues = new();

        [Space]
        [SerializeField] protected float _finalValue;

        [Header("Cache Values")]
        [SerializeField] protected float _flatValue;
        [SerializeField] protected float _ratioValue;
        [SerializeField] protected float _scaleValue;
        [SerializeField] protected float _setValue;

        public event Action<float, float> OnFinalValueChanged;

        public virtual float FinalValue
        {
            get => _finalValue;
            protected set
            {
                if (value == _finalValue) return;
                float oldValue = _finalValue;
                _finalValue = value;
                OnFinalValueChanged?.Invoke(oldValue, _finalValue);
            }
        }

        public float FlatValue => _flatValue;
        public float RatioValue => _ratioValue;
        public float ScaleValue => _scaleValue;
        public float SetValue => _setValue;

        public virtual StatValue Get(string id)
        {
            return _statValues.Find((statValue) => statValue.Id == id);
        }

        public virtual StatValue Add(float value, StatValueType type = StatValueType.Flat, StatSourceType sourceType = StatSourceType.Default)
        {
            StatValue addValue = new (value, type, sourceType);
            _statValues.Add(addValue);
            this.Recalculate();
            return addValue;
        }

        public virtual void ModifyValue(string id, float value)
        {
            int statIndex = _statValues.FindIndex((statValue) => statValue.Id == id);
            if (statIndex < 0) return;

            StatValue stat = _statValues[statIndex].CopyWith(value: value);
            _statValues[statIndex] = stat;
            this.Recalculate();
        }

        public virtual void RemoveById(string id)
        {
            _statValues.RemoveAll((statValue) => statValue.Id == id);
            this.Recalculate();
        }

        public virtual void Clear(StatSourceType sourceType = StatSourceType.Default)
        {
            _statValues.RemoveAll(value => value.SourceType == sourceType);
            this.Recalculate();
        }
        public virtual void ClearAll()
        {
            _statValues.Clear();
            this.Recalculate();
        }

        public void Recalculate()
        {
            _flatValue = 0f;
            _ratioValue = 1f;
            _scaleValue = 1f;
            _setValue = 0f;
            bool isSet = false;

            foreach (StatValue statValue in _statValues)
            {
                switch (statValue.Type)
                {
                    case StatValueType.Flat:
                        _flatValue += statValue.Value;
                        break;

                    case StatValueType.Ratio:
                        _ratioValue *= 1 + statValue.Value;
                        break;

                    case StatValueType.Scale:
                        _scaleValue *= statValue.Value;
                        break;

                    case StatValueType.Set:
                        _setValue = statValue.Value;
                        isSet = true;
                        break;
                }
            }

            FinalValue = isSet ? _setValue : (_flatValue * _ratioValue) * _scaleValue;
        }

        StatSaveData ISaveable<StatSaveData>.Save()
        {
            StatSaveData data = new();
            foreach (StatValue statValue in _statValues)
            {
                data.values.Add((statValue as ISaveable<StatValueSaveData>).Save());
            }
            return data;
        }

        void ILoadable<StatSaveData>.Load(StatSaveData data)
        {
            if (data == null) return;
            _statValues.Clear();
            foreach (StatValueSaveData saveData in data.values)
            {
                StatValue statValue = StatValue.Create(saveData);
                _statValues.Add(statValue);
            }
            this.Recalculate();
        }
    }
}
