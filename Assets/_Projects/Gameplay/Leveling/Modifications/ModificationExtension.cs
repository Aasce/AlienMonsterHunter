using Asce.Game.Stats;
using UnityEngine;

namespace Asce.Game.Levelings
{
    public static class ModificationExtension
    {
        public static StatValueType ToStatType(this ModificationType modificationType)
        {
            return modificationType switch
            {
                ModificationType.Additive => StatValueType.Flat,
                ModificationType.Multiplicative => StatValueType.Ratio,
                ModificationType.Set => StatValueType.Flat,
                _ => StatValueType.Flat,
            };
        }
    }
}