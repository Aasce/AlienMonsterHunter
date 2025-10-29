using UnityEngine;

namespace Asce.Game.Levelings
{
    [CreateAssetMenu(menuName = "Asce/Entities/Character Leveling", fileName = "Character Leveling")]
    public class SO_CharacterLeveling : SO_EntityLeveling
    {
        [Header("Experience Settings")]
        [Tooltip("Base experience required to reach Level 2.")]
        [SerializeField, Min(0)] private int _baseExpToLevelUp = 100;

        [Tooltip("Additional experience increase required for each next level.")]
        [SerializeField, Min(0)] private int _expIncrementPerLevel = 20;

        /// <summary> The base amount of experience required to level up from Level 1 to Level 2. </summary>
        public int BaseExpToLevelUp => _baseExpToLevelUp;

        /// <summary> The additional experience required for each subsequent level. </summary>
        public int ExpIncrementPerLevel => _expIncrementPerLevel;
    }
}
