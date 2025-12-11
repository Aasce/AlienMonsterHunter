using Asce.Game.Managers;
using Asce.Game.UIs.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UIMenuSettingsPanel : UISettingsPanel
    {
        [Header("Main Menu")]
        [SerializeField] private Button _quitButton;


        public override void Initialize()
        {
            base.Initialize();
            _quitButton.onClick.AddListener(() =>
            {
                UIConfirmPanel confirmPanel = MainMenuManager.Instance.UIController.PanelController.GetPanelByName("Confirm") as UIConfirmPanel;
                if (confirmPanel == null) return;
                confirmPanel.Show(
                    title: "Quit Game",
                    description: "Are you sure you want to quit the game?",
                    onYes: () =>
                    {
                        this.Hide();
                        GameManager.Instance.QuitGame();
                    },
                    onNo: () => { }
                );
            });
        }
    }
}
