using Asce.Game.Entities.Characters;
using log4net.Core;
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
            if (_icon != null) _icon.sprite = Item.Information.Icon;
            if (_nameText != null) _nameText.text = Item.Information.Name;
            if (_statGroup != null) _statGroup.Set(Item.Information.Stats);
            if (_abilities != null) _abilities.Set(Item.Information.AbilitiesName);
        }

        private void Unregister()
        {
            if (Item == null) return;
            if (Item.Information == null) return;



        }

        private void SetInviteContent()
        {
            bool isJoined = false;
            if (isJoined)
            {
                if (_inviteContent != null) _inviteContent.gameObject.SetActive(false);
                if (_joinedContent != null)
                {
                    _joinedContent.gameObject.SetActive(true);
                    if (_levelProgess != null) _levelProgess.Set(Item);
                }
            }
            else
            {
                if (_joinedContent != null) _joinedContent.gameObject.SetActive(false);
                if (_inviteContent != null)
                {
                    _inviteContent.gameObject.SetActive(true);
                }
            }
        }
    }
}
