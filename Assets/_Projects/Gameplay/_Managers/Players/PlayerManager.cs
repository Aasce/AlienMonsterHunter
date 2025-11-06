using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Players
{
    public class PlayerManager : DontDestroyOnLoadSingleton<PlayerManager>
    {
        [SerializeField] private Player _player;

        public Player Player
        {
            get => _player;
            protected set => _player = value;
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
