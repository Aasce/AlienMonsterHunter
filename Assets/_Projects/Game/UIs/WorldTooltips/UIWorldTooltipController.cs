using Asce.Game.UIs;
using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.MainGame.UIs.ToolTips
{
    public class UIWorldTooltipController : UIComponent, ICanvasController
    {
        [SerializeField, Readonly] private Canvas _canvas;
        [SerializeField] UIInteractionTip _interactionTip;

        public Canvas Canvas => _canvas;
        public UIInteractionTip InteractionTip => _interactionTip;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _canvas);
        }

        public void Initialize()
        {
            InteractionTip.Initialize();
        }
    }
}
