using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class GunSaveData : SaveData
    {
        public string id;
        public string name;
        
        public float damage;
        public float penetration;
        public float shootSpeed;

        public int currentAmmo;
        public int remainingAmmo;
        public int magazineSize;
        public int startAmmo;

        public float reloadSpeed;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is GunSaveData gunData)
            {
                id = gunData.id;
                name = gunData.name;

                damage = gunData.damage;
                penetration = gunData.penetration;
                shootSpeed = gunData.shootSpeed;

                currentAmmo = gunData.currentAmmo;
                remainingAmmo = gunData.remainingAmmo;
                magazineSize = gunData.magazineSize;
                startAmmo = gunData.startAmmo;

                reloadSpeed = gunData.reloadSpeed;
            }
        }
    }
}