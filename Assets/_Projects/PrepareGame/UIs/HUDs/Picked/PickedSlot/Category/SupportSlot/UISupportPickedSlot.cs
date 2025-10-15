using Asce.Game.Supports;
using Asce.Managers.Attributes;
using Asce.PrepareGame.Picks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UISupportPickedSlot : UIPickedSlot<Support>
    {
        [Space]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [Space]
        [SerializeField, Readonly] private int _slotIndex = -1;

        public int SlotIndex
        {
            get => _slotIndex;
            set => _slotIndex = value;
        }

        protected override void InternalSet(Support item)
        {
            PickController.Instance.PickSupport(_slotIndex, item);
            if (item == null || item.Information == null)
            {
                this.ShowContent(false);
                return;
            }

            this.ShowContent(true);
            if (_icon != null) _icon.sprite = item.Information.Icon;
            if (_nameText != null) _nameText.text = item.Information.Name;
            if (_levelText != null) _levelText.text = $"lv.NaN";
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }
    }
}
