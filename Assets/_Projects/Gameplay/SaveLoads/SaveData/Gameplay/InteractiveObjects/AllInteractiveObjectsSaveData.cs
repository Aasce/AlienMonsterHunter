using Asce.SaveLoads;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class AllInteractiveObjectsSaveData : SaveData
    {
        public List<InteractiveObjectSaveData> interactiveObjects = new();

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is AllInteractiveObjectsSaveData allData)
            {
                interactiveObjects.Clear();
                interactiveObjects.AddRange(allData.interactiveObjects);
            }
        }
    }

}