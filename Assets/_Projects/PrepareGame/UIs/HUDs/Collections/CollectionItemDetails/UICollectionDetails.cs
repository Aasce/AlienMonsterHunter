using Asce.Core.UIs;
using UnityEngine;

namespace Asce.PrepareGame.UIs
{
    public abstract class UICollectionDetails<T> : UIComponent
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
