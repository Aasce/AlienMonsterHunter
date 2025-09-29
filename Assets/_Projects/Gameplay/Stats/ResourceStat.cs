using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Asce.Game.Stats
{
    [Serializable]
    public class ResourceStat : Stat 
    {
        [Header("Resource Stat")]
        [SerializeField] private float _currentValue = 0;

        public event Action<float, float> OnCurrentValueChanged;


        public override float FinalValue
        {
            get => _finalValue;
            protected set
            {
                float oldValue = _finalValue;
                base.FinalValue = value;
                if (_finalValue > oldValue) CurrentValue += _finalValue - oldValue;
            }
        }


        public float CurrentValue
        {
            get => _currentValue;
            set
            {
                float oldValue = _currentValue;
                _currentValue = Mathf.Clamp(value, 0, FinalValue);
                OnCurrentValueChanged?.Invoke(oldValue, _currentValue);
            }
        }
    }
}