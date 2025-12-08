using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Game.Maps;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UIChoiceMapItem : UIComponent, IPointerClickHandler
    {
        [Header("References")]
        [SerializeField] protected RectTransform _content;
        [SerializeField] protected RectTransform _nothingContent;

        [Space]
        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected Image _thumbnail;

        [Header("Runtime")]
        [SerializeField, Readonly] protected UIChoiceLevelPanel _controller;
        [SerializeField, Readonly] protected Map _item;

        public UIChoiceLevelPanel Panel
        {
            get => _controller;
            set => _controller = value;
        }
        public Map Item => _item;

        public void Set(Map item)
        {
            this.Unregister();
            _item = item;
            this.Register();
        }

        private void Register()
        {
            if (_item == null || _item.Information == null)
            {
                this.IsShowContent(false);
                return;
            }

            this.IsShowContent(true);
            _nameText.text = _item.Information.Name;
            _thumbnail.sprite = _item.Information.Thumbnail;
        }

        private void Unregister()
        {

        }

        protected void IsShowContent(bool isShow)
        {
            _content.gameObject.SetActive(isShow);
            _nothingContent.gameObject.SetActive(!isShow);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Panel.ChoiceMap(Item);
        }

    }
}
