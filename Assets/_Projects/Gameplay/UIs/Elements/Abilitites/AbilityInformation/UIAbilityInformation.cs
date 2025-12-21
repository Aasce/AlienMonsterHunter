using Asce.Game.Abilities;
using Asce.Core.UIs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Asce.Core.Attributes;

namespace Asce.Game.UIs.Elements
{
    public class UIAbilityInformation : UIComponent, IPointerClickHandler, IUIHighlightable
    {
        [SerializeField] private Image _icon;

        [Space]
        [SerializeField] private Image _background;
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _nothingContent;

        [Header("Reference")]
        [SerializeField, Readonly] private Ability _ability;
        [SerializeField, Readonly] private UIAbilities _controller;

        [Header("Highlight")]
        [SerializeField] private Color _highlightColor = Color.cyan;
        [SerializeField] private Color _normalColor = Color.white;

        public UIAbilities Controller
        {
            get => _controller;
            set => _controller = value;
        }

        public void Set(Ability ability)
        {
            if (ability == _ability) return;
            _ability = ability;
            if (_ability == null)
            {
                this.ShowContent(false);
                return;
            }

            this.ShowContent(true);
            _icon.sprite = _ability.Information.Icon; 
        }

        public void SetHighlight(bool isHighlight)
        {
            if (isHighlight)  _background.color = _highlightColor;
            else _background.color = _normalColor;
        }

        private void ShowContent(bool isShow)
        {
            _content.gameObject.SetActive(isShow);
            _nothingContent.gameObject.SetActive(!isShow);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Controller == null) return;
            Controller.ShowDetails(_ability);
            this.Highlight();
        }

    }
}
