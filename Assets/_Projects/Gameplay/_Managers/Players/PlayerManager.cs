using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Players
{
    public class PlayerManager : DontDestroyOnLoadSingleton<PlayerManager>
    {
        [SerializeField] private Player _player;
        [SerializeField] private PlayerProgress _progress;

        public Player Player
        {
            get => _player;
            protected set => _player = value;
        }

        public PlayerProgress Progress => _progress;

        private void Reset()
        {
            this.LoadComponent(out _progress);
        }

        /// <summary>
        ///     Registers the active player.
        /// </summary>
        public void RegisterPlayer(Player player)
        {
            Player = player;
        }

        /// <summary>
        ///     Clears the current player reference (e.g. when unloading scene).
        /// </summary>
        public void UnregisterPlayer(Player player)
        {
            if (Player == player)
                Player = null;
        }
    }
}
