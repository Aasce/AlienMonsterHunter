using Asce.Game.Guns;
using Asce.Game.Players;
using Asce.Game.UIs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UIGunCollectionItem : UICollectionItem<Gun>
    {
        [Header("Gun")]
        [SerializeField] private UITintColor _tintColor;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        public GunProgress Progress => PlayerManager.Instance.Progress.GunsProgress.Get(Item.Information.Name);

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
                this.IsShowContent(false);
                return;
            }

            this.IsShowContent(true);
            _icon.sprite = Item.Information.Icon;
            _nameText.text = Item.Information.Name;
            this.SetPurchasedState();

            Progress.OnUnlocked += SetPurchasedState;
            Progress.OnLevelChanged += Progress_OnLevelChanged;
        }

        protected override void Unregister()
        {
            base.Unregister();
            if (Item == null || Item.Information == null) return;

            Progress.OnUnlocked -= SetPurchasedState;
            Progress.OnLevelChanged -= Progress_OnLevelChanged;
        }

        protected virtual void SetPurchasedState()
        {
            GunProgress progress = Progress;
            bool isPurchased = progress != null && progress.IsUnlocked;
            if (isPurchased)
            {
                _tintColor.TintColor = Color.white;
                _levelText.gameObject.SetActive(true);
                _levelText.text = $"lv. {progress.Level}";
            }
            else
            {
                _tintColor.TintColor = Color.gray;
                _levelText.gameObject.SetActive(false);
            }
        }

        private void Progress_OnLevelChanged(int newLevel)
        {
            _levelText.text = $"lv. {newLevel}";
        }

    }
}
