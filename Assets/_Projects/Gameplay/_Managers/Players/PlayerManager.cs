using Asce.Core;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.Players
{
    public class PlayerManager : DontDestroyOnLoadSingleton<PlayerManager>
    {
        [SerializeField] private Player _player;
        [SerializeField] private PlayerItems _items;
        [SerializeField] private PlayerProgress _progress;

        public Player Player
        {
            get => _player;
            protected set => _player = value;
        }

        public PlayerItems Items => _items;
        public PlayerProgress Progress => _progress;

        private void Reset()
        {
            this.LoadComponent(out _items);
            this.LoadComponent(out _progress);
        }

        private void Start()
        {
            _items.Initialize();
            _progress.Initialize();

            if (_progress.GameProgress.OpenGamesCount <= 1)
            {
                this.AddFirstGameResources();
            }
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

        public void AddFirstGameResources()
        {
            foreach (var item in Progress.GameProgress.FirstGameResources.Items)
            {
                Items.Add(item.Name, (int)item.Value);
            }

            Progress.CharactersProgress.Get("Blue Soldier").Unlock(Progress.GameProgress.FirstGameResources.FirstGameStartUnlock);
            Progress.GunsProgress.Get("Frontier").Unlock(Progress.GameProgress.FirstGameResources.FirstGameStartUnlock);
        }
    }
}
