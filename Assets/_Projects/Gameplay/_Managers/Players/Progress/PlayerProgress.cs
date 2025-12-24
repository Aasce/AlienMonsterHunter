using Asce.Core;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Players
{
    public class PlayerProgress : GameComponent
    {
        [SerializeField] private GameProgress _gameProgress;

        [Space]
        [SerializeField] private CharactersProgress _charactersProgress;
        [SerializeField] private GunsProgress _gunsProgress;
        [SerializeField] private SupportsProgress _supportsProgress;


        public GameProgress GameProgress => _gameProgress;

        public CharactersProgress CharactersProgress => _charactersProgress;
        public GunsProgress GunsProgress => _gunsProgress;
        public SupportsProgress SupportsProgress => _supportsProgress;

        public void Initialize()
        {
            _gameProgress.Initialize();
        }

        public void SaveAll()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;
            playerProgressController.SaveAllProgress();
        }
    }
}