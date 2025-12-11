using Asce.Core;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Sounds
{
    public class PlayMusicOnStart : GameComponent
    {
        [Header("Playlist")]
        [SerializeField] private List<string> _musicNames = new();

        [Header("Options")]
        [SerializeField] private bool _loopPlaylist = true;
        [SerializeField] private bool _startPlayRandom = true;

        private Cooldown _checkCooldown = new(2f);
        private int _currentIndex = 0;

        private void Start()
        {
            if (_startPlayRandom) _currentIndex = Random.Range(0, _musicNames.Count);
            this.Play();
        }

        private void Update()
        {
            _checkCooldown.Update(Time.deltaTime);
            if (_checkCooldown.IsComplete)
            {
                _checkCooldown.Reset();
                if (!SoundManager.Instance.IsMusicPlaying)
                {
                    this.Next();
                }
            }
        }

        /// <summary> Moves to the next track in the playlist. </summary>
        private void Next()
        {
            _currentIndex++;

            // End of playlist
            if (_currentIndex >= _musicNames.Count)
            {
                if (_loopPlaylist) _currentIndex = 0;
                else return;
            }

            this.Play();
        }

        /// <summary> Plays the music at the current playlist index. </summary>
        private void Play()
        {
            if (_musicNames.Count == 0) return;
            if (_currentIndex < 0 || _currentIndex >= _musicNames.Count) return; 

            string name = _musicNames[_currentIndex];
            SoundManager.Instance.PlayMusic(name);
            SoundManager.Instance.MusicSource.loop = false; // playlist handles looping manually
        }
    }
}
