using Asce.Game.Levelings;
using Asce.Managers;
using Asce.Managers.Attributes;
using System;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class EntityLeveling : GameComponent
    {
        [SerializeField, Readonly] protected SO_EntityLeveling _baseLeveling;
        [SerializeField] protected int _currentLevel;

        public event Action<int> OnLevelChanged;
        public event Action<int> OnLevelUp;

        public SO_EntityLeveling BaseLeveling => _baseLeveling;
        public int CurrentLevel => _currentLevel;
        public bool IsMaxLevel => _currentLevel >= BaseLeveling.MaxLevel;

        public virtual void Initialize(SO_EntityLeveling baseLeveling)
        {
            _baseLeveling = baseLeveling;
        }

        public virtual void SetLevel(int level)
        {
            if (_currentLevel == level) return;
            _currentLevel = Mathf.Clamp(level, 0, BaseLeveling.MaxLevel);
            OnLevelChanged?.Invoke(_currentLevel);
        }

        public virtual void LevelUp()
        {
            if (IsMaxLevel) return;
            this.SetLevel(_currentLevel + 1);
            OnLevelUp?.Invoke(_currentLevel);
        }
    }
}