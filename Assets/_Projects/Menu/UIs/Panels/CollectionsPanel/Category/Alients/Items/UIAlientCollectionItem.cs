using Asce.Game.Entities.Enemies;
using Asce.PrepareGame.UIs;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Menu.UIs.Alients
{
    public class UIAlientCollectionItem : UICollectionItem<Enemy>
    {
        [Space]
        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected Image _icon;

        public override void InternalSet(Enemy item)
        {
            if (Item == null || Item.Information == null)
            {
                this.IsShowContent(false);
                return;
            }

            this.IsShowContent(true);
            _nameText.text = Item.Information.Name;
            _icon.sprite = Item.Information.Icon;
        }
		
    }
}
