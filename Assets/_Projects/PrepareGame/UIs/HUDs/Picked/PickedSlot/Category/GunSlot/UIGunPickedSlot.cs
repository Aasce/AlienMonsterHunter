using Asce.Game.Guns;
using Asce.Game.Players;
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

        public GunProgress Progress => PlayerManager.Instance.Progress.GunsProgress.Get(Item.Information.Name);

        public override void Initialize()
        {
            base.Initialize();

            this.Set(PickController.Instance.GunPrefab);
            PickController.Instance.OnPickGun += PickController_OnPickGun;
        }

        private void OnDestroy()
        {
            if (PlayerManager.Instance == null) return;
            this.Unregister();
        }

        protected override void Register()
        {
            if (Item == null || Item.Information == null)
            {
                this.ShowContent(false);
                return;
            }

            this.ShowContent(true);
            _icon.sprite = Item.Information.Icon;
            _nameText.text = Item.Information.Name;
            _levelText.text = $"lv. {Progress.Level}";

            Progress.OnLevelChanged += Progress_OnLevelChanged;
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
        private void Progress_OnLevelChanged(int newLevel)
        {
            _levelText.text = $"lv. {newLevel}";
        }

    }
}
