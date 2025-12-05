using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Game.Progress;
using Asce.Core;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.ResultGame
{
    public class ResultGameManager : MonoBehaviourSingleton<ResultGameManager>
    {
        [SerializeField] private UIResultGameController _uiController;

        [SerializeField] private ResultShareData _resultData;

        [Space]
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private string _prepareGameSceneName;
        [SerializeField] private string _mainGameSceneName;

        public UIResultGameController UIController => _uiController;
        public ResultShareData ResultData => _resultData;

        private void Start()
        {
            this.Initialize();

            if (!GameManager.Instance.Shared.TryGet<ResultShareData>("ResultGame", out _resultData))
            {
                Debug.Log("[ResultGameManager] ResultShareData is null");
            }
            UIController.AssignUI();
        }

        private void Initialize()
        {
            UIController.Initialize();
        }

        public void PlayAgain()
        {
            this.SaveAll();
            SaveLoadCurrentGameController currentGameController = SaveLoadManager.Instance.GetController("Current Game") as SaveLoadCurrentGameController;
            if (currentGameController != null) currentGameController.ClearCurrentGame();
            GameManager.Instance.Shared.SetOrAdd("NewGame", true);
            SceneLoader.Instance.Load(_prepareGameSceneName, delay: 0.5f);
        }

        public void PlayNewGame()
        {
            this.SaveAll();
            SaveLoadCurrentGameController currentGameController = SaveLoadManager.Instance.GetController("Current Game") as SaveLoadCurrentGameController;
            if (currentGameController != null) currentGameController.ClearCurrentGame();
            GameManager.Instance.Shared.SetOrAdd("NewGame", true);
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