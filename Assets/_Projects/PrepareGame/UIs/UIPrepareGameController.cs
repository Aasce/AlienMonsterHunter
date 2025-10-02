using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.PrepareGame.UIs
{
    public class UIPrepareGameController : MonoBehaviourSingleton<UIPrepareGameController>
    {

        [SerializeField] private UIPrepareGameHUDController _hud;


        public UIPrepareGameHUDController HUD => _hud;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _hud);
            if (_hud == null) Debug.LogError($"[{typeof(UIPrepareGameController).ToString().ColorWrap(Color.red)}]] UIPrepareGameHUDController is not assigned", this);
        }
    }
}
