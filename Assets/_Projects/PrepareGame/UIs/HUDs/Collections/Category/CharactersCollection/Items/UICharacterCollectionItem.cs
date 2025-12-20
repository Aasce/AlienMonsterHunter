using Asce.Game.Entities.Characters;
using Asce.Game.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UICharacterCollectionItem : UICollectionItem<Character>
    {
        [Header("Character")]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI _roleValueText;
        [SerializeField] private Slider _difficultySlider;

        public CharacterProgress Progress => PlayerManager.Instance.Progress.CharactersProgress.Get(Item.Information.Name);
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
            _icon.sprite = Item.Information.Icon;
            _nameText.text = Item.Information.Name;
            _roleValueText.text = Item.Information.Role.ToString();
            _difficultySlider.value = Item.Information.Difficulty;
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
