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

        public override void Initialize()
        {
            base.Initialize();

            if (SlotIndex < PickController.Instance.SupportPrefabs.Count)
            {
                this.Set(PickController.Instance.SupportPrefabs[SlotIndex]);
            }
            PickController.Instance.OnPickSupport += PickController_OnPickSupport;
        }

        protected override void InternalSet(Support item)
        {
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

        protected override void DiscardButton_OnClick()
        {
            base.DiscardButton_OnClick();
            PickController.Instance.PickSupport(SlotIndex, null);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

        private void PickController_OnPickSupport(int index, Support support)
        {
            if (SlotIndex != index) return;
            this.Set(support);
        }

    }
}
