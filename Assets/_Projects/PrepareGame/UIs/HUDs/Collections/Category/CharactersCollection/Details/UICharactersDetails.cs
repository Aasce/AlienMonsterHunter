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
        [SerializeField] private UILevelProgess _levelProgess;

        [Header("Stats")]
        [SerializeField] private UIStatsGroup _statGroup;


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

            if (_icon != null) _icon.sprite = Item.Information.Icon;
            if (_nameText != null) _nameText.text = Item.Information.Name;
            if (_levelProgess != null) _levelProgess.Set(Item);
            if (_statGroup != null) _statGroup.Set(Item.Information.Stats);
        }

        private void Unregister()
        {
            if (Item == null) return;
            if (Item.Information == null) return;



        }
    }
}
