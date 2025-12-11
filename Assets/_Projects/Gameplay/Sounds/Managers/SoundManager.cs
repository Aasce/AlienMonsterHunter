using Asce.Core;
using Asce.Core.Utils;
using Asce.Game.Managers;
using Asce.SaveLoads;
using System.Collections;
using UnityEngine;

namespace Asce.Game.Sounds
{
    public class SoundManager : DontDestroyOnLoadSingleton<SoundManager>
    {
        [SerializeField] private SoundSettings _settings;

        [Space]
        [SerializeField] protected AudioSource _musicSource;
        [SerializeField] private Pool<AudioSource> _sfxs = new();

        private Coroutine _crossfadeCoroutine;

        public SoundSettings Settings => _settings;
        public AudioSource MusicSource => _musicSource;

        public bool IsMusicPlaying => _musicSource != null && _musicSource.isPlaying;

        protected void Start()
        {
            _settings.OnMasterVolumeChanged += (newVolume) => this.ApplyVolumes();
            _settings.OnMusicVolumeChanged += (newVolume) => this.ApplyMusicVolumes();
            _settings.OnSFXVolumeChanged += (newVolume) => this.ApplySFXVolumes();
        }

        public AudioSource PlayMusic(string name)
        {
            SO_SoundParameters parameters = GameManager.Instance.AllSounds.Get(name);
            if (parameters == null || _musicSource == null) return null;

            _musicSource.ApplyParameters(parameters);
            _musicSource.Play();
            return _musicSource;
        }

        public AudioSource PlaySFX(string name, Vector2 position = default, float delay = 0f)
        {
            SO_SoundParameters parameters = GameManager.Instance.AllSounds.Get(name);
            if (parameters == null) return null;

            AudioSource source = _sfxs.Activate();
            if (source == null) return null;

            source.ApplyParameters(parameters);
            source.transform.position = position;
            if (!parameters.Loop) this.StartCoroutine(this.AutoStopSFX(source));

            if (delay <= 0f) source.Play();
            else this.StartCoroutine(PlayDelay(source, delay));

            return source;
        }

        protected void StopSFX(AudioSource source)
        {
            if (source == null) return;
            if (source.clip == null) return;

            source.Stop();
            _sfxs.Deactivate(source);
        }

        public void CrossFadeMusic(string name, float? fadeDuration = null, float? slientDuration = null)
        {
            var parameters = GameManager.Instance.AllSounds.Get(name);
            if (parameters == null) return;

            if (_musicSource.clip == parameters.Clip) return;

            if (_crossfadeCoroutine != null)
                StopCoroutine(_crossfadeCoroutine);

            float fadeTime = fadeDuration ?? _settings.CrossFadeDuration;
            float slientTime = slientDuration ?? _settings.CrossSlientDuration;
            _crossfadeCoroutine = StartCoroutine(CrossFadeRoutine(parameters, fadeTime, slientTime));
        }

        public void ApplyVolumes()
        {
            this.ApplyMusicVolumes();
            this.ApplySFXVolumes();
        }

        public void ApplyMusicVolumes()
        {
            if (_musicSource != null && _musicSource.clip != null)
            {
                SO_SoundParameters musicParams = GameManager.Instance.AllSounds.Get(_musicSource.gameObject.name);
                if (musicParams != null)
                {
                    float volume = musicParams.Volume * _settings.MusicVolume * _settings.MasterVolume;
                    _musicSource.volume = volume;
                }
            }
        }

        public void ApplySFXVolumes()
        {
            foreach (var sfx in _sfxs.Activities)
            {
                if (sfx == null || sfx.clip == null) continue;

                SO_SoundParameters sfxParams = GameManager.Instance.AllSounds.Get(sfx.gameObject.name);
                if (sfxParams == null) continue;

                float volume = sfxParams.Volume * _settings.MusicVolume * _settings.MasterVolume;
                sfx.volume = volume;
            }
        }

        protected IEnumerator CrossFadeRoutine(SO_SoundParameters newMusic, float fadeDuration, float slientDuration)
        {
            float startVolume = _musicSource.volume;

            // Fade out
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }

            _musicSource.ApplyParameters(newMusic);
            if (slientDuration > 0) yield return new WaitForSeconds(slientDuration);
            _musicSource.Play();

            // Fade in
            t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(0f, newMusic.Volume, t / fadeDuration);
                yield return null;
            }

            _crossfadeCoroutine = null;
        }

        protected virtual IEnumerator PlayDelay(AudioSource source, float delay)
        {
            if (source == null) yield break;
            if (delay > 0f) yield return new WaitForSeconds(delay);

            source.Play();
        }

        protected IEnumerator AutoStopSFX(AudioSource source)
        {
            if (source == null) yield break;
            if (source.clip == null) yield break;
            yield return new WaitForSeconds(source.clip.length);

            this.StopSFX(source);
        }
    }
}
