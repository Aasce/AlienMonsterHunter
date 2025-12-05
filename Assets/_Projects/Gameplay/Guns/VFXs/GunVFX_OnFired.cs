using Asce.Game.VFXs;
using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.Guns
{
    [RequireComponent(typeof(Gun))]
    public class GunVFX_OnFired : GameComponent
    {
        [SerializeField, Readonly] private Gun _gun;
        [SerializeField] private string _gunFiredVFXName = string.Empty;
        [SerializeField, Readonly] private VFXObject _muzzleFlashVFX;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _gun);
        }

        private void Start()
        {
            if (_gun == null) return;
            _gun.OnFired += Gun_OnFired;
        }

        private void LateUpdate()
        {
            if (_muzzleFlashVFX == null) return;
            Vector2 direction = _gun.transform.up;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            _muzzleFlashVFX.transform.SetPositionAndRotation(_gun.BarrelPosition, Quaternion.Euler(0f, 0f, angle));
        }

        private void Gun_OnFired(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            _muzzleFlashVFX = VFXController.Instance.Spawn(_gunFiredVFXName, _gun.BarrelPosition, angle);
        }
    }
}
