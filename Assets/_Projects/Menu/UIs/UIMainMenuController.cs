using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Menu.UIs
{
    public class UIMainMenuController : MonoBehaviourSingleton<UIMainMenuController>
    {
        [SerializeField] private UIMainMenuHUDController _hud;
        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _hud);
            if (_hud == null) Debug.LogError($"[{typeof(UIMainMenuController).ToString().ColorWrap(Color.red)}]] UIMainMenuHUDController is not assigned", this);
        }
    }
}
