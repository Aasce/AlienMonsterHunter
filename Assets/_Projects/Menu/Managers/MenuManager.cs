using Asce.Game.Managers;
using Asce.Core;
using Asce.MainMenu.UIs;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.MainMenu
{
    public class MenuManager : MonoBehaviourSingleton<MenuManager>
    {
        [SerializeField] private UIMainMenuController _uiController;

        [Space]
        [SerializeField] private string _mainGameSceneName;
        [SerializeField] private string _prepareGameSceneName;

        public UIMainMenuController UIController => _uiController;

        private void Start()
        {
            this.Initialize();
            GameManager.Instance.Shared.Remove("NewGame");
        }

        private void Initialize()
        {
            UIController.Initialize();
        }

        public void PlayGame()
        {
            GameManager.Instance.Shared.SetOrAdd("NewGame", false);
            SceneLoader.Instance.Load(_mainGameSceneName, delay: 0.5f);
        }

        public void PlayNewGame()
        {
            SaveLoadCurrentGameController currentGameController = SaveLoadManager.Instance.GetController("Current Game") as SaveLoadCurrentGameController;
            if (currentGameController != null) currentGameController.ClearCurrentGame();
            GameManager.Instance.Shared.SetOrAdd("NewGame", true);
            SceneLoader.Instance.Load(_prepareGameSceneName, delay: 0.5f);
        }
    }
}
