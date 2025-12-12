using System;
using UnityEngine;

namespace Asce.Game.Sounds
{
    public class SFXPlayer : SFXPlayerComponent
    {
        [SerializeField] private string _name;

        [Space]
        [SerializeField] private float _delay = 0f;

        public override event Action<AudioSource> OnSFXPlayed;

        public void Play()
        {
            _source = SoundManager.Instance.PlaySFX(_name, transform.position, _delay);
            if (_source != null) OnSFXPlayed?.Invoke(_source);
        }
    }
}
