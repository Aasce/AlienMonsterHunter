using Asce.Game.UIs.Panels;
using Asce.Core;
using Asce.Core.Utils;
using Asce.MainMenu.UIs.HUDs;
using UnityEngine;

namespace Asce.MainMenu.UIs
{
    public class UIMainMenuController : GameComponent, IHasPanelController
    {
        [SerializeField] private UIMainMenuHUDController _hudController;
        [SerializeField] private UIPanelController _panelController;

        public UIMainMenuHUDController HUDController => _hudController;
        public UIPanelController PanelController => _panelController;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _hudController);
            if (_hudController == null) Debug.LogError($"[{typeof(UIMainMenuController).ToString().ColorWrap(Color.red)}]] UIMainMenuHUDController is not assigned", this);

            this.LoadComponent(out _panelController);
            if (_panelController == null) Debug.LogError($"[{typeof(UIMainMenuController).ToString().ColorWrap(Color.red)}]] UIPanelController is not assigned", this);
        }

        public void Initialize()
        {
            HUDController.Initialize();
            PanelController.Initialize();
        }
    }
}
