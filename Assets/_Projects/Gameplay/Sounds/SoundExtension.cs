using UnityEngine;

namespace Asce.Game.Sounds
{
    public static class SoundExtension
    {
        public static void ApplyParameters(this AudioSource source, SO_SoundParameters parameters)
        {
            if (source == null) return;
            if (parameters == null) return;

            source.gameObject.name = parameters.Name;
            source.clip = parameters.Clip;

            source.playOnAwake = parameters.PlayOnAwake;
            source.loop = parameters.Loop;

            // Apply volume with settings
            float baseVolume = parameters.Volume;

            float musicVolume = SoundManager.Instance.Settings.MixedMusicVolume;
            float sfxVolume = SoundManager.Instance.Settings.MixedSFXVolume;
            float categoryVolume = parameters.IsMusic ? musicVolume : sfxVolume;
            baseVolume *= categoryVolume;
            
            source.volume = baseVolume;
            source.pitch = parameters.Pitch;

            source.spatialBlend = parameters.SpatialBlend;
            source.rolloffMode = parameters.VolumeRolloff;
            source.maxDistance = parameters.MaxDistance;
            source.minDistance = parameters.MinDistance;
        }

    }
}