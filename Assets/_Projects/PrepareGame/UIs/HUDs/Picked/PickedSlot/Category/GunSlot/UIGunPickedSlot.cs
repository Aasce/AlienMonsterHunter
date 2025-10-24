using Asce.Game.Guns;
using Asce.PrepareGame.Picks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UIGunPickedSlot : UIPickedSlot<Gun>
    {
        [Space]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        public override void Initialize()
        {
            base.Initialize();

            this.Set(PickController.Instance.GunPrefab);
            PickController.Instance.OnPickGun += PickController_OnPickGun;
        }

        protected override void InternalSet(Gun item)
        {
            if (item == null || item.Information == null)
            {
                this.ShowContent(false);
                return;
            }

            this.ShowContent(true);
            if (_icon != null) _icon.sprite = item.Information.Icon;
            if (_nameText != null) _nameText.text = item.Information.Name;
            if (_levelText != null) _levelText.text = $"lv.NaN";
        }

        protected override void DiscardButton_OnClick()
        {
            base.DiscardButton_OnClick();
            PickController.Instance.PickGun(null);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

        private void PickController_OnPickGun(Gun gun)
        {
            this.Set(gun);
        }
    }
}
