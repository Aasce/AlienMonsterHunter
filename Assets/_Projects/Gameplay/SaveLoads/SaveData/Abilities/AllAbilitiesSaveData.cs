using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class AllAbilitiesSaveData<T> : SaveData where T : AbilitySaveData
    {
        public List<T> abilities = new();
    }
}