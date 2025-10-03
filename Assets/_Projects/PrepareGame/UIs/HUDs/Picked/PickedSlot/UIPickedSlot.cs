using Asce.Managers.UIs;
using Asce.PrepareGame.UIs.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public abstract class UIPickedSlot<T> : UIObject, IPointerClickHandler
    {
        [SerializeField] protected T _item;

        [Space]
        [SerializeField] protected Button _discardButton;
        [SerializeField] protected RectTransform _content;
        [SerializeField] protected RectTransform _nothingAlert;

        [Space]
        [SerializeField] protected UICollectionView<T> _collection;

        public T Item
        {
            get => _item;
            protected set => _item = value;
        }
        public Button DiscardButton => _discardButton;


        protected virtual void Start()
        {
            if (DiscardButton != null) DiscardButton.onClick.AddListener(DiscardButton_OnClick);
        }

        public virtual void Set(T item)
        {
            this.InternalSet(item);
            Item = item;
        }

        protected abstract void InternalSet(T item);

        protected virtual void DiscardButton_OnClick()
        {
            this.Set(default);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            UIPrepareGameController.Instance.HUD.Tabs.ShowTabByTabView(_collection.RectTransform);
        }

        protected virtual void ShowContent(bool isShow)
        {
            if (_content != null) _content.gameObject.SetActive(isShow);
            if (_discardButton != null) _discardButton.gameObject.SetActive(isShow);
            if (_nothingAlert != null) _nothingAlert.gameObject.SetActive(!isShow);
        }
    }
}
