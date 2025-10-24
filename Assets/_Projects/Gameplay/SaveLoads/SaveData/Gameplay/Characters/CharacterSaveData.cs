using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class CharacterSaveData : EntitySaveData
    {
        public GunSaveData gun;
        public CharacterAbilitiesSaveData abilities;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is CharacterSaveData characterData)
            {
                gun = new ();
                gun.CopyFrom(characterData.gun);

                abilities = new ();
                abilities.CopyFrom(characterData.abilities);
            }
        }
    }
}