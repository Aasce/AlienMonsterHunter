using Asce.Core.Attributes;
using Asce.Core.UIs;
using UnityEngine;

namespace Asce.Game.UIs
{
    public class UIHighlightGroup : UIComponent
    {
        [SerializeField] private Transform _content;
        [SerializeField, Readonly] private IUIHighlightable _currentHighlight;

        public event System.Action<IUIHighlightable> OnHighlightChanged; 

        public void Set(IUIHighlightable highlight)
        {
            if (_content == null) _content = this.transform;
            _currentHighlight = highlight;

            foreach (Transform item in _content)
            {
                if (!item.TryGetComponent(out IUIHighlightable uiHighlight)) continue;
                uiHighlight.SetHighlight(uiHighlight == _currentHighlight);
            }

            OnHighlightChanged?.Invoke(_currentHighlight);
        }
    }
}
