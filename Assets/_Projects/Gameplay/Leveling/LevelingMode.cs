using UnityEngine;

namespace Asce.Game.Levelings
{
    public enum LevelingMode
    {
        PerLevelChanges,
        UniformGrowth,
        HybridGrowth // New mode: specific levels + fallback to uniform growth
    }
}