using System;
using UnityEngine;

namespace Asce.Game.Stats
{
    [Serializable]
    public class ResourceStat : Stat 
    {
        [Header("Resource Stat")]
        [SerializeField] private float _currentValue = 0;

        public event Action<float, float> OnCurrentValueChanged;

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

        public ResourceStat() : base()
        {
            OnFinalValueChanged += (oldValue, newValue) => 
            {
                if (newValue > oldValue) CurrentValue += newValue - oldValue;
            };
        }
    }
}