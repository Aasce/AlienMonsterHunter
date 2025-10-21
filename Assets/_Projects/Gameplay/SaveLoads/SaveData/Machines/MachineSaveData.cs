using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class MachineSaveData : EntitySaveData
    {
        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is MachineSaveData enemyData)
            {

            }
        }
    }
}
