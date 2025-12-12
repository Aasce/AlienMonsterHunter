using UnityEngine;

namespace Asce.Game.Sounds
{
    [CreateAssetMenu(menuName = "Asce/Sounds/Sound Parameters", fileName = "Sound Parameters")]
    public class SO_SoundParameters : ScriptableObject
    {
        [SerializeField] protected string _name;
        [SerializeField] protected AudioClip _clip;
        [SerializeField] protected bool _isMusic = false;

        [Header("Parameters")]
        [SerializeField] protected bool _loop = false;
        [SerializeField] protected bool _playOnAwake = true;
        [SerializeField, Range(0, 256)] protected int _priority = 128;

        [SerializeField, Range(0f, 2f)] protected float _volume = 1.0f;
        [SerializeField, Range(-3f, 3f)] protected float _pitch = 1.0f;

        [Space]
        [SerializeField, Range(0f, 1f)] protected float _spatialBlend = 1.0f;
        [SerializeField] protected AudioRolloffMode _volumeRolloff = AudioRolloffMode.Logarithmic;
        [SerializeField, Min(0f)] protected float _minDistance = 1.0f;
        [SerializeField, Min(0f)] protected float _maxDistance = 1.0f;

        [SerializeField, Range(-1f, 1f)] protected float _stereoPan = 0f;
        [SerializeField, Range(0f, 1.1f)] protected float _reverbZoneMix = 1f;
        [SerializeField, Range(0f, 5f)] protected float _dopplerLevel = 1f;
        [SerializeField, Range(0f, 360f)] protected float _spread = 0f;

        [Space]
        [SerializeField] protected bool _bypassEffects = false;
        [SerializeField] protected bool _bypassListenerEffects = false;
        [SerializeField] protected bool _bypassReverbZones = false;

        public string Name => _name;
        public AudioClip Clip => _clip;
        public bool IsMusic => _isMusic;

        public bool Loop => _loop;
        public bool PlayOnAwake => _playOnAwake;
        public int Priority => _priority;

        public float Volume => _volume;
        public float Pitch => _pitch;

        public float SpatialBlend => _spatialBlend;
        public AudioRolloffMode VolumeRolloff => _volumeRolloff;
        public float MinDistance => _minDistance;
        public float MaxDistance => _maxDistance;

        public float StereoPan => _stereoPan;
        public float ReverbZoneMix => _reverbZoneMix;
        public float DopplerLevel => _dopplerLevel;
        public float Spread => _spread;

        public bool BypassEffects => _bypassEffects;
        public bool BypassListenerEffects => _bypassListenerEffects;
        public bool BypassReverbZones => _bypassReverbZones;

    }
}
