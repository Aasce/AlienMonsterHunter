using Asce.Game.UIs.Panels;
using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.PrepareGame.UIs
{
    public class UIPrepareGameController : MonoBehaviourSingleton<UIPrepareGameController>, IHasPanelController
    {
        [SerializeField] private UIPrepareGameHUDController _hudController;
        [SerializeField] private UIPanelController _panelController;

        public UIPrepareGameHUDController HUDController => _hudController;
        public UIPanelController PanelController => _panelController;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _hudController);
            if (_hudController == null) Debug.LogError($"[{typeof(UIPrepareGameController).ToString().ColorWrap(Color.red)}]] UIPrepareGameHUDController is not assigned", this);

            this.LoadComponent(out _panelController);
            if (_panelController == null) Debug.LogError($"[{typeof(UIPrepareGameController).ToString().ColorWrap(Color.red)}]] UIPanelController is not assigned", this);
        }
    }
}
