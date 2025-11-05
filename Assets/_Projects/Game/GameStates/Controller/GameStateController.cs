using Asce.Managers;
using Asce.Managers.Utils;
using System;
using UnityEngine;

namespace Asce.Game.Managers
{
    public class GameStateController : MonoBehaviourSingleton<GameStateController>
    {
        [SerializeField] private WinCondition _winCondition;
        [SerializeField] private LoseCondition _loseCondition;

        [Space]
        [SerializeField] private Cooldown _checkCooldown = new(1f);

        public event Action OnVictory;
        public event Action OnDefeat;

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
            MainGameManager.Instance.GameState = MainGameState.Completed;
            OnVictory?.Invoke();
        }

        public void ToDefeat()
        {
            MainGameManager.Instance.GameState = MainGameState.Failed;
            OnDefeat?.Invoke();
        }
    }
}
