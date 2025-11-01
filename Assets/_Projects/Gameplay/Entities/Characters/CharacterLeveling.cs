using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    public class CharacterLeveling : EntityLeveling
    {
        [SerializeField] private int _currentExp = 0;
        public event Action<int> OnAddExp;
        public event Action<int> OnCurrentExpChanged;

        public new SO_CharacterLeveling BaseLeveling => base.BaseLeveling as SO_CharacterLeveling;

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
            return BaseLeveling.BaseExpToLevelUp + BaseLeveling.ExpIncrementPerLevel * CurrentLevel;
        }
        protected override void OnBeforeSave(LevelingSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("CurrentExp", _currentExp);
        }

        protected override void OnAfterLoad(LevelingSaveData data)
        {
            base.OnAfterLoad(data);
            _currentExp = data.GetCustom<int>("CurrentExp");

        }

    }
}
