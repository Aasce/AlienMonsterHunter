using Asce.Game.Supports;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UISupportsDetails : UICollectionDetails<Support>
    {
        [Header("Information")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _icon;

        [Header("Description")]
        [SerializeField] private TextMeshProUGUI _callCDText;
        [SerializeField] private TextMeshProUGUI _recallCDText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _upgradeButton;


        private void Start()
        {
            if (_upgradeButton != null) _upgradeButton.onClick.AddListener(UpgradeButton_OnClick);
        }

        public override void Set(Support support)
        {
            if (Item == support) return;
            this.Unregister();
            Item = support;
            this.Register();
        }


        private void Register()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

            if (_nameText != null) _nameText.text = Item.Information.Name;
            if (_icon != null) _icon.sprite = Item.Information.Icon;

            if (_callCDText != null) _callCDText.text = $"Call CD: {Item.Information.Cooldown:#.#}s";
            if (_recallCDText != null) _recallCDText.text = $"Recall CD: {Item.Information.CooldownOnRecall:#.#}s";
            if (_descriptionText != null) _descriptionText.text = Item.Information.Description;
        }

        private void Unregister()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

        }

        private void UpgradeButton_OnClick()
        {


        }

    }
}
