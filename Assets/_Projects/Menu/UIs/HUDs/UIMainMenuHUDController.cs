using Asce.Game.SaveLoads;
using Asce.Game.UIs.HUDs;
using Asce.Game.UIs.Panels;
using Asce.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Menu.UIs.HUDs
{
    public class UIMainMenuHUDController : UIHUDController
    {
        [SerializeField] private Button _playButton;

        private void Start()
        {
            if (_playButton != null) _playButton.onClick.AddListener(PlayButton_OnClick);
            Shared.Remove("NewGame");
        }

        private void PlayButton_OnClick()
        {
            CurrentGameConfigData gameConfigData = SaveLoadManager.Instance.Load<CurrentGameConfigData>("CurrentGameConfig");
            if (gameConfigData == null || !gameConfigData.hasSave)
            {
                MenuManager.Instance.PlayNewGame();
                return;
            }

            UIConfirmPanel confirmPanel = UIMainMenuController.Instance.PanelController.GetPanelByName("Confirm") as UIConfirmPanel;
            if (confirmPanel == null)
            {
                MenuManager.Instance.PlayGame();
                return;
            }

            confirmPanel.Set(
                title: "Game Progress",
                description: "Do you want to continue your previous game or start a new one?",
                yesText: "Continue",
                noText: "New Game",
                onYes: () => MenuManager.Instance.PlayGame(),
                onNo: () => MenuManager.Instance.PlayNewGame()
            );

            confirmPanel.Show();
        }
    }
}