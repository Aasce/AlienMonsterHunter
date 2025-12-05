using Asce.Game.Managers;
using Asce.Game.Progress;
using Asce.Core;
using UnityEngine;

namespace Asce.MainGame.Managers
{
    public class ResultController : ControllerComponent
    {
        [SerializeField] private ResultShareData _resultData = new();

        public override string ControllerName => "Result";

        public override void Initialize()
        {
            base.Initialize();
            MainGameManager.Instance.GameStateController.OnEndGame += GameStateController_OnEndGame;
        }

        public override void Ready()
        {
            base.Ready();
            _resultData.StartCharacterLevel = MainGameManager.Instance.Player.Character.Leveling.CurrentLevel;
            _resultData.StartCharacterExp = MainGameManager.Instance.Player.Character.Leveling.CurrentExp;
        }

        public void OnLoad()
        {

        }

        private void GameStateController_OnEndGame()
        {
            _resultData.FinalResult = MainGameManager.Instance.GameStateController.GameResultType;
            _resultData.ElapsedTime = MainGameManager.Instance.PlayTimeController.ElapsedTime;

            _resultData.EndCharacterLevel = MainGameManager.Instance.Player.Character.Leveling.CurrentLevel;
            _resultData.EndCharacterExp = MainGameManager.Instance.Player.Character.Leveling.CurrentExp;

            GameManager.Instance.Shared.SetOrAdd("ResultGame", _resultData);
        }

    }
}
