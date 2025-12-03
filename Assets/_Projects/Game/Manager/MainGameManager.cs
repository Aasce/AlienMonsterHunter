using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.MainGame.Players;
using Asce.MainGame.UIs;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.MainGame.Managers
{
    public class MainGameManager : MonoBehaviourSingleton<MainGameManager>
    {
        [Header("References")]
        [SerializeField, Readonly] private GameStateController _gameStateController;
        [SerializeField, Readonly] private MainGameSaveLoadController _saveLoadController;
        [SerializeField, Readonly] private NewGameController _newGameController;
        [SerializeField, Readonly] private SpawnerController _spawnerController;
        [SerializeField, Readonly] private PlayTimeController _playTimeController;
        [SerializeField, Readonly] private ResultController _resultController;

        [SerializeField] private MainGamePlayer _player;
        [SerializeField] private UIMainGameController _uiController;

        [Space]
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private string _resultGameSceneName;

        public GameStateController GameStateController => _gameStateController;
        public MainGameSaveLoadController SaveLoadController => _saveLoadController;
        public NewGameController NewGameController => _newGameController;
        public SpawnerController SpawnerController => _spawnerController;
        public PlayTimeController PlayTimeController => _playTimeController;
        public ResultController ResultController => _resultController;

        public MainGamePlayer Player => _player;
        public UIMainGameController UIController => _uiController;


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _gameStateController);
            this.LoadComponent(out _saveLoadController);
            this.LoadComponent(out _newGameController);
            this.LoadComponent(out _spawnerController);
            this.LoadComponent(out _playTimeController);
            this.LoadComponent(out _resultController);
        }

        protected override void Awake()
        {
            base.Awake();

        }

        private void Start()
        {
            this.Initialize();

            bool isNewGame = GameManager.Instance.Shared.Get<bool>("NewGame");
            if (isNewGame)
            {
                GameStateController.GameState = MainGameState.Creating;
                NewGameController.CreateNewGame();
                SaveLoadController.SaveCurrentGame();
            }
            else
            {
                GameStateController.GameState = MainGameState.Loading;
                SaveLoadController.LoadCurrentGame();
            }

            UIController.AssignUI();
            CameraController.Instance.SetToTarget();
            ResultController.OnReady();
            GameStateController.GameState = MainGameState.Playing;
        }

        private void Initialize()
        {
            GameStateController.GameState = MainGameState.Initialize;
            GameStateController.Initialize();
            NewGameController.Initialize();
            SpawnerController.Initialize();
            PlayTimeController.Initialize();
            ResultController.Initialize();
            Player.Initialize();
            UIController.Initialze();
            PlayerManager.Instance.RegisterPlayer(Player);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (PlayerManager.Instance != null) PlayerManager.Instance.UnregisterPlayer(Player);
        }

        public void BackToMainMenu()
        {
            SaveLoadController.SaveCurrentGame();
            GameStateController.GameState = MainGameState.Exiting; 
            SceneLoader.Instance.Load(_mainMenuSceneName, delay: 0.5f);
        }

        public void ToResultGame()
        {
            SaveLoadController.SaveCurrentGame();
            GameStateController.GameState = MainGameState.Exiting; 
            SceneLoader.Instance.Load(_resultGameSceneName, delay: 0.5f);
        }

    }
}