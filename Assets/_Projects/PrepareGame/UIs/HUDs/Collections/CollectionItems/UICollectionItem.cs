using Asce.Managers.UIs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Asce.PrepareGame.UIs.Collections
{
    public abstract class UICollectionItem<T> : UIObject, IPointerClickHandler
    {
        [SerializeField] private UICollectionView<T> _collection;
        [SerializeField] private T _item;

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

        public void Set(T item)
        {
            Item = item;
            this.InternalSet(item);
        }
        protected abstract void InternalSet(T item);

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Collection != null)
            {
                Collection.ItemClick(this);
            }
        }

    }
}
