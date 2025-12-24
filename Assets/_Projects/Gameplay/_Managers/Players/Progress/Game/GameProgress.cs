using Asce.Core;
using Asce.Core.Attributes;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Players
{
    public class GameProgress : GameComponent, ISaveable<GameProgressSaveData>, ILoadable<GameProgressSaveData>
    {
        [SerializeField] private SO_FirstGameResources _firstGameResources;

        [Space]
        [SerializeField, Readonly] private int _openGamesCount = 0;

        public SO_FirstGameResources FirstGameResources => _firstGameResources;
        public int OpenGamesCount => _openGamesCount;

        public void Initialize()
        {
            this.LoadProgress();
            _openGamesCount++;
            this.SaveProgress();
        }

        public virtual void SaveProgress()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.SaveGameProgress(this);
        }

        public virtual void LoadProgress()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.LoadGameProgress(this);
        }

        GameProgressSaveData ISaveable<GameProgressSaveData>.Save()
        {
            return new GameProgressSaveData
            {
                openGamesCount = _openGamesCount,
            };
        }

        void ILoadable<GameProgressSaveData>.Load(GameProgressSaveData data)
        {
            if (data == null) return;
            _openGamesCount = data.openGamesCount;
        }
    }
}
