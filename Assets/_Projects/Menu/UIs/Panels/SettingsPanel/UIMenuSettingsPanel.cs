using Asce.Game.Managers;
using Asce.Game.UIs.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Menu.UIs.Panels
{
    public class UIMenuSettingsPanel : UISettingsPanel
    {
        [Header("Main Menu")]
        [SerializeField] private Button _quitButton;


        protected override void Start()
        {
            base.Start();
            if (_quitButton != null) _quitButton.onClick.AddListener(() =>
            {
                UIConfirmPanel confirmPanel = MenuManager.Instance.UIController.PanelController.GetPanelByName("Confirm") as UIConfirmPanel;
                if (confirmPanel == null) return;
                confirmPanel.Show(
                    title: "Quit Game",
                    description: "Are you sure you want to quit the game?",
                    onYes: () => GameManager.Instance.QuitGame(),
                    onNo: () => { }
                );
            });
        }
    }
}
