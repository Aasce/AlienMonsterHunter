using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Sounds;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class HellFlamethrower_MachineSFX : GameComponent
    {
        [SerializeField, Readonly] private HellFlamethrower_Machine _machine;
        [SerializeField, Readonly] private SFXPlayer _sfxPlayer;


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _machine);
            this.LoadComponent(out _sfxPlayer);
        }

        private void Start()
        {
            _machine.OnSprayStateChanged += Machine_OnSprayStateChanged;
            _machine.OnDead += Machine_OnDead;
        }

        private void Machine_OnSprayStateChanged(bool state)
        {
            if (state) _sfxPlayer.Play();
            else _sfxPlayer.Stop();
        }

        private void Machine_OnDead(Combats.DamageContainer container)
        {
            _sfxPlayer.Stop();
        }

    }
}
