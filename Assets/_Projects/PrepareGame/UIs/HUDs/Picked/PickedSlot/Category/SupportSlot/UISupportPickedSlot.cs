using Asce.Game.Players;
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

        public SupportProgress Progress => PlayerManager.Instance.Progress.SupportsProgress.Get(Item.Information.Name);

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

        protected void OnDestroy()
        {
            if (PlayerManager.Instance == null) return;
            this.Unregister();
        }

        protected override void Register()
        {
            base.Register();
            if (Item == null || Item.Information == null)
            {
                this.ShowContent(false);
                return;
            }

            this.ShowContent(true);
            _icon.sprite = Item.Information.Icon;
            _nameText.text = Item.Information.Name;
            _levelText.text = $"lv. {Progress.Level}";

            Progress.OnLevelChanged += Progress_OnLevelChanged;
        }

        protected override void Unregister()
        {
            base.Unregister();
            if (Item == null || Item.Information == null) return;

            Progress.OnLevelChanged -= Progress_OnLevelChanged;
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

        private void Progress_OnLevelChanged(int newLevel)
        {
            _levelText.text = $"lv. {newLevel}";
        }

    }
}
