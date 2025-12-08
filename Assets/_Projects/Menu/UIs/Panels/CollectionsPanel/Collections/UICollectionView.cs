using Asce.Game.UIs;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainMenu.UIs
{
    public abstract class UICollectionView<T> : UITabView where T : MonoBehaviour
    {
        [SerializeField] protected Pool<UICollectionItem<T>> _pool = new();

        protected abstract IEnumerable<T> Items { get; }

        public override void Show()
        {
            base.Show();
            this.Refresh();
        }

        protected virtual void Refresh()
        {
            _pool.Clear(onClear: (item) => item.Hide());
            foreach (T item in Items)
            {
                UICollectionItem<T> uiItem = _pool.Activate();
                uiItem.Set(item);

                uiItem.RectTransform.SetAsLastSibling();
                uiItem.Show();
            }
        }
    }
}
