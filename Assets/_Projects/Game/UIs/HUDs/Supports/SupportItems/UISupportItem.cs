using Asce.Game.Supports;
using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Core.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainGame.UIs
{
    public class UISupportItem : UIComponent
    {
        [SerializeField, Readonly] private SupportContainer _container;

        [Header("References")]
        [SerializeField, Readonly] private UISupportsInformation _supportsInformation;
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _nothingContent;

        [Space]
        [SerializeField] private Image _background;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _cooldownFiler;
        [SerializeField] private TextMeshProUGUI _cooldownText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _callKeyText;

        [Header("Config")]
        [SerializeField] private Color _validInstanceColor = Color.yellow;

        public UISupportsInformation SupportsInformation
        {
            get => _supportsInformation;
            set
            {
                if (_supportsInformation == value) return;
                _supportsInformation = value;
            }
        }

        public SupportContainer Container
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

        private void Update()
        {
            if (Container == null || !Container.IsValid) return;
            if (Container.CurrentSupportIsValid)
            {
                _background.color = _validInstanceColor;
                return;
            }

            _background.color = Color.white;
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
            _levelText.text = $"{_container.Level}";


            _container.OnLevelChanged += Support_LevelChanged;
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

        private void Support_LevelChanged(int newLevel)
        {
            _levelText.text = $"{_container.Level}";
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
