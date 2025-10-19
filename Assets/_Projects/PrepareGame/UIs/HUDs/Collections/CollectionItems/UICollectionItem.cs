using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Asce.PrepareGame.UIs.Collections
{
    public abstract class UICollectionItem<T> : UIObject, IPointerClickHandler
    {
        [SerializeField, Readonly] private UICollectionView<T> _collection;
        [SerializeField, Readonly] private T _item;

        [Header("References")]
        [SerializeField] protected RectTransform _content;
        [SerializeField] protected RectTransform _nothingContent;


        public UICollectionView<T> Collection
        {
            get => _collection;
            set => _collection = value;
        }

        public T Item
        {
            get => _item;
            set => _item = value;
        }

        protected virtual void Start()
        {

        }

        public void Set(T item)
        {
            Item = item;
            this.InternalSet(item);
        }
        protected abstract void InternalSet(T item);

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (Collection != null)
            {
                Collection.ItemClick(this);
            }
        }

        protected virtual void IsShowContent(bool isShow)
        {
            _content.gameObject.SetActive(isShow);
            _nothingContent.gameObject.SetActive(!isShow);
        }
    }
}
