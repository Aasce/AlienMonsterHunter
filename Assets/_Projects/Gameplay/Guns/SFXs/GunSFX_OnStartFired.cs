using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Sounds;
using System;
using UnityEngine;

namespace Asce.Game.Guns
{
    public class GunSFX_OnStartFired : GameComponent
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
            _gun.OnFireStart += Gun_OnFireStart;
            _gun.OnFireEnd += Gun_OnFireEnd;
        }

        private void Gun_OnFireStart()
        {
            _sfxPlayer.Play();
        }

        private void Gun_OnFireEnd()
        {
            _sfxPlayer.Stop();
        }
    }
}
