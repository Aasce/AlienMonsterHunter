using Asce.Game.Levelings;
using System;
using UnityEngine;

namespace Asce.Game.Entities.Characters
{
    public class CharacterLeveling : EntityLeveling
    {
        [SerializeField] private int _currentExp = 0;
        public event Action<int> OnAddExp;

        public new SO_CharacterLeveling BaseLeveling => base.BaseLeveling as SO_CharacterLeveling;
        public int CurrentExp => _currentExp;

        /// <summary>
        ///     Add experience to the character.  
        ///     If experience exceeds the threshold, the character levels up.  
        /// </summary>
        public void AddExp(int exp)
        {
            if (exp <= 0) return;

            _currentExp += exp;
            while (_currentExp >= ExpToLevelUp() && !IsMaxLevel)
            {
                int expToNext = ExpToLevelUp();
                _currentExp -= expToNext;
                LevelUp();

                if (IsMaxLevel) // Prevent overflow beyond max level
                {
                    _currentExp = 0;
                    break;
                }
            }
            OnAddExp?.Invoke(exp);
        }

        /// <summary> Returns the required experience to reach the next level. </summary>
        public int ExpToLevelUp()
        {
            return BaseLeveling.BaseExpToLevelUp + BaseLeveling.ExpIncrementPerLevel * CurrentLevel;
        }
    }
}
