using Asce.Game.UIs.Panels;
using Asce.Managers;
using Asce.Managers.Utils;
using Asce.Menu.UIs.HUDs;
using UnityEngine;

namespace Asce.Menu.UIs
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
