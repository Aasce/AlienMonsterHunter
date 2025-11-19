using Asce.Game.Players;
using Asce.Game.Supports;
using Asce.Game.UIs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UISupportCollectionItem : UICollectionItem<Support>
    {
        [Header("Support")]
        [SerializeField] private UITintColor _tintColor;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        public SupportProgress Progress => PlayerManager.Instance.Progress.SupportsProgress.Get(Item.Information.Name);

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

        protected virtual void SetLockedState()
        {
            SupportProgress progress = Progress;
            bool isUnlocked = progress != null && progress.IsUnlocked;
            if (isUnlocked)
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
