using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class CharacterAbilitiesSaveData : EntitySaveData
    {
        public List<AbilityContainerSaveData> abilityContainers = new();

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is CharacterAbilitiesSaveData characterData)
            {
                abilityContainers.Clear();
                abilityContainers.AddRange(characterData.abilityContainers);
            }
        }
    }
}