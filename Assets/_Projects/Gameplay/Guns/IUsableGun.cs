using UnityEngine;

namespace Asce.Game.Guns
{
    public interface IUsableGun
    {
        public Gun Gun { get; set; }

        public void Fire();
        public void AltFire();
        public void Reload();
    }
}
