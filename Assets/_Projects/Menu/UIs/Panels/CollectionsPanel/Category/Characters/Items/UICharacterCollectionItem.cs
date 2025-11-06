using Asce.Game.Entities.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Menu.UIs.Characters
{
    public class UICharacterCollectionItem : UICollectionItem<Character>
    {
        [Space]
        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected Image _icon;

        [Space]
        [SerializeField] protected TextMeshProUGUI _roleText;
        [SerializeField] protected Slider _difficultSlider;

        [Header("Invite or Join")]
        [SerializeField] protected RectTransform _joinedContent;
        // [SerializeField] protected UILevelProgess _levelProgess;
        
        [Space]
        [SerializeField] protected RectTransform _inviteContent;
        [SerializeField] protected TextMeshProUGUI _inviteCostText;
        [SerializeField] protected Button _inviteButton;

        protected bool IsJoined => false;

        protected override void Start()
        {
            base.Start();
            _inviteButton.onClick.AddListener(InviteButton_OnClick);
        }

        public override void InternalSet(Character item)
        {
            if (Item == null || Item.Information == null)
            {
                this.IsShowContent(false);
                return;
            }

            this.IsShowContent(true);
            _nameText.text = Item.Information.Name;
            _icon.sprite = Item.Information.Icon;
            // _roleText.text = Item.Information.Role;
            // _difficultSlider.value = Item.Information.Difficulty;
            this.ShowJoinContent();
        }


        protected void ShowJoinContent()
        {
            bool isJoined = this.IsJoined;
            _joinedContent.gameObject.SetActive(isJoined);
            _inviteContent.gameObject.SetActive(!isJoined);

            if (isJoined)
            {

            }
            else
            {
                float cost = 100f; // Item.Information.InviteCost;
                _inviteCostText.text = $"${cost}";
            }
        }

        private void InviteButton_OnClick()
        {
            if (IsJoined) return;
        }

    }
}
