using Asce.Game.Managers;
using Asce.Managers;
using Asce.Menu.UIs;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Menu
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
            Shared.Remove("NewGame");
        }

        private void Initialize()
        {
            UIController.Initialize();
        }

        public void PlayGame()
        {
            Shared.SetOrAdd("NewGame", false);
            SceneLoader.Instance.Load(_mainGameSceneName, delay: 0.5f);
        }

        public void PlayNewGame()
        {
            SaveLoadCurrentGameController currentGameController = SaveLoadManager.Instance.GetController("Current Game") as SaveLoadCurrentGameController;
            if (currentGameController != null) currentGameController.ClearCurrentGame();
            Shared.SetOrAdd("NewGame", true);
            SceneLoader.Instance.Load(_prepareGameSceneName, delay: 0.5f);
        }
    }
}
