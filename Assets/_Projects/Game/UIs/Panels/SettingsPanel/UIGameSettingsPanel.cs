using Asce.Game.UIs.Panels;
using Asce.MainGame.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainGame.UIs.Panels
{
    public class UIGameSettingsPanel : UISettingsPanel
    {
        [Header("Prepare Game")]
        [SerializeField] private Button _backMenuButton;


        protected override void Start()
        {
            base.Start();
            if (_backMenuButton != null) _backMenuButton.onClick.AddListener(() =>
            {
                UIConfirmPanel confirmPanel = MainGameManager.Instance.UIController.PanelController.GetPanelByName("Confirm") as UIConfirmPanel;
                if (confirmPanel == null) return;
                confirmPanel.Show(
                    title: "Back to Menu?",
                    description: "Are you sure you want to back to menu?",
                    onYes: () => MainGameManager.Instance.BackToMainMenu(),
                    onNo: () => { }
                );
            });
        }

    }
}
