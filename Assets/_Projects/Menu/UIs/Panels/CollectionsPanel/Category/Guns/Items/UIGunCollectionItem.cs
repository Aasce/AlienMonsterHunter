using Asce.Game.Guns;
using Asce.Game.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Guns
{
    public class UIGunCollectionItem : UICollectionItem<Gun>
    {
        [Space]
        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected Image _icon;

        [Space]
        [SerializeField] protected TextMeshProUGUI _levelText;

        public GunProgress Progress => PlayerManager.Instance.Progress.GunsProgress.Get(Item.Information.Name);
        public override bool IsUnlocked => Progress != null && Progress.IsUnlocked;


        protected override void Start()
        {
            base.Start();
        }


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
            _levelText.gameObject.SetActive(false);
        }

        protected override void SetUnlockState()
        {
            base.SetUnlockState();
            _levelText.gameObject.SetActive(true);
            _levelText.text = $"lv. {Progress.Level}";
        }

        private void Progress_OnLevelChanged(int newLevel)
        {
            _levelText.text = $"lv. {newLevel}";
        }
    }
}
