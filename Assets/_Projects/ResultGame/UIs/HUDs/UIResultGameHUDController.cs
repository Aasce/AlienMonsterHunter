using Asce.Game.Managers;
using Asce.Game.Progress;
using Asce.Game.UIs.HUDs;
using Asce.ResultGame.UIs.HUDs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.ResultGame
{
    public class UIResultGameHUDController : UIHUDController
    {
        [SerializeField] private UIResultTitle _title;
        [SerializeField] private UIMainResults _mainResults;
        [SerializeField] private Button _backMenuButton;
        [SerializeField] private Button _playAgainButton;
        [SerializeField] private Button _nextGameButton;

        public UIResultTitle Title => _title;
        public UIMainResults MainResults => _mainResults;

        public override void Initialize()
        {
            base.Initialize();
            _title.Initialize();
            _mainResults.Initialize();

            _backMenuButton.onClick.AddListener(BackMenuButton_OnClick);
            _playAgainButton.onClick.AddListener(PlayAgainButton_OnClick);
            _nextGameButton.onClick.AddListener(NextGameButton_OnClick);
        }

        public override void Ready()
        {
            base.Ready();
            ResultShareData resultData = ResultGameManager.Instance.ResultData;
            if (resultData == null) Title.Set(GameResultType.Unknown);
            else Title.Set(resultData.FinalResult);

            if (resultData.FinalResult == GameResultType.Victory)
            {
                _nextGameButton.gameObject.SetActive(true);
            }
            else
            {
                _nextGameButton.gameObject.SetActive(false);
            }

            _mainResults.Ready();
        }

        private void BackMenuButton_OnClick()
        {
            ResultGameManager.Instance.BackToMainMenu();

        }

        private void NextGameButton_OnClick()
        {
            ResultGameManager.Instance.PlayNextGame();

        }

        private void PlayAgainButton_OnClick()
        {
            ResultGameManager.Instance.PlayAgain();
        }

    }
}