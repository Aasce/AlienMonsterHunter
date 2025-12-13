using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Sounds;
using UnityEngine;

namespace Asce.Game.Guns
{
    public class GunSFX_OnFired : GameComponent
    {
        [SerializeField, Readonly] private Gun _gun;
        [SerializeField, Readonly] private SFXPlayer _sfxPlayer;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _gun);
            this.LoadComponent(out _sfxPlayer);
        }

        private void Start()
        {
            if (_gun == null) return;
            _gun.OnFired += Gun_OnFired;
        }

        private void Gun_OnFired(Vector2 direction)
        {
            _sfxPlayer.Play();
        }
    }
}
