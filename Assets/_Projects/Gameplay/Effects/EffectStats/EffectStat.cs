using Asce.Game.SaveLoads;
using Asce.SaveLoads;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Effects
{
    [System.Serializable]
    public class EffectStat : ISaveable<EffectStatSaveData>, ILoadable<EffectStatSaveData>
    {
        [SerializeField] protected List<EffectStatValue> _statValues = new();
        public event Action<bool> OnAffectChanged;

        public bool IsAffect => _statValues.Count > 0;


        public virtual EffectStatValue Add()
        {
            bool isAffect = IsAffect;
            EffectStatValue addValue = new(null);
            _statValues.Add(addValue);
            if (isAffect != IsAffect) OnAffectChanged?.Invoke(IsAffect);
            return addValue;
        }

        public virtual void RemoveById(string id)
        {
            bool isAffect = IsAffect;
            _statValues.RemoveAll((statValue) => statValue.Id == id);
            if (isAffect != IsAffect) OnAffectChanged?.Invoke(IsAffect);
        }

        public virtual void ClearAll()
        {
            bool isAffect = IsAffect;
            _statValues.Clear();
            if (isAffect != IsAffect) OnAffectChanged?.Invoke(IsAffect);
        }

        EffectStatSaveData ISaveable<EffectStatSaveData>.Save()
        {
            EffectStatSaveData data = new();
            foreach (EffectStatValue statValue in _statValues)
            {
                data.ids.Add(statValue.Id);
            }
            return data;
        }

        void ILoadable<EffectStatSaveData>.Load(EffectStatSaveData data)
        {
            if (data == null) return;

            _statValues.Clear();
            foreach (string id in data.ids)
            {
                EffectStatValue statValue = new(id);
                _statValues.Add(statValue);
            }
        }
    }
}