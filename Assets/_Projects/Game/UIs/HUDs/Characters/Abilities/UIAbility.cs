using Asce.Game.Abilities;
using Asce.Managers.UIs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    public class UIAbility : UIObject
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _cooldownFiler;
        [SerializeField] private TextMeshProUGUI _cooldownText;

        [Space]
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _nothingContent;

        private AbilityContainer _container;

        public AbilityContainer Container
        {
            get => _container;
            set
            {
                if (_container == value) return;
                this.Unregister();
                _container = value;
                this.Register();
            }
        }


        private void Register()
        {
            if (_container == null || !_container.IsValid)
            {
                this.ShowContent(false);
                return;
            }
            this.ShowContent(true);
            if (_icon != null) _icon.sprite = _container.AbilityPrefab.Information.Icon;
            _container.Cooldown.OnBaseTimeChanged += AbilityCooldown_OnBaseTimeChanged;
            _container.Cooldown.OnCurrentTimeChanged += AbilityCooldown_OnCurrentTimeChanged;
        }

        private void Unregister()
        {
            if (_container == null)
            {
                return;
            }
            _container.Cooldown.OnBaseTimeChanged -= AbilityCooldown_OnBaseTimeChanged;
            _container.Cooldown.OnCurrentTimeChanged -= AbilityCooldown_OnCurrentTimeChanged;
        }

        private void ShowContent(bool isShow)
        {
            if (_content != null) _content.gameObject.SetActive(isShow);
            if (_nothingContent != null) _nothingContent.gameObject.SetActive(!isShow);
        }

        private void SetCooldown(float cooldownRatio)
        {
            if (_cooldownFiler != null)
            {
                _cooldownFiler.fillAmount = cooldownRatio;
            }
            if (_cooldownText != null)
            {
                if (cooldownRatio <= 0f) _cooldownText.gameObject.SetActive(false);
                else
                {
                    _cooldownText.gameObject.SetActive(true);
                    _cooldownText.text = _container.Cooldown.CurrentTime.ToString("#");
                }
            }
        }

        private void AbilityCooldown_OnBaseTimeChanged(object sender, float newValue)
        {
            float ratio = _container.Cooldown.Ratio;
            this.SetCooldown(ratio);
        }

        private void AbilityCooldown_OnCurrentTimeChanged(object sender, float newValue)
        {
            float ratio = _container.Cooldown.Ratio;
            this.SetCooldown(ratio);
        }
    }
}
