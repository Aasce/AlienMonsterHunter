using Asce.Game.Entities.Characters;
using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Game.UIs;
using Asce.Game.UIs.Panels;
using Asce.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game
{
    public class MainGameManager : MonoBehaviourSingleton<MainGameManager>
    {
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private MainGameState _gameState = MainGameState.None;

        public event Action<ValueChangedEventArgs<MainGameState>> OnGameStateChanged;

        public MainGameState GameState
        {
            get => _gameState;
            set
            {
                if (_gameState == value) return;
                MainGameState oldValue = _gameState;
                _gameState = value;
                OnGameStateChanged?.Invoke(new ValueChangedEventArgs<MainGameState>(oldValue, _gameState));
            }
        }

        public bool IsPlaying => GameState == MainGameState.Playing || GameState == MainGameState.Pausing;

        protected override void Awake()
        {
            base.Awake();
            GameState = MainGameState.Initialize;
        }

        private void Start()
        {
            this.InitializeController();

            bool isNewGame = Shared.Get<bool>("NewGame");
            if (isNewGame)
            {
                this.CreateCharacterForPlayer();
                this.CreateSupportForPlayer();
            }
            else
            {
                GameState = MainGameState.Loading;
                this.LoadCurrentGame();
            }
            Player.Instance.Initialize();
            this.AssignUI();

            GameState = MainGameState.Playing;
        }

        public void BackToMainMenu()
        {
            MainGameSaveLoadController.Instance.SaveCurrentGame();
            GameState = MainGameState.Exiting; 
            SceneLoader.Instance.Load(_mainMenuSceneName, delay: 0.5f);
        }

        private void InitializeController()
        {
            UIGameController.Instance.PanelController.HideAll();
        }

        private void LoadCurrentGame()
        {
            MainGameSaveLoadController.Instance.LoadCurrentGame();
        }

        private void CreateCharacterForPlayer()
        {
            string characterName = Shared.Get<string>("character");
            string gunName = Shared.Get<string>("gun");

            Gun gunPrefab = GameManager.Instance.AllGuns.Get(gunName);
            Gun gunInstance = Instantiate(gunPrefab);
            gunInstance.name = gunPrefab.name;

            Character characterPrefab = GameManager.Instance.AllCharacters.Get(characterName);
            Character characterInstance = Instantiate(characterPrefab);
            if (characterInstance != null)
            {
                characterInstance.Gun = gunInstance;
            }

            Player.Instance.Character = characterInstance;
            Player.Instance.Character.transform.position = Player.Instance.SpawnPoint;
            Player.Instance.InitializeCharacter();
        }

        private void CreateSupportForPlayer()
        {
            Player.Instance.Supports.Clear();
            List<string> supportNames = Shared.Get<List<string>>("supports");
            if (supportNames == null) return;
            Player.Instance.Supports.AddRange(supportNames);
            Player.Instance.InitializeSupportCaller();
        }

        private void AssignUI()
        {
            Player.Instance.Character.OnDead += Character_OnDead;
            Player.Instance.OnCharacterChanged += Player_OnCharacterChanged;
        }

        private void Player_OnCharacterChanged(ValueChangedEventArgs<Character> args)
        {
            if (args.OldValue != null) args.OldValue.OnDead -= Character_OnDead;
            if (args.NewValue != null)
            {
                args.NewValue.OnDead -= Character_OnDead;
                args.NewValue.OnDead += Character_OnDead;
            }
        }

        private void Character_OnDead()
        {
            GameState = MainGameState.Failed;
            Player.Instance.Character.gameObject.SetActive(false);
            UIDeathPanel deathPanel = UIGameController.Instance.PanelController.GetPanelByName("Death") as UIDeathPanel;
            if (deathPanel == null) return;

            deathPanel.OnReviveClicked -= DeathPanel_OnReviveClicked;
            deathPanel.OnReviveClicked += DeathPanel_OnReviveClicked;
            deathPanel.Show();
        }

        private void DeathPanel_OnReviveClicked()
        {
            Player.Instance.ReviveCharacter(isReviveAtSpawnPoint: true);
            GameState = MainGameState.Playing;
        }
    }
}