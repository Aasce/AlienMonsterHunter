using Asce.Game.Guns;
using Asce.Game.Players;
using Asce.Game.UIs.Elements;
using Asce.Game.UIs.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UIGunDetails : UICollectionDetails<Gun>
    {
        [Header("Information")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _icon;

        [Header("Purchase")]
        [SerializeField] private RectTransform _lockContent;
        [SerializeField] private Button _unlockButton;

        [Space]
        [SerializeField] private RectTransform _unlockedContent;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private Button _upgradeButton;

        [Header("Stats")]
        [SerializeField] private UIMagazineGroup _magazineGroup;
        [SerializeField] private UIGunMode _gunMode;

        protected GunProgress Progress => PlayerManager.Instance.Progress.GunsProgress.Get(Item.Information.Name);

        private void Start()
        {
            _unlockButton.onClick.AddListener(UnlockButton_OnClick);
            _upgradeButton.onClick.AddListener(UpgradeButton_OnClick);
        }
		
		private void OnDestroy()
		{
			if (PlayerManager.Instance == null) return;
			this.Unregister();
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

            this.SetLockedState();
            Progress.OnLevelChanged += Progress_OnLevelChanged;
        }

        private void Unregister()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

            Progress.OnLevelChanged -= Progress_OnLevelChanged;
        }

        private void SetLockedState()
        {
            GunProgress progress = Progress;
            bool isUnlocked = progress != null && progress.IsUnlocked;
            _unlockedContent.gameObject.SetActive(isUnlocked);
            _lockContent.gameObject.SetActive(!isUnlocked);

            if (isUnlocked)
            {
                _level.text = $"lv. {progress.Level}";
            }
            else
            {

            }
        }

        private void UnlockButton_OnClick()
        {
            UIUnlockGunPanel unlockPanel = MainMenuManager.Instance.UIController.PanelController.GetPanelByName("Unlock Gun") as UIUnlockGunPanel;
            if (unlockPanel == null) return;

            unlockPanel.Icon.sprite = Item.Information.Icon;
            unlockPanel.NameText.text = Item.Information.Name;
            unlockPanel.ItemProgress = Progress;
            unlockPanel.OnUnlock += (condition) =>
            {
                Progress.Unlock(condition);
                this.SetLockedState();
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
