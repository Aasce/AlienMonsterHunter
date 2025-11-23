using Asce.Game.Effects;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainGame.UIs.HUDs
{
    public class UIEffect : UIObject
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _durationFilter;
        [SerializeField] private TextMeshProUGUI _stackText;

        [Header("Runtime")]
        [SerializeField, Readonly] private Effect _effect;

        public Effect Effect
        {
            get => _effect;
            set
            {
                if (_effect == value) return;
                this.Unregister();
                _effect = value;
                this.Register();
            }
        }


        private void Register() 
        {
            if (Effect == null) return;
            if (Effect.Information == null) return;

            _icon.sprite = Effect.Information.Icon;
            this.SetDuration(Effect.Duration.Ratio);

            _stackText.gameObject.SetActive(Effect.Information.ApplyType == EffectApplyType.Stacking);
            this.SetStack(Effect.Stack);

            Effect.Duration.OnBaseTimeChanged += Duration_OnBaseTimeChanged;
            Effect.Duration.OnCurrentTimeChanged += Duration_OnCurrentTimeChanged;
            Effect.OnStacked += Effect_OnStacked;
        }

        private void Unregister()
        {
            if (Effect == null) return;
            Effect.Duration.OnBaseTimeChanged -= Duration_OnBaseTimeChanged;
            Effect.Duration.OnCurrentTimeChanged -= Duration_OnCurrentTimeChanged;
            Effect.OnStacked -= Effect_OnStacked;
        }

        private void Duration_OnCurrentTimeChanged(object sender, float newValue)
        {
            this.SetDuration(Effect.Duration.Ratio);
        }

        private void Duration_OnBaseTimeChanged(object sender, float newValue)
        {
            this.SetDuration(Effect.Duration.Ratio);
        }

        private void Effect_OnStacked()
        {
            this.SetStack(Effect.Stack);
        }


        private void SetDuration(float cooldownRatio)
        {
            _durationFilter.fillAmount = cooldownRatio;
        }

        private void SetStack(int stack)
        {
            if (Effect.Information.ApplyType == EffectApplyType.Stacking && stack > 1)
            {
                _stackText.gameObject.SetActive(true);
                _stackText.text = stack.ToString();
            }
            else
            {
                _stackText.gameObject.SetActive(false);
            }
        }

    }
}
