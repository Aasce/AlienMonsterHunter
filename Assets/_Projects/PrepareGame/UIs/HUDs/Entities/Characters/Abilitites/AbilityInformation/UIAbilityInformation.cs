using Asce.Game.Abilities;
using Asce.Core.UIs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UIAbilityInformation : UIComponent, IPointerClickHandler
    {
        [SerializeField] private Image _icon;

        [Space]
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _nothingContent;

        [Header("Reference")]
        [SerializeField] private Ability _ability;
        [SerializeField] private UIAbilities _controller;

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
            if (_icon != null) _icon.sprite = _ability.Information.Icon; 
        }


        private void ShowContent(bool isShow)
        {
            if (_content != null) _content.gameObject.SetActive(isShow);
            if (_nothingContent != null) _nothingContent.gameObject.SetActive(!isShow);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Controller == null) return;
            Controller.ShowDetails(_ability);
        }
    }
}
