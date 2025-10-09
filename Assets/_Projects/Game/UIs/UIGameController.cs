using Asce.Game.UIs.HUDs;
using Asce.Game.UIs.Panels;
using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.UIs
{
    public class UIGameController : MonoBehaviourSingleton<UIGameController>
    {
        [SerializeField] private UIGameHUDController _hud;
        [SerializeField] private UIPanelController _panel;


        public UIGameHUDController HUDController => _hud;
        public UIPanelController PanelController => _panel;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _hud);
            if (_hud == null) Debug.LogError($"[{typeof(UIGameController).ToString().ColorWrap(Color.red)}]] UIHUDController is not assigned", this);

            this.LoadComponent(out _panel);
            if (_panel == null) Debug.LogError($"[{typeof(UIGameController).ToString().ColorWrap(Color.red)}]] UIPanelController is not assigned", this);
        }
    }
}
