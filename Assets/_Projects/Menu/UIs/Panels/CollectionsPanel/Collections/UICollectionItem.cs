using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Game.Players;
using Asce.Game.UIs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public abstract class UICollectionItem<T> : UIComponent, IPointerClickHandler, IUIHighlightable where T : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected Image _background;
        [SerializeField] protected RectTransform _content;
        [SerializeField] protected RectTransform _nothingContent;
        [SerializeField] protected UITintColor _tintColor;

        [Header("Lock")]
        [SerializeField] protected RectTransform _unlockContent;
        [SerializeField] protected RectTransform _lockContent;

        [Header("Hightlight")]
        [SerializeField] protected Color _highlightColor = Color.yellow;
        [SerializeField] protected Color _normalColor = Color.white;
        [SerializeField] protected Color _lockColor = Color.gray;

        // Runtime
        [SerializeField, Readonly] private UICollectionView<T> _collection;
        [SerializeField, Readonly] protected T _item;


        public UICollectionView<T> Collection
        {
            get => _collection;
            set => _collection = value;
        }

        public T Item
        {
            get => _item;
            protected set => _item = value;
        }

        public abstract bool IsUnlocked { get; }


        protected virtual void Start() { }

        protected void OnDestroy()
        {
            if (PlayerManager.Instance == null) return;
            this.Unregister();
        }

        public void ResetStatus()
        {
            this.SetHighlight(false);
        }

        public void Set(T item)
        {
            this.Unregister();
            Item = item;
            this.Register();
        }

        public void SetHighlight(bool isHighlight)
        {
            if (isHighlight)
            {
                if (IsUnlocked) _background.color = _highlightColor;
                else _background.color = Color.Lerp(_highlightColor, _lockColor, 0.5f);
            }
            else
            {
                if (IsUnlocked) _background.color = _normalColor;
                else _background.color = _lockColor;
            }
        }


        protected void IsShowContent(bool isShow)
        {
            _content.gameObject.SetActive(isShow);
            _nothingContent.gameObject.SetActive(!isShow);
        }

        protected virtual void SetLockedState()
        {
            if (IsUnlocked) this.SetUnlockState();
            else this.SetLockState();
        }

        protected virtual void Register()
        {

        }

        protected virtual void Unregister()
        {

        }

        protected virtual void SetLockState()
        {
            _tintColor.TintColor = _lockColor;
            _unlockContent.gameObject.SetActive(false);
            _lockContent.gameObject.SetActive(true);
        }

        protected virtual void SetUnlockState()
        {
            _tintColor.TintColor = _normalColor;
            _unlockContent.gameObject.SetActive(true);
            _lockContent.gameObject.SetActive(false);
        }


        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Collection.ItemClick(this);
            this.Highlight();
        }
    }
}
