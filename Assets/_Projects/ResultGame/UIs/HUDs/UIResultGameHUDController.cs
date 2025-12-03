using Asce.Game.Managers;
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
        [SerializeField] private Button _backMenuButton;
        [SerializeField] private Button _playAgainButton;
        [SerializeField] private Button _nextGameButton;

        public UIResultTitle Title => _title;

        public override void Initialize()
        {
            base.Initialize();
            _title.Initialize();

            _backMenuButton.onClick.AddListener(BackMenuButton_OnClick);
            _playAgainButton.onClick.AddListener(PlayAgainButton_OnClick);
            _nextGameButton.onClick.AddListener(NextGameButton_OnClick);
        }

        private void BackMenuButton_OnClick()
        {
            ResultGameManager.Instance.BackToMainMenu();

        }

        private void NextGameButton_OnClick()
        {
            ResultGameManager.Instance.PlayNewGame();

        }

        private void PlayAgainButton_OnClick()
        {
            ResultGameManager.Instance.PlayGame();
        }

    }
}