using Asce.Game.UIs.HUDs;
using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.UIs
{
    public class UIController : MonoBehaviourSingleton<UIController>
    {
        [SerializeField] private UIHUDController _hud;


        public UIHUDController HUD => _hud;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _hud);
            if (_hud == null) Debug.LogError($"[{typeof(UIController).ToString().ColorWrap(Color.red)}]] UIHUDController is not assigned", this);
        }
    }
}
