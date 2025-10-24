using Asce.SaveLoads;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class AllEntitiesSaveData<T> : SaveData where T : EntitySaveData
    {
        public List<T> entities = new();
    }
}