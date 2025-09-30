using Asce.Game.UIs.HUDs;
using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.UIs
{
    public class UIGameController : MonoBehaviourSingleton<UIGameController>
    {
        [SerializeField] private UIGameHUDController _hud;


        public UIGameHUDController HUD => _hud;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _hud);
            if (_hud == null) Debug.LogError($"[{typeof(UIGameController).ToString().ColorWrap(Color.red)}]] UIHUDController is not assigned", this);
        }
    }
}
