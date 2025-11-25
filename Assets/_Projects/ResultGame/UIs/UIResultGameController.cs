using Asce.Game.UIs.Panels;
using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.ResultGame
{
    public class UIResultGameController : GameComponent, IHasPanelController
    {
        [SerializeField] private UIResultGameHUDController _hudController;
        [SerializeField] private UIPanelController _panelController;

        public UIResultGameHUDController HUDController => _hudController;
        public UIPanelController PanelController => _panelController;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _hudController);
            if (_hudController == null) Debug.LogError($"[{typeof(UIResultGameController).ToString().ColorWrap(Color.red)}]] UIMainMenuHUDController is not assigned", this);

            this.LoadComponent(out _panelController);
            if (_panelController == null) Debug.LogError($"[{typeof(UIResultGameController).ToString().ColorWrap(Color.red)}]] UIPanelController is not assigned", this);
        }

        public void Initialize()
        {
            HUDController.Initialize();
            PanelController.Initialize();
        }
    }
}