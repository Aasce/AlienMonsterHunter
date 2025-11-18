using Asce.Game.SaveLoads;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Levelings
{
    public class ExpLeveling : Leveling, ISaveable<ExpLevelingSaveData>, ILoadable<ExpLevelingSaveData>
    {
        [SerializeField] private int _currentExp = 0;
        public event Action<int> OnAddExp;
        public event Action<int> OnCurrentExpChanged;

        public new SO_ExpLevelingInformation BaseLeveling => base.BaseLeveling as SO_ExpLevelingInformation;

        public int CurrentExp
        {
            get => _currentExp;
            private set
            {
                if (_currentExp == value) return;
                _currentExp = Mathf.Max(0, value);
                OnCurrentExpChanged?.Invoke(_currentExp);
            }
        }

        public override void SetLevel(int level)
        {
            base.SetLevel(level);
            CurrentExp = 0;
        }

        public virtual void SetExp(int exp)
        {
            CurrentExp = Mathf.Clamp(exp, 0, ExpToLevelUp());
        }

        public override void LevelUp()
        {
            base.LevelUp();
            CurrentExp = 0;
        }

        /// <summary>
        ///     Add experience to the character.  
        ///     Handles multi-level up logic and keeps excess exp when reaching max level.  
        ///     Invokes OnAddExp once after all processing.  
        /// </summary>
        public void AddExp(int exp)
        {
            if (exp <= 0) return;

            int totalAdded = exp;
            int remainingExp = _currentExp + exp;

            while (!IsMaxLevel)
            {
                int expToNext = ExpToLevelUp();
                if (remainingExp < expToNext)
                {
                    CurrentExp = remainingExp;
                    OnAddExp?.Invoke(totalAdded);
                    return;
                }

                remainingExp -= expToNext;
                LevelUp();

                if (IsMaxLevel)
                {
                    // Reached max level, keep remaining exp as-is
                    CurrentExp = remainingExp;
                    OnAddExp?.Invoke(totalAdded);
                    return;
                }
            }

            // Already max level before adding exp, just add normally and clamp
            CurrentExp += exp;
            OnAddExp?.Invoke(totalAdded);
        }

        /// <summary> Returns the required experience to reach the next level. </summary>
        public int ExpToLevelUp()
        {
            if (BaseLeveling == null) return 0;
            return BaseLeveling.ExpToLevelUp(CurrentLevel);
        }

        ExpLevelingSaveData ISaveable<ExpLevelingSaveData>.Save()
        {
            ExpLevelingSaveData levelingData = new()
            {
                level = _currentLevel,
                exp = _currentExp,
            };
            return levelingData;
        }

        void ILoadable<ExpLevelingSaveData>.Load(ExpLevelingSaveData data)
        {
            if (data == null) return;
            _currentLevel = data.level;
            _currentExp = data.exp;
        }
    }
}
