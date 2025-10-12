using Asce.Game.UIs.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs.Panels
{
    public class UIPrepareGameSettingsPanel : UISettingsPanel
    {
        [Header("Prepare Game")]
        [SerializeField] private Button _backMenuButton;


        protected override void Start()
        {
            base.Start();
            if (_backMenuButton != null) _backMenuButton.onClick.AddListener(() =>
            {
                UIConfirmPanel confirmPanel = UIPrepareGameController.Instance.PanelController.GetPanelByName("Confirm") as UIConfirmPanel;
                if (confirmPanel == null) return;
                confirmPanel.Show(
                    title: "Back to Menu?",
                    description: "Are you sure you want to back to menu?",
                    onYes: () => PrepareGameManager.Instance.BackToMainMenu(),
                    onNo: () => { }
                );
            });
        }
    }
}
