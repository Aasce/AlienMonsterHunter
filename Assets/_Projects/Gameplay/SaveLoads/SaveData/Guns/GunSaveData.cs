using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class GunSaveData : SaveData
    {
        public string id;
        public string name;
        public int currentAmmo;
        public int remainingAmmo;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is GunSaveData gunData)
            {
                id = gunData.id;
                name = gunData.name;
                currentAmmo = gunData.currentAmmo;
                remainingAmmo = gunData.remainingAmmo;
            }
        }
    }
}