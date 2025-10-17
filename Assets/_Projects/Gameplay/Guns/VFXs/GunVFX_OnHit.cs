using Asce.Game.VFXs;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Guns
{
    [RequireComponent(typeof(Gun))]
    public class GunVFX_OnHit : GameComponent
    {
        [SerializeField, Readonly] private Gun _gun;
        [SerializeField] private string _gunHitVFXName = string.Empty;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _gun);
        }

        private void Start()
        {
            if (_gun == null) return;
            _gun.OnHit += Gun_OnHit;
        }

        private void Gun_OnHit(Vector2 hitPosition, Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            VFXController.Instance.Spawn(_gunHitVFXName, hitPosition, angle);
        }
    }
}
