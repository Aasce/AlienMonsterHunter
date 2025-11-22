using Asce.Game.Entities.Characters;
using Asce.Game.Players;
using Asce.Game.UIs.Panels;
using Asce.PrepareGame.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UICharactersDetails : UICollectionDetails<Character>
    {
        [Header("Information")]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _roleText;
        [SerializeField] private Slider _difficultySlider;

        [Header("Joined")]
        [SerializeField] private RectTransform _inviteContent;
        [SerializeField] private Button _inviteButton;

        [SerializeField] private RectTransform _joinedContent;
        [SerializeField] private UILevelProgess _levelProgess;

        [Header("Stats")]
        [SerializeField] private UIStatsGroup _statGroup;

        [Header("Abilitites")]
        [SerializeField] private UIAbilities _abilities;

        public CharacterProgress Progress => PlayerManager.Instance.Progress.CharactersProgress.Get(Item.Information.Name);

        private void Start()
        {
            _inviteButton.onClick.AddListener(InviteButton_OnClick);
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

            this.SetInviteContent();
            _icon.sprite = Item.Information.Icon;
            _nameText.text = Item.Information.Name;
            _roleText.text = Item.Information.Role.ToString();
            _difficultySlider.value = Item.Information.Difficulty;
            _statGroup.Set(Item.Information.Stats);
            _abilities.Set(Item.Information.AbilitiesName);
        }

        private void Unregister()
        {
            if (Item == null) return;
            if (Item.Information == null) return;



        }

        private void SetInviteContent()
        {
            CharacterProgress progress = PlayerManager.Instance.Progress.CharactersProgress.Get(Item.Information.Name);
            bool isUnlocked = progress != null && progress.IsUnlocked;
            _joinedContent.gameObject.SetActive(isUnlocked);
            _inviteContent.gameObject.SetActive(!isUnlocked);

            if (isUnlocked)
            {
                _levelProgess.Set(progress);
            }
            else
            {

            }
        }

        private void InviteButton_OnClick()
        {
            UIUnlockCharacterPanel unlockPanel = PrepareGameManager.Instance.UIController.PanelController.GetPanelByName("Unlock Character") as UIUnlockCharacterPanel;
            if (unlockPanel == null) return;

            unlockPanel.Icon.sprite = Item.Information.Icon;
            unlockPanel.NameText.text = Item.Information.Name;
            unlockPanel.ItemProgress = Progress;
            unlockPanel.OnUnlock += (condition) =>
            {
                Progress.Unlock(condition);
                this.SetInviteContent();
            };
            unlockPanel.Show();
        }

    }
}
