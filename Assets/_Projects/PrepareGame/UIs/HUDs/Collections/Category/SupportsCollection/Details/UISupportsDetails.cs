using Asce.Game.Players;
using Asce.Game.Supports;
using Asce.Game.UIs.Panels;
using Asce.PrepareGame.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UISupportsDetails : UICollectionDetails<Support>
    {
        [Header("Information")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _icon;

        [Header("Locked")]
        [SerializeField] private RectTransform _lockedContent;
        [SerializeField] private Button _unlockButton;

        [Space]
        [SerializeField] private RectTransform _unlockedContent;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private Button _upgradeButton;

        [Header("Description")]
        [SerializeField] private TextMeshProUGUI _callCDText;
        [SerializeField] private TextMeshProUGUI _recallCDText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        protected SupportProgress Progress => PlayerManager.Instance.Progress.SupportsProgress.Get(Item.Information.Name);

        private void Start()
        {
            _unlockButton.onClick.AddListener(UnlockButton_OnClick);
            _upgradeButton.onClick.AddListener(UpgradeButton_OnClick);
        }

        public override void Set(Support support)
        {
            if (Item == support) return;
            this.Unregister();
            Item = support;
            this.Register();
        }

        private void Register()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

            _nameText.text = Item.Information.Name;
            _icon.sprite = Item.Information.Icon;

            _callCDText.text = $"Call CD: {Item.Information.Cooldown:#.#}s";
            _recallCDText.text = $"Recall CD: {Item.Information.CooldownOnRecall:#.#}s";
            _descriptionText.text = Item.Information.Description;

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
            SupportProgress progress = Progress;
            bool isUnlocked = progress != null && progress.IsUnlocked;
            _unlockedContent.gameObject.SetActive(isUnlocked);
            _lockedContent.gameObject.SetActive(!isUnlocked);

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
            UIUnlockSupportPanel unlockPanel = PrepareGameManager.Instance.UIController.PanelController.GetPanelByName("Unlock Support") as UIUnlockSupportPanel;
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
