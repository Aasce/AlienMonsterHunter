using Asce.Core.UIs;
using System;
using UnityEngine.EventSystems;

namespace Asce.Game.UIs
{
    public class UIClickBlock : UIComponent, IPointerClickHandler
    {
        public event Action<PointerEventData> OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(eventData);
        }
    }
}
