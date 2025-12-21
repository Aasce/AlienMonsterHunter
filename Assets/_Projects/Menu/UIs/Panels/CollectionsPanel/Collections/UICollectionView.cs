using Asce.Game.UIs;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainMenu.UIs.Panels
{
    public abstract class UICollectionView<T> : UITabView where T : MonoBehaviour
    {
        [SerializeField] protected UICollectionDetails<T> _details;

        [Space]
        [SerializeField] protected Pool<UICollectionItem<T>> _pool = new();

        public UICollectionDetails<T> Details => _details;
        public abstract IEnumerable<T> GetCollection();

        public override void Show()
        {
            base.Show();
            this.Refresh();
            Details.Hide();
        }

        protected virtual void Refresh()
        {
            _pool.Clear(onClear: (item) => item.Hide());
            foreach (T item in GetCollection())
            {
                UICollectionItem<T> uiItem = _pool.Activate(out bool isCreated); 
                if (uiItem == null) continue;

                if (isCreated) uiItem.Collection = this;
                uiItem.Set(item);
                uiItem.RectTransform.SetAsLastSibling();
                uiItem.Show();
            }
        }

        public virtual void ShowDetails(T item)
        {
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
