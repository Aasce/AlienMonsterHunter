using Asce.Core.UIs;
using UnityEngine;

namespace Asce.Game.UIs
{
    public interface IUIHighlightable : IUIComponent
    {
        public void SetHighlight(bool isHighlight);
    }
}
