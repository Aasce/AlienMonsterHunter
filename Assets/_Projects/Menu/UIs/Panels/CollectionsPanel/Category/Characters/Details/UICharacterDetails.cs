using Asce.Game.Entities.Characters;
using Asce.Game.Players;
using Asce.Game.UIs.Elements;
using Asce.Game.UIs.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UICharacterDetails : UICollectionDetails<Character>
    {
        [Header("Information")]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _roleText;
        [SerializeField] private Slider _difficultySlider;

        [Header("Locked")]
        [SerializeField] private RectTransform _lockContent;
        [SerializeField] private Button _unlockButton;

        [SerializeField] private RectTransform _unlockContent;
        [SerializeField] private UILevelProgess _levelProgess;

        [Header("Stats")]
        [SerializeField] private UIStatsGroup _statGroup;

        [Header("Abilitites")]
        [SerializeField] private UIAbilities _abilities;

        public CharacterProgress Progress => PlayerManager.Instance.Progress.CharactersProgress.Get(Item.Information.Name);


        private void Start()
        {
            _unlockButton.onClick.AddListener(UnlockButton_OnClick);
        }

        public override void Set(Character character)
        {
            if (Item == character) return;
            this.Unregister();
            Item = character;
            this.Register();
        }


        private void Register()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

            this.SetLockedState();
            _icon.sprite = Item.Information.Icon;
            _nameText.text = Item.Information.Name;
            _roleText.text = Item.Information.Role.ToString();
            _difficultySlider.value = Item.Information.Difficulty;
            _statGroup.Set(Item.Information);
            _abilities.Set(Item.Information.AbilitiesName);
        }

        private void Unregister()
        {
            if (Item == null) return;
            if (Item.Information == null) return;



        }

        private void SetLockedState()
        {
            CharacterProgress progress = Progress;
            bool isUnlocked = progress != null && progress.IsUnlocked;
            _unlockContent.gameObject.SetActive(isUnlocked);
            _lockContent.gameObject.SetActive(!isUnlocked);

            if (isUnlocked)
            {
                _levelProgess.Set(progress);
            }
            else
            {

            }
        }

        private void UnlockButton_OnClick()
        {
            UIUnlockCharacterPanel unlockPanel = MainMenuManager.Instance.UIController.PanelController.GetPanelByName("Unlock Character") as UIUnlockCharacterPanel;
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

    }
}
