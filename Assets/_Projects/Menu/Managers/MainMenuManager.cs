using Asce.Core;
using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Game.Progress;
using Asce.MainMenu.Picks;
using Asce.MainMenu.SaveLoads;
using Asce.MainMenu.UIs;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.MainMenu
{
    public class MainMenuManager : MonoBehaviourSingleton<MainMenuManager>
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

            this.SaveAll();
            SceneLoader.Instance.Load(_mainGameSceneName, delay: 0.5f);
        }

        public void PlayNewGame()
        {
            PickMapLevelShareData mapLevelData = PickController.Instance.CreateLevelShareData();
            if (string.IsNullOrEmpty(mapLevelData.MapName)) return;

            GameManager.Instance.Shared.SetOrAdd("MapLevel", mapLevelData);
            GameManager.Instance.Shared.SetOrAdd("NewGame", true);

            SaveLoadCurrentGameController currentGameController = SaveLoadManager.Instance.GetController("Current Game") as SaveLoadCurrentGameController;
            if (currentGameController != null) currentGameController.ClearCurrentGame();

            this.SaveAll();
            SceneLoader.Instance.Load(_prepareGameSceneName, delay: 0.5f);
        }


        private void SaveAll()
        {
            MainMenuSaveLoadController.Instance.SaveLastPick();
            PlayerManager.Instance.Progress.SaveAll();
            PlayerManager.Instance.Items.SaveAll();
        }

    }
}
