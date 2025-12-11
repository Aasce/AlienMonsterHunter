using Asce.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Sounds
{
    /// <summary>
    ///     Plays a list of music tracks sequentially like a playlist.
    ///     Automatically moves to the next track when the current one finishes.
    /// </summary>
    public class PlaylistMusicPlayer : GameComponent
    {
        [Header("Playlist")]
        [SerializeField] private List<string> _musicNames = new();

        [Header("Options")]
        [SerializeField] private bool _loopPlaylist = true;
        [SerializeField] private bool _forceReplayOnSceneLoad = false;

        private int _currentIndex = 0;
        private AudioSource _currentSource;

        private void Start()
        {
            if (_musicNames == null || _musicNames.Count == 0)
                return;

            if (_forceReplayOnSceneLoad)
                _currentIndex = 0;

            this.Play();
        }

        private void Update()
        {
            if (_currentSource != null && !_currentSource.isPlaying)
            {
                this.Next();
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
            string name = _musicNames[_currentIndex];
            _currentSource = SoundManager.Instance.PlayMusic(name);

            if (_currentSource != null)
                _currentSource.loop = false; // playlist handles looping manually
        }

    }
}
