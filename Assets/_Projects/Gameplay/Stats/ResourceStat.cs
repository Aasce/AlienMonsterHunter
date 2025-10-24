using Asce.Game.SaveLoads;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Stats
{
    [Serializable]
    public class ResourceStat : Stat, ISaveable<ResourceStatSaveData>, ILoadable<ResourceStatSaveData>
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


        ResourceStatSaveData ISaveable<ResourceStatSaveData>.Save()
        {
            StatSaveData baseData = ((ISaveable<StatSaveData>) this).Save();
            ResourceStatSaveData data = new();
            data.CopyFrom(baseData);
            data.currentValue = CurrentValue;
            return data;
        }

        void ILoadable<ResourceStatSaveData>.Load(ResourceStatSaveData data)
        {
            if (data == null) return;
            ((ILoadable<StatSaveData>)this).Load(data);
            CurrentValue = data.currentValue;
        }
    }
}