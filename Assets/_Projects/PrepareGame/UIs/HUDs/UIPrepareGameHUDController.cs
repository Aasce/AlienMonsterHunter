using Asce.Managers.UIs;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.PrepareGame.UIs
{
    [RequireComponent(typeof(Canvas))]
    public class UIPrepareGameHUDController : UIObject
    {
        [SerializeField] private Canvas _canvas;


        public Canvas Canvas => _canvas;


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _canvas);
        }
    }
}
