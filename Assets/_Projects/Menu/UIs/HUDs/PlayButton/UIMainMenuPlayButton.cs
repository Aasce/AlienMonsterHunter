using Asce.Core.UIs;
using Asce.Game.UIs.Panels;
using Asce.SaveLoads;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.HUDs
{
    public class UIMainMenuPlayButton : UIComponent
    {
        [SerializeField] private Button _playButton;

        public virtual void Initialize()
        {
            _playButton.onClick.AddListener(PlayButton_OnClick);
        }

        private void PlayButton_OnClick()
        {
            SaveLoadCurrentGameController currentGameController = SaveLoadManager.Instance.GetController("Current Game") as SaveLoadCurrentGameController;
            if (currentGameController == null || !currentGameController.HasSaveCurrentGame())
            {
                // No Current Game Save -> open Choice Level to play New Game
                OpenChoiceLevelPanel();
                return;
            }

            UIConfirmPanel confirmPanel = MenuManager.Instance.UIController.PanelController.GetPanelByName("Confirm") as UIConfirmPanel;
            if (confirmPanel == null)
            {
                // No Confirm Panel -> play Current Game (Because Current Game Save exits).
                MenuManager.Instance.PlayGame();
                return;
            }

            confirmPanel.Set(
                title: "Game Progress",
                description: "Do you want to continue your previous game or start a new one?",

                yesText: "Continue",
                onYes: () => MenuManager.Instance.PlayGame(),

                noText: "New Game",
                onNo: () => OpenChoiceLevelPanel()
            );

            confirmPanel.Show();
        }

        private void OpenChoiceLevelPanel()
        {
            UIPanel choiceLevelPanel = MenuManager.Instance.UIController.PanelController.GetPanelByName("Choice Level");
            if (choiceLevelPanel == null) return;
            choiceLevelPanel.Show();
        }
    }
}
