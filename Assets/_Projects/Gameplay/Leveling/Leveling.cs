using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Levelings
{
    public class Leveling : GameComponent, ISaveable<LevelingSaveData>, ILoadable<LevelingSaveData>
    {
        [SerializeField, Readonly] protected SO_LevelingInformation _information;
        [SerializeField] protected int _currentLevel;

        public event Action<int> OnLevelChanged;
        public event Action<int> OnLevelUp;
        public event Action<int> OnLevelSetted;

        public SO_LevelingInformation BaseLeveling => _information;
        public int CurrentLevel => _currentLevel;
        public bool IsMaxLevel => _currentLevel >= BaseLeveling.MaxLevel;

        public virtual void Initialize(SO_LevelingInformation baseLeveling)
        {
            _information = baseLeveling;
        }

        public virtual void SetLevel(int level)
        {
            if (_currentLevel == level) return;
            _currentLevel = Mathf.Clamp(level, 0, BaseLeveling.MaxLevel);
            OnLevelSetted?.Invoke(_currentLevel);
            OnLevelChanged?.Invoke(_currentLevel);
        }

        public virtual void LevelUp()
        {
            if (IsMaxLevel) return;
            _currentLevel = Mathf.Clamp(_currentLevel + 1, 0, BaseLeveling.MaxLevel);
            OnLevelUp?.Invoke(_currentLevel);
            OnLevelChanged?.Invoke(_currentLevel);
        }

        LevelingSaveData ISaveable<LevelingSaveData>.Save()
        {
            LevelingSaveData levelingData = new()
            {
                level = _currentLevel,
            };
            this.OnBeforeSave(levelingData);
            return levelingData;
        }

        void ILoadable<LevelingSaveData>.Load(LevelingSaveData data)
        {
            if (data == null) return;
            _currentLevel = data.level;
            this.OnAfterLoad(data);
        }

        protected virtual void OnBeforeSave(LevelingSaveData data) { }
        protected virtual void OnAfterLoad(LevelingSaveData data) { }
    }
}