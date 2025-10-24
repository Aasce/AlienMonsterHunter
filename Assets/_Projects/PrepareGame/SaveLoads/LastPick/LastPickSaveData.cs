using Asce.SaveLoads;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.PrepareGame.SaveLoads
{
    public class LastPickSaveData : SaveData
    {
        public string characterName;
        public string gunName;
        public List<string> supportIds = new();

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is LastPickSaveData lastPickData)
            {
                characterName = lastPickData.characterName;
                gunName = lastPickData.gunName;
                supportIds.Clear();
                supportIds.AddRange(lastPickData.supportIds);
            }
        }
    }
}
