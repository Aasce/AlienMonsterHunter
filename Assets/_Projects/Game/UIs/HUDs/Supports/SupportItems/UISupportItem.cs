using Asce.Game.Supports;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    public class UISupportItem : UIObject
    {
        [SerializeField, Readonly] private UISupportsInformation _supportsInformation;
        [SerializeField, Readonly] private SupportContainer _container;

        [Header("References")]
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _nothingContent;

        [Space]
        [SerializeField] private Image _icon;
        [SerializeField] private Image _cooldownFiler;
        [SerializeField] private TextMeshProUGUI _cooldownText;
        [SerializeField] private TextMeshProUGUI _callKeyText;

        public UISupportsInformation SupportsInformation
        {
            get => _supportsInformation;
            set
            {
                if (_supportsInformation == value) return;
                _supportsInformation = value;
            }
        }

        public void Set(SupportContainer container)
        {
            if (_container == container) return;
            this.Unregister();
            _container = container;
            this.Register();
        }

        public void SetKey(KeyCode key)
        {
            if (key == KeyCode.None) _callKeyText.gameObject.SetActive(false);
            else
            {
                _callKeyText.gameObject.SetActive(true);
                _callKeyText.text = key.ToReadableString();
            }
        }

        private void Register()
        {
            if (_container == null || !_container.IsValid)
            {
                this.IsShowContent(false);
                return;
            }

            this.IsShowContent(true);
            _icon.sprite = _container.Information.Icon;
            _container.Cooldown.OnCurrentTimeChanged += Cooldown_OnCurrentTimeChanged;
            _container.Cooldown.OnBaseTimeChanged += Cooldown_OnBaseTimeChanged;
        }

        private void Unregister()
        {
            if (_container == null || !_container.IsValid) return;

            _container.Cooldown.OnCurrentTimeChanged -= Cooldown_OnCurrentTimeChanged;
            _container.Cooldown.OnBaseTimeChanged -= Cooldown_OnBaseTimeChanged;
        }

        private void IsShowContent(bool isShow)
        {
            _content.gameObject.SetActive(isShow);
            _nothingContent.gameObject.SetActive(!isShow);
        }

        private void SetCooldown(float cooldownRatio)
        {
            _cooldownFiler.fillAmount = cooldownRatio;
            if (cooldownRatio <= 0f) _cooldownText.gameObject.SetActive(false);
            else
            {
                _cooldownText.gameObject.SetActive(true);
                _cooldownText.text = _container.Cooldown.CurrentTime.ToString("#");
            }
        }

        private void Cooldown_OnBaseTimeChanged(object sender, float newValue)
        {
            float ratio = _container.Cooldown.Ratio;
            this.SetCooldown(ratio);
        }

        private void Cooldown_OnCurrentTimeChanged(object sender, float newValue)
        {
            float ratio = _container.Cooldown.Ratio;
            this.SetCooldown(ratio);
        }

    }
}
