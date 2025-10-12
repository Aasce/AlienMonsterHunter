using Asce.Game.Stats;
using Asce.Managers.UIs;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    public class UIResourceStatBar : UIObject
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _text;
        private ResourceStat _resourceStat;

        public Slider Slider => _slider;
        public TextMeshProUGUI Text => _text;

        public ResourceStat ResourceStat
        {
            get => _resourceStat;
            set
            {
                if (_resourceStat == value) return;
                this.Unregister();
                _resourceStat = value;
                this.Register();
            }
        }

        private void Register()
        {
            if (ResourceStat == null)
            {
                if (Slider != null) Slider.gameObject.SetActive(false);
                if (Text != null) Text.gameObject.SetActive(false);
                return;
            }
            this.SetSlider();
            this.SetText();

            ResourceStat.OnFinalValueChanged += ResourceStat_OnFinalValueChanged;
            ResourceStat.OnCurrentValueChanged += ResourceStat_OnCurrentValueChanged;
        }

        private void Unregister()
        {
            if (ResourceStat == null) return;
            ResourceStat.OnFinalValueChanged -= ResourceStat_OnFinalValueChanged;
            ResourceStat.OnCurrentValueChanged -= ResourceStat_OnCurrentValueChanged;
        }

        private void ResourceStat_OnFinalValueChanged(float oldValue, float newValue)
        {
            Slider.maxValue = newValue;
            this.SetText();
        }

        private void ResourceStat_OnCurrentValueChanged(float oldValue, float newValue)
        {
            Slider.value = newValue;
            this.SetText();
        }

        private void SetSlider()
        {
            if (Slider == null) return;
            Slider.gameObject.SetActive(true);
            Slider.maxValue = ResourceStat.FinalValue;
            Slider.value = ResourceStat.CurrentValue;
        } 

        private void SetText()
        {
            if (Text == null) return;
            Text.gameObject.SetActive(true);
            Text.text = $"{ResourceStat.CurrentValue:0}/{ResourceStat.FinalValue:0}";
        }

    }
}
