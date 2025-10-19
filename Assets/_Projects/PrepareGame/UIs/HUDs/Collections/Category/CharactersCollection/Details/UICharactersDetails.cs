using Asce.Game.Entities.Characters;
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
            bool isJoined = false;
            _joinedContent.gameObject.SetActive(isJoined);
            _inviteContent.gameObject.SetActive(!isJoined);

            if (isJoined)
            {
                _levelProgess.Set(Item);
            }
            else
            {

            }
        }
    }
}
