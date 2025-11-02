using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Levelings
{
    [CreateAssetMenu(menuName = "Asce/Levelings/Leveling Information", fileName = "Leveling Information")]
    public class SO_LevelingInformation : ScriptableObject
    {
        [Header("Level Settings")]
        [SerializeField, Min(1)] protected int _maxLevel = 10;

        [SerializeField] protected LevelingMode _levelingMode = LevelingMode.PerLevelChanges;

        [Tooltip("Use when Leveling Mode = PerLevelChanges")]
        [SerializeField] protected List<LevelModificationGroup> _levelChanges = new();
        private ReadOnlyCollection<LevelModificationGroup> _levelChangesReadonly;

        [Tooltip("Use when Leveling Mode = UniformGrowth")]
        [SerializeField] protected LevelModificationGroup _uniformGrowth;


        /// <summary> The maximum level that can be reached. </summary>
        public int MaxLevel => _maxLevel;

        /// <summary> Determines how leveling behavior is applied. </summary>
        public LevelingMode LevelingMode => _levelingMode;

        /// <summary> The list of modification groups for each level (used when LevelingMode = PerLevelChanges). </summary>
        public ReadOnlyCollection<LevelModificationGroup> LevelChanges => _levelChangesReadonly ??= _levelChanges.AsReadOnly();

        /// <summary> The uniform modification applied on every level-up (used when LevelingMode = UniformGrowth). </summary>
        public LevelModificationGroup UniformGrowth => _uniformGrowth;

        /// <summary>
        /// Get the modification group for a specific level based on the current leveling mode.
        /// </summary>
        public LevelModificationGroup GetLevelModifications(int level)
        {
            switch (_levelingMode)
            {
                case LevelingMode.PerLevelChanges:
                    level--;
                    if (level < 0 || level >= _levelChanges.Count)
                        return null;
                    return _levelChanges[level];

                case LevelingMode.UniformGrowth:
                    return _uniformGrowth;

                default:
                    return null;
            }
        }
    }
}
