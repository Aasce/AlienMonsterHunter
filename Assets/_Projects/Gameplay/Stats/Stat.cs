using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Stats
{
    [System.Serializable]
    public class Stat
    {
        [SerializeField] protected List<StatValue> _statValues = new();

        [Space]
        [SerializeField] protected float _finalValue;

        [Header("Cache Values")]
        [SerializeField] protected float _baseValue;
        [SerializeField] protected float _flatValue;
        [SerializeField] protected float _ratioValue;
        [SerializeField] protected float _scaleValue;

        public event Action<float, float> OnFinalValueChanged;

        public float FinalValue
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

        public float BaseValue => _baseValue;
        public float FlatValue => _flatValue;
        public float RatioValue => _ratioValue;
        public float ScaleValue => _scaleValue;

        public StatValue[] ArrayValues => _statValues.ToArray();


        public virtual void Add(float value, StatValueType type = StatValueType.Flat)
        {
            StatValue addValue = new (value, type);
            _statValues.Add(addValue);
            this.Recalculate();
        }

        public virtual void Clear(bool isClearBase = false)
        {
            if (isClearBase) _statValues.Clear();
            else _statValues.RemoveAll(value => value.Type != StatValueType.Base);

            this.Recalculate();
        }

        public void Recalculate()
        {
            _baseValue = 0f;
            _flatValue = 0f;
            _ratioValue = 1f;
            _scaleValue = 1f;

            foreach (StatValue statValue in _statValues)
            {
                switch (statValue.Type)
                {
                    case StatValueType.Base:
                        _baseValue += statValue.Value;
                        break;

                    case StatValueType.Flat:
                        _flatValue += statValue.Value;
                        break;

                    case StatValueType.Ratio:
                        _ratioValue *= 1 + statValue.Value;
                        break;

                    case StatValueType.Scale:
                        _scaleValue *= statValue.Value;
                        break;
                }
            }

            FinalValue = ((_baseValue + _flatValue) * _ratioValue) * _scaleValue;
        }

    }
}
