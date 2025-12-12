using UnityEngine;

namespace Asce.Game.Sounds
{
    public static class SoundExtension
    {
        public static void ApplyParameters(this AudioSource source, SO_SoundParameters parameters)
        {
            if (source == null || parameters == null) return;

            source.gameObject.name = parameters.Name;
            source.clip = parameters.Clip;

            source.playOnAwake = parameters.PlayOnAwake;
            source.loop = parameters.Loop;

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

            source.panStereo = parameters.StereoPan;
            source.reverbZoneMix = parameters.ReverbZoneMix;
            source.dopplerLevel = parameters.DopplerLevel;
            source.spread = parameters.Spread;
            source.priority = parameters.Priority;

            source.bypassEffects = parameters.BypassEffects;
            source.bypassListenerEffects = parameters.BypassListenerEffects;
            source.bypassReverbZones = parameters.BypassReverbZones;
        }


    }
}