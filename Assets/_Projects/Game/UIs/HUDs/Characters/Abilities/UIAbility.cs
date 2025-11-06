using Asce.Game.Abilities;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    public class UIAbility : UIObject
    {
        [SerializeField, Readonly] private AbilityContainer _container;

        [Header("References")]
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _nothingContent;

        [Space]
        [SerializeField] private Image _icon;
        [SerializeField] private Image _cooldownFiler;
        [SerializeField] private TextMeshProUGUI _cooldownText;
        [SerializeField] private TextMeshProUGUI _callKeyText;


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
                this.ShowContent(false);
                return;
            }
            this.ShowContent(true);
            _icon.sprite = _container.AbilityPrefab.Information.Icon;
            float ratio = _container.Cooldown.Ratio;
            this.SetCooldown(ratio);

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
