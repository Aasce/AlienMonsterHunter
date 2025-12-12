using System;
using UnityEngine;

namespace Asce.Game.Sounds
{
    public class SFXEnableTrigger : SFXPlayerComponent
    {
        [SerializeField] private string _name;

        [Space]
        [SerializeField] private float _delay = 0f;
        [SerializeField] private bool _iStopOnDisable = true;

        public override event Action<AudioSource> OnSFXPlayed;

        private void Start()
        {
            this.Play();
        }

        private void OnEnable()
        {
            if (!this.didStart) return;
            this.Play();
        }

        private void OnDisable()
        {
            if (!_iStopOnDisable) return;
            if (_source ==  null) return;

            if (SoundManager.Instance != null)
                SoundManager.Instance.StopSFX(_source);
			_source = null;
        }

        private void Play()
        {
            _source = SoundManager.Instance.PlaySFX(_name, transform.position, _delay);
            if (_source != null) OnSFXPlayed?.Invoke(_source);
        }
    }
}
