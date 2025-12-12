using Asce.Core;
using System;
using UnityEngine;

namespace Asce.Game.Sounds
{
    public class SFXDisableTrigger : SFXPlayerComponent
    {
        [SerializeField] private string _name;

        [Space]
        [SerializeField] private float _delay = 0f;

        public override event Action<AudioSource> OnSFXPlayed;

        private void OnDisable()
        {
            if (ApplicationState.isQuitting) return;
            this.Play();
        }

        private void Play()
        {
            _source = SoundManager.Instance.PlaySFX(_name, transform.position, _delay);
            if (_source != null) OnSFXPlayed?.Invoke(_source);
        }
    }
}
