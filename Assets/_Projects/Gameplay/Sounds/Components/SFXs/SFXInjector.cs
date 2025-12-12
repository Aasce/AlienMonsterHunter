using Asce.Core;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Sounds
{
    public class SFXInjector : GameComponent
    {
        [SerializeField] private SFXEnableTrigger _trigger;

        [Space]
        [SerializeField] private List<SFXControlComponent> _controls = new();

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _trigger);
        }

        private void Awake()
        {
            _trigger.OnSFXPlayed += Trigger_OnSFXPlayed;
        }

        private void Trigger_OnSFXPlayed(AudioSource source)
        {
            foreach (var control in _controls)
            {
                control.Source = source;
            }
        }

    }
}
