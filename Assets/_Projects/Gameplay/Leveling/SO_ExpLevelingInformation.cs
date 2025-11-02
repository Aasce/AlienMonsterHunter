using UnityEngine;

namespace Asce.Game.Levelings
{
    [CreateAssetMenu(menuName = "Asce/Levelings/Exp Leveling Information", fileName = "Exp Leveling")]
    public class SO_ExpLevelingInformation : SO_LevelingInformation
    {
        [Header("Experience Settings")]
        [Tooltip("Base experience required to reach Level")]
        [SerializeField, Min(0)] private int _baseExpToLevelUp = 100;

        [Tooltip("Additional experience increase required for each next level.")]
        [SerializeField, Min(0)] private int _expIncrementPerLevel = 20;

        /// <summary> The base amount of experience required to level up from Level 1 to Level 2. </summary>
        public int BaseExpToLevelUp => _baseExpToLevelUp;

        /// <summary> The additional experience required for each subsequent level. </summary>
        public int ExpIncrementPerLevel => _expIncrementPerLevel;
    }
}
