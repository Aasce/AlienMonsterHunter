using Asce.Game.UIs;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.PrepareGame.UIs.Collections
{
    public abstract class UICollectionView<T> : UITabView
    {
        [SerializeField] protected UICollectionDetails<T> _details;

        [Space]
        [SerializeField] protected Pool<UICollectionItem<T>> _pool = new();

        public UICollectionDetails<T> Details => _details;
        public abstract IEnumerable<T> GetCollection();

        public override void Show()
        {
            base.Show();
            this.ResetCollection();
            if (Details != null) Details.Hide();
        }

        public virtual void ResetCollection()
        {
            _pool.Clear(onClear: (item) => item.Hide());
            IEnumerable<T> collection = GetCollection();
            foreach (T item in collection)
            {
                if (item == null) continue;
                UICollectionItem<T> uiItem = _pool.Activate(out bool isCreated);
                if (item == null) continue;

                if (isCreated) uiItem.Collection = this;
                uiItem.Set(item);
                uiItem.RectTransform.SetAsLastSibling();
                uiItem.Show();
            }
        }

        public virtual void ShowDetails(T item)
        {
            if (Details == null) return;
            Details.Set(item);
            Details.Show();
        }

        public virtual void ItemClick(UICollectionItem<T> uiItem)
        {
            if (uiItem == null) return;
            this.ShowDetails(uiItem.Item);
        }
    }
}
