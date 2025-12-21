using Asce.Game.Players;
using Asce.Game.Supports;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels.Collections
{
    public class UISupportCollectionItem : UICollectionItem<Support>
    {
        [Space]
        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected Image _icon;

        [Space]
        [SerializeField] protected TextMeshProUGUI _levelText;

        public SupportProgress Progress => PlayerManager.Instance.Progress.SupportsProgress.Get(Item.Information.Name);
        public override bool IsUnlocked => Progress != null && Progress.IsUnlocked;

        protected override void Register()
        {
            base.Register();
            if (Item == null || Item.Information == null)
            {
                this.IsShowContent(false);
                return;
            }

            this.IsShowContent(true);
            _nameText.text = Item.Information.Name;
            _icon.sprite = Item.Information.Icon;
            this.SetLockedState();

            Progress.OnUnlocked += SetLockedState;
            Progress.OnLevelChanged += Progress_OnLevelChanged;
        }

        protected override void Unregister()
        {
            base.Unregister();
            if (Item == null || Item.Information == null) return;

            Progress.OnUnlocked -= SetLockedState;
            Progress.OnLevelChanged -= Progress_OnLevelChanged;
        }

        protected override void SetLockState()
        {
            base.SetLockState();
        }

        protected override void SetUnlockState()
        {
            base.SetUnlockState();
            _levelText.text = $"lv. {Progress.Level}";
        }

        private void Progress_OnLevelChanged(int newLevel)
        {
            _levelText.text = $"lv. {newLevel}";
        }
    }
}
