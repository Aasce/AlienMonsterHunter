using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Asce.Menu.UIs
{
    public abstract class UICollectionItem<T> : UIObject, IPointerClickHandler where T : MonoBehaviour
    {
        [SerializeField, Readonly] protected T _item;

        [Header("References")]
        [SerializeField] protected RectTransform _content;
        [SerializeField] protected RectTransform _nothingContent;

        public T Item => _item;


        protected virtual void Start() { }


        public void Set(T Item)
        {
            _item = Item;
            InternalSet(Item);
        }

        protected void IsShowContent(bool isShow)
        {
            _content.gameObject.SetActive(isShow);
            _nothingContent.gameObject.SetActive(!isShow);
        }

        public abstract void InternalSet(T Item);

        public virtual void OnPointerClick(PointerEventData eventData)
        {

        }
    }
}
