using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class AllSupportsSaveData : SaveData
    {
        public List<SupportSaveData> supports = new();
    }
}