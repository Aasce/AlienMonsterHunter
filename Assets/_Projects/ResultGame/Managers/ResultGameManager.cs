using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Managers;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.ResultGame
{
    public class ResultGameManager : MonoBehaviourSingleton<ResultGameManager>
    {
        [SerializeField] private UIResultGameController _uiController;

        [Space]
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private string _prepareGameSceneName;
        [SerializeField] private string _mainGameSceneName;

        public UIResultGameController UIController => _uiController;

        private void Start()
        {
            this.Initialize();
            Shared.Remove("NewGame");
        }

        private void Initialize()
        {
            UIController.Initialize();
        }

        public void PlayGame()
        {
            this.SaveAll();
            Shared.SetOrAdd("NewGame", false);
            SceneLoader.Instance.Load(_mainGameSceneName, delay: 0.5f);
        }

        public void PlayNewGame()
        {
            this.SaveAll();
            SaveLoadCurrentGameController currentGameController = SaveLoadManager.Instance.GetController("Current Game") as SaveLoadCurrentGameController;
            if (currentGameController != null) currentGameController.ClearCurrentGame();
            Shared.SetOrAdd("NewGame", true);
            SceneLoader.Instance.Load(_prepareGameSceneName, delay: 0.5f);
        }

        public void BackToMainMenu()
        {
            this.SaveAll();
            SceneLoader.Instance.Load(_mainMenuSceneName, delay: 0.5f);
        }

        private void SaveAll()
        {
            PlayerManager.Instance.Progress.SaveAll();
            PlayerManager.Instance.Currencies.SaveAll();
        }
    }
}