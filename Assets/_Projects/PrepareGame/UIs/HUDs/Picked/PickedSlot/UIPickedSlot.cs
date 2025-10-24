using Asce.Managers.UIs;
using Asce.PrepareGame.UIs.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public abstract class UIPickedSlot<T> : UIObject, IPointerClickHandler where T : UnityEngine.Object
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


        public virtual void Initialize()
        {
            if (DiscardButton != null) DiscardButton.onClick.AddListener(DiscardButton_OnClick);
        }

        public virtual void Set(T item)
        {
            Item = item;
            this.InternalSet(item);
        }

        protected abstract void InternalSet(T item);

        protected virtual void DiscardButton_OnClick() { }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            UIPrepareGameController.Instance.HUDController.Tabs.ShowTabByTabView(_collection.RectTransform);
            if (Item != null) _collection.ShowDetails(Item);
        }

        protected virtual void ShowContent(bool isShow)
        {
            if (_content != null) _content.gameObject.SetActive(isShow);
            if (_discardButton != null) _discardButton.gameObject.SetActive(isShow);
            if (_nothingAlert != null) _nothingAlert.gameObject.SetActive(!isShow);
        }
    }
}
