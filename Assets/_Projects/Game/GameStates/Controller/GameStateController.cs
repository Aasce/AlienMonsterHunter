using Asce.Managers;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.MainGame.Managers
{
    public class GameStateController : GameComponent
    {
        [SerializeField] private MainGameState _gameState = MainGameState.None;

        [Space]
        [SerializeField] private WinCondition _winCondition;
        [SerializeField] private LoseCondition _loseCondition;

        [Space]
        [SerializeField] private Cooldown _checkCooldown = new(1f);

        public event Action<ValueChangedEventArgs<MainGameState>> OnGameStateChanged;
        public event Action OnVictory;
        public event Action OnDefeat;
        public event Action OnEndGame;

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

        public virtual void Initialize()
        {
            if (_winCondition != null) _winCondition.Initialize();
            if (_loseCondition != null) _loseCondition.Initialize();
        }

        private void Update()
        {
            _checkCooldown.Update(Time.deltaTime);
            if (_checkCooldown.IsComplete)
            {
                _checkCooldown.Reset();

                if (_winCondition != null)
                {
                    _winCondition.OnCheck();
                    if (_winCondition.IsSatisfied())
                    {
                        this.ToVictory();
                        return;
                    }
                }

                if (_loseCondition != null)
                {
                    _loseCondition.OnCheck();
                    if (_loseCondition.IsSatisfied())
                    {
                        this.ToDefeat();
                        return;
                    }
                }
            }
        }

        public void ToVictory()
        {
            GameState = MainGameState.Completed;
            OnVictory?.Invoke();
            OnEndGame?.Invoke();
        }

        public void ToDefeat()
        {
            GameState = MainGameState.Failed;
            OnDefeat?.Invoke();
            OnEndGame?.Invoke();
        }
    }
}
