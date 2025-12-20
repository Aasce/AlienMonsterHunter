using Asce.Core.UIs;
using Asce.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    public class UIHighlightButton : UIComponent, IUIHighlightable
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _background;

        [Header("Highlight")]
        [SerializeField] protected Color _highlightColor = Color.yellow;
        [SerializeField] protected Color _normalColor = Color.white;
        [SerializeField] protected Color _lockColor = Color.gray;

        public Button Button => _button;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _button);
        }

        private void Start()
        {
            _button.onClick.AddListener(Button_OnClick);
        }

        public void SetHighlight(bool isHighlight)
        {
            if (isHighlight)
            {
                if (_button.interactable) _background.color = _highlightColor;
                else _background.color = Color.Lerp(_highlightColor, _lockColor, 0.5f);
            }
            else
            {
                if (_button.interactable) _background.color = _normalColor;
                else _background.color = _lockColor;
            }
        }

        private void Button_OnClick()
        {
            this.Highlight();
        }
    }
}