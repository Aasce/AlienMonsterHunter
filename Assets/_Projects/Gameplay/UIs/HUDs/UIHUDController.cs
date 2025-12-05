using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.UIs.HUDs
{
    [RequireComponent(typeof(Canvas))]
    public abstract class UIHUDController : UIComponent, ICanvasController
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