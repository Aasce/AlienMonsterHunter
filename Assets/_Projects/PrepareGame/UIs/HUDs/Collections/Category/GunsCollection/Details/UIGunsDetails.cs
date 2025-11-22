using Asce.Game.Guns;
using Asce.Game.Players;
using Asce.Game.UIs.Panels;
using Asce.PrepareGame.Manager;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UIGunsDetails : UICollectionDetails<Gun>
    {
        [Header("Information")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _icon;

        [Header("Purchase")]
        [SerializeField] private RectTransform _buyContent;
        [SerializeField] private Button _buyButton;

        [Space]
        [SerializeField] private RectTransform _purchasedContent;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private Button _upgradeButton;

        [Header("Stats")]
        [SerializeField] private UIMagazineGroup _magazineGroup;
        [SerializeField] private UIGunMode _gunMode;

        protected GunProgress Progress => PlayerManager.Instance.Progress.GunsProgress.Get(Item.Information.Name);

        private void Start()
        {
            _buyButton.onClick.AddListener(BuyButton_OnClick);
            _upgradeButton.onClick.AddListener(UpgradeButton_OnClick);
        }

        public override void Set(Gun gun)
        {
            if (Item == gun) return;
            this.Unregister();
            Item = gun;
            this.Register();
        }

        private void Register()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

            _nameText.text = Item.Information.Name;
            _icon.sprite = Item.Information.Icon;
            _magazineGroup.Set(Item.Information);
            _gunMode.Set(null);

            this.SetPurchasedState();
            Progress.OnLevelChanged += Progress_OnLevelChanged;
        }

        private void Unregister()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

            Progress.OnLevelChanged -= Progress_OnLevelChanged;
        }

        private void SetPurchasedState()
        {
            GunProgress progress = Progress;
            bool isPurchased = progress != null && progress.IsUnlocked;
            _purchasedContent.gameObject.SetActive(isPurchased);
            _buyContent.gameObject.SetActive(!isPurchased);

            if (isPurchased)
            {
                _level.text = $"lv. {progress.Level}";
            }
            else
            {

            }
        }

        private void BuyButton_OnClick()
        {
            UIUnlockGunPanel unlockPanel = PrepareGameManager.Instance.UIController.PanelController.GetPanelByName("Unlock Gun") as UIUnlockGunPanel;
            if (unlockPanel == null) return;

            unlockPanel.Icon.sprite = Item.Information.Icon;
            unlockPanel.NameText.text = Item.Information.Name;
            unlockPanel.ItemProgress = Progress;
            unlockPanel.OnUnlock += (condition) =>
            {
                Progress.Unlock(condition);
                this.SetPurchasedState();
            };
            unlockPanel.Show();
        }

        private void UpgradeButton_OnClick()
        {
            Progress.Level = Progress.Level + 1;
        }

        private void Progress_OnLevelChanged(int newLevel)
        {
            _level.text = $"lv. {newLevel}";
        }
    }
}
