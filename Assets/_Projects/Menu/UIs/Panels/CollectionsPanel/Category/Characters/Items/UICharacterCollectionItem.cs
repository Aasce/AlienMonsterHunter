using Asce.Game.Entities.Characters;
using Asce.Game.Players;
using Asce.Game.UIs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Characters
{
    public class UICharacterCollectionItem : UICollectionItem<Character>
    {
        [Space]
        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected Image _icon;

        [Space]
        [SerializeField] protected TextMeshProUGUI _roleText;
        [SerializeField] protected Slider _difficultySlider;

        [Space]
        [SerializeField] protected UILevelProgess _levelProgess;

        public CharacterProgress Progress => PlayerManager.Instance.Progress.CharactersProgress.Get(Item.Information.Name);
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
            _roleText.text = Item.Information.Role.ToString();
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
        }

        protected override void SetUnlockState()
        {
            base.SetUnlockState();
            _levelProgess.Set(Progress);
        }

        private void Progress_OnLevelChanged(int newLevel)
        {

        }

    }
}
