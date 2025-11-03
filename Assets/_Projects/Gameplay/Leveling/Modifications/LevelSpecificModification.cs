using System;
using UnityEngine;

namespace Asce.Game.Levelings
{
    [Serializable]
    public class LevelSpecificModification
    {
        [Min(1)]
        public int Level;

        public LevelModificationGroup ModificationGroup;
    }
}
