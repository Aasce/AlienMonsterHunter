using Asce.Game.Managers;
using Asce.Core;
using Asce.Core.Utils;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Asce.MainGame.Managers
{
    public class GameStateController : ControllerComponent
    {
        [SerializeField] private MainGameState _gameState = MainGameState.None;
        [SerializeField] private GameResultType _gameResultType = GameResultType.Unknown;

        [Space]
        [SerializeField] private List<WinCondition> _winConditions = new();
        [SerializeField] private List<LoseCondition> _loseConditions = new();

        [Space]
        [SerializeField] private Cooldown _checkCooldown = new(1f);

        public event Action<ValueChangedEventArgs<MainGameState>> OnGameStateChanged;
        public event Action OnVictory;
        public event Action OnDefeat;
        public event Action OnEndGame;

        public override string ControllerName => "Game State";
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
        public GameResultType GameResultType => _gameResultType;
        public List<WinCondition> WinConditions => _winConditions;
        public List<LoseCondition> LoseConditions => _loseConditions;

        public bool IsPlaying => GameState == MainGameState.Playing || GameState == MainGameState.Pausing;
        public override void Initialize()
        {
            base.Initialize();
            foreach (var winCondition in _winConditions)
            {
                if (winCondition != null) winCondition.Initialize();
            }
            foreach (var loseCondition in _loseConditions)
            {
                if (loseCondition != null) loseCondition.Initialize();
            }
        }

        private void Update()
        {
            if (GameState != MainGameState.Playing) return;

            _checkCooldown.Update(Time.deltaTime);
            if (!_checkCooldown.IsComplete) return;
            _checkCooldown.Reset();
            foreach (var winCondition in _winConditions)
            {
                if (winCondition == null) continue;
                winCondition.OnCheck();
                if (winCondition.IsSatisfied())
                {
                    this.ToVictory();
                    return;
                }
            }

            foreach (var loseCondition in _loseConditions)
            {
                if (loseCondition == null) continue;
                loseCondition.OnCheck();
                if (loseCondition.IsSatisfied())
                {
                    this.ToDefeat();
                    return;
                }
            }
        }

        public void ToVictory()
        {
            GameState = MainGameState.Completed;
            _gameResultType = GameResultType.Victory;
            OnVictory?.Invoke();
            OnEndGame?.Invoke();
        }

        public void ToDefeat()
        {
            GameState = MainGameState.Failed;
            _gameResultType = GameResultType.Defeat;
            OnDefeat?.Invoke();
            OnEndGame?.Invoke();
        }
    }
}
