using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Game.Progress;
using Asce.Core;
using Asce.SaveLoads;
using UnityEngine;
using Asce.Game.Maps;

namespace Asce.ResultGame
{
    public class ResultGameManager : MonoBehaviourSingleton<ResultGameManager>
    {
        [SerializeField] private AwardController _awardController;
        [SerializeField] private UIResultGameController _uiController;

        [SerializeField] private ResultShareData _resultData;

        [Space]
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private string _prepareGameSceneName;
        [SerializeField] private string _mainGameSceneName;

        public AwardController AwardController => _awardController;
        public UIResultGameController UIController => _uiController;
        public ResultShareData ResultData => _resultData;

        private void Start()
        {
            this.Initialize();

            if (!GameManager.Instance.Shared.TryGet("ResultGame", out _resultData))
            {
                Debug.Log("[ResultGameManager] ResultShareData is null");
            }

            this.Ready();
        }

        private void Initialize()
        {
            AwardController.Initialize();
            UIController.Initialize();
        }

        private void Ready()
        {
            AwardController.Ready();
            UIController.Ready();
        }

        public void PlayAgain()
        {
            this.SaveAll();
            SaveLoadCurrentGameController currentGameController = SaveLoadManager.Instance.GetController("Current Game") as SaveLoadCurrentGameController;
            if (currentGameController != null) currentGameController.ClearCurrentGame();
            GameManager.Instance.Shared.SetOrAdd("NewGame", true);
            SceneLoader.Instance.Load(_prepareGameSceneName, delay: 0.5f);
        }

        public void PlayNextGame()
        {
            this.SaveAll();
            SaveLoadCurrentGameController currentGameController = SaveLoadManager.Instance.GetController("Current Game") as SaveLoadCurrentGameController;
            if (currentGameController != null) currentGameController.ClearCurrentGame();
            GameManager.Instance.Shared.SetOrAdd("NewGame", true);

            if (!GameManager.Instance.Shared.TryGet("MapLevel", out PickMapLevelShareData mapLevelData))
            {
                Debug.LogError("[ResultGameManager] Map Level Share Data is null", this);
                return;
            }

            Map mapPrefab = GameManager.Instance.AllMaps.Get(mapLevelData.MapName);
            SO_MapLevelInformation levelInformation = mapPrefab.Information.GetLevel(mapLevelData.Level);

            int mapLevelIndex = mapPrefab.Information.Levels.IndexOf(levelInformation);
            if (mapLevelIndex + 1 >= mapPrefab.Information.Levels.Count)
            {
                int mapIndex = GameManager.Instance.AllMaps.Maps.IndexOf(mapPrefab);
                if (mapIndex + 1 >= GameManager.Instance.AllMaps.Maps.Count)
                {
                    Debug.Log("[ResultGameManager] No more maps available. Returning to main menu.", this);
                    this.BackToMainMenu();
                    return;
                }

                Map nextMapPrefab = GameManager.Instance.AllMaps.Maps[mapIndex + 1];
                if (nextMapPrefab.Information.Levels.Count <= 0)
                {
                    Debug.Log("[ResultGameManager] Next map has no levels available.", this);
                    this.BackToMainMenu();
                    return;
                }

                SO_MapLevelInformation nextMapFirstLevelInformation = nextMapPrefab.Information.Levels[0];
                GameManager.Instance.Shared.SetOrAdd("MapLevel", new PickMapLevelShareData()
                {
                    MapName = nextMapPrefab.Information.Name,
                    Level = nextMapFirstLevelInformation.Level,
                });

                SceneLoader.Instance.Load(_prepareGameSceneName, delay: 0.5f);
                return;
            }

            SO_MapLevelInformation nextLevelInformation = mapPrefab.Information.Levels[mapLevelIndex + 1];
            GameManager.Instance.Shared.SetOrAdd("MapLevel", new PickMapLevelShareData()
            {
                MapName = mapPrefab.Information.Name,
                Level = nextLevelInformation.Level,
            });

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
            PlayerManager.Instance.Items.SaveAll();
        }
    }
}