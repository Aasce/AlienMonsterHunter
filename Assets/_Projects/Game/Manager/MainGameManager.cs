using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.MainGame.Players;
using Asce.MainGame.UIs;
using Asce.Core;
using UnityEngine;
using System.Collections.ObjectModel;

namespace Asce.MainGame.Managers
{
    public class MainGameManager : MonoBehaviourSingleton<MainGameManager>
    {
        [Header("References")]
        [SerializeField] private ListObjects<string, ControllerComponent> _controllers = new((controller) =>
        {
            if (controller == null) return null;
            return controller.ControllerName;
        });

        [Space]
        [SerializeField] private MainGamePlayer _player;

        [Space]
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private string _resultGameSceneName;

        public ReadOnlyCollection<ControllerComponent> Controllers => _controllers.List;
        public MainGamePlayer Player => _player;

        public ControllerComponent GetController(string name) => this.GetController<ControllerComponent>(name);
        public T GetController<T>(string name) where T : ControllerComponent
        {
            return _controllers.Get(name) as T;
        }

        public GameStateController GameStateController => this.GetController<GameStateController>("Game State");
        public MainGameSaveLoadController SaveLoadController => this.GetController<MainGameSaveLoadController>("Save Load");
        public NewGameController NewGameController => this.GetController<NewGameController>("New Game");
        public SpawnerController SpawnerController => this.GetController<SpawnerController>("Spawner");
        public PlaytimeController PlayTimeController => this.GetController<PlaytimeController>("Playtime");
        public ResultController ResultController => this.GetController<ResultController>("Result");
        public UIMainGameController UIController => this.GetController<UIMainGameController>("UI");

        protected override void Awake()
        {
            base.Awake();

        }

        private void Start()
        {
            this.Initialize();
            this.CreateOrLoad();
            this.Ready();

            CameraController.Instance.SetToTarget();
            GameStateController.GameState = MainGameState.Playing;
        }

        private void Initialize()
        {
            GameStateController.GameState = MainGameState.Initialize;
            PlayerManager.Instance.RegisterPlayer(Player);

            foreach (ControllerComponent controller in Controllers)
            {
                controller.Initialize();
            }
            Player.Initialize();
        }

        private void CreateOrLoad()
        {
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
        }

        private void Ready()
        {
            GameStateController.GameState = MainGameState.Ready;
            foreach (ControllerComponent controller in Controllers)
            {
                controller.Ready();
            }
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