using Asce.Game.UIs.Panels;
using Asce.PrepareGame.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs.Panels
{
    public class UIPrepareGameSettingsPanel : UISettingsPanel
    {
        [Header("Prepare Game")]
        [SerializeField] private Button _backMenuButton;

        public override void Initialize()
        {
            base.Initialize();
            _backMenuButton.onClick.AddListener(() =>
            {
                UIConfirmPanel confirmPanel = PrepareGameManager.Instance.UIController.PanelController.GetPanelByName("Confirm") as UIConfirmPanel;
                if (confirmPanel == null) return;
                confirmPanel.Show(
                    title: "Back to Menu?",
                    description: "Are you sure you want to back to menu?",
                    onYes: () =>
                    {
                        this.Hide();
                        PrepareGameManager.Instance.BackToMainMenu();
                    },
                    onNo: () => { }
                );
            });
        }
    }
}
