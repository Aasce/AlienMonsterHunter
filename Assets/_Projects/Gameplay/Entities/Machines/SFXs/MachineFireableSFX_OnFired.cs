using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Sounds;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public class MachineFireableSFX_OnFired : GameComponent
    {
        [SerializeField, Readonly] private Machine _machine;
        [SerializeField, Readonly] private SFXPlayer _sfxPlayer;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _sfxPlayer);
            if (this.LoadComponent(out _machine))
            {
                if (_machine is not IMachineFireable) 
                    Debug.Log("[MachineFireableSFX_OnFired] Machine is not IMachineFireable", this);
            }
        }

        private void Start()
        {
            if (_machine is IMachineFireable fireable)
            {
                fireable.OnFired += MachineFireable_OnFired;
            }
        }

        private void MachineFireable_OnFired(Vector2 position, Vector2 direction)
        {
            _sfxPlayer.Play();
        }
    }
}
