using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class CharacterSaveData : EntitySaveData
    {
        public GunSaveData gun;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is CharacterSaveData characterData)
            {
                gun = new GunSaveData();
                gun.CopyFrom(characterData.gun);
            }
        }
    }
}