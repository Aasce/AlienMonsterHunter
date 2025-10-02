using Asce.Managers.UIs;
using UnityEngine;

namespace Asce.PrepareGame.UIs
{
    public abstract class UICollectionDetails<T> : UIObject
    {
        private T _item;

        public T Item
        {
            get => _item;
            protected set => _item = value;
        }

        public abstract void Set(T item);
    }
}
