using Asce.Game.UIs.Panels;
using Asce.MainGame.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainGame.UIs.Panels
{

    public class UIGameVictoryPanel : UIPanel
    {
        [SerializeField] private Button _backMenuButton;
        [SerializeField] private Button _playAgainButton;
        [SerializeField] private Button _nextGameButton;

        protected override void Reset()
        {
            base.Reset();
            _name = "Game Victory";
        }

        public override void Initialize()
        {
            base.Initialize();
            _backMenuButton.onClick.AddListener(BackMenuButton_OnClick);
            _playAgainButton.onClick.AddListener(PlayAgainButton_OnClick);
            _nextGameButton.onClick.AddListener(NextGameButton_OnClick);
        }

        private void BackMenuButton_OnClick()
        {
            MainGameManager.Instance.BackToMainMenu();
        }

        private void PlayAgainButton_OnClick()
        {
            MainGameManager.Instance.BackToMainMenu();
        }

        private void NextGameButton_OnClick()
        {
            MainGameManager.Instance.BackToMainMenu();
        }

    }
}
