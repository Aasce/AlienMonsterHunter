using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.UIs.HUDs
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UIHUDController : UIObject, ICanvasController
    {
        [SerializeField, Readonly] protected Canvas _canvas;

        public Canvas Canvas => _canvas;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _canvas);
        }

        public virtual void Initialize() { }
    }
}