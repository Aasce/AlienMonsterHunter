using Asce.Core;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Sounds
{
    public class SoundSettings : GameComponent, ISaveable<SoundSettingsSaveData>, ILoadable<SoundSettingsSaveData>
    {
        public const float MAX_VOLUME = 2f;

        [SerializeField, Range(0f, 2f)] protected float _masterVolume = 1f;
        [SerializeField, Range(0f, 2f)] protected float _musicVolume = 1f;
        [SerializeField, Range(0f, 2f)] protected float _sfxVolume = 1f;

        [Space]
        [SerializeField] private float _crossfadeDuration = 1.5f;
        [SerializeField] private float _crossSlientDuration = 0f;

        public event Action<float> OnMasterVolumeChanged;
        public event Action<float> OnMusicVolumeChanged;
        public event Action<float> OnSFXVolumeChanged;

        public float MixedMusicVolume => _musicVolume * _masterVolume;
        public float MixedSFXVolume => _sfxVolume * _masterVolume;

        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                if (Mathf.Approximately(_masterVolume, value)) return;
                _masterVolume = Mathf.Clamp(value, 0f, MAX_VOLUME);
                OnMasterVolumeChanged?.Invoke(_masterVolume);
            }
        }


        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                if (Mathf.Approximately(_musicVolume, value)) return;
                _musicVolume = Mathf.Clamp(value, 0f, MAX_VOLUME);
                OnMusicVolumeChanged?.Invoke(_musicVolume);
            }
        }


        public float SFXVolume
        {
            get => _sfxVolume;
            set
            {
                if (Mathf.Approximately(_sfxVolume, value)) return;
                _sfxVolume = Mathf.Clamp(value, 0f, MAX_VOLUME);
                OnSFXVolumeChanged?.Invoke(_sfxVolume);
            }
        }

        public float CrossFadeDuration => _crossfadeDuration;
        public float CrossSlientDuration => _crossSlientDuration;

        SoundSettingsSaveData ISaveable<SoundSettingsSaveData>.Save()
        {
            SoundSettingsSaveData settingsData = new()
            {
                masterVolume = _masterVolume,
                musicVolume = _musicVolume,
                sfxVolume = _sfxVolume,
            };
            return settingsData;
        }

        void ILoadable<SoundSettingsSaveData>.Load(SoundSettingsSaveData data)
        {
            if (data == null) return;
            _masterVolume = data.masterVolume;
            _musicVolume = data.musicVolume;
            _sfxVolume = data.sfxVolume;
        }

    }
}