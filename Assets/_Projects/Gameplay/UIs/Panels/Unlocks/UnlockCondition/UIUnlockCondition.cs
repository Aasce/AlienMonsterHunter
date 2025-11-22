using Asce.Game.Progress;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.Panels
{
    public class UIUnlockCondition : UIObject
    {
        [SerializeField] private UITintColor _tintColor;
        [SerializeField] private TextMeshProUGUI _conditionText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Toggle _checkbox;

        [Header("Runtime")]
        [SerializeField, Readonly] private SO_UnlockCondition _unlockCondition;

        public TextMeshProUGUI ConditionText => _conditionText;
        public TextMeshProUGUI DescriptionText => _descriptionText;
        public Toggle Checkbox => _checkbox;

        public SO_UnlockCondition UnlockCondition
        {
            get => _unlockCondition;
            set
            {
                if (_unlockCondition == value) return;
                this.Unregister();
                _unlockCondition = value;
                this.Register();
            }
        }


        public virtual void Initialize()
        {
            Checkbox.onValueChanged.AddListener(Checkbox_OnValueChanged);
        }

        private void OnDestroy()
        {
            this.Unregister();
        }


        private void Register()
        {
            if (_unlockCondition == null) return;
            _conditionText.text = _unlockCondition.Name;
            _descriptionText.text = _unlockCondition.Description; 
            
            bool isMet = _unlockCondition.IsMet();
            _checkbox.interactable = isMet;
            _checkbox.isOn = false;
            if (isMet) _tintColor.TintColor = Color.white;
            else _tintColor.TintColor = Color.gray;
        }

        private void Unregister()
        {
            if (_unlockCondition == null) return;


        }



        private void Checkbox_OnValueChanged(bool newValue)
        {

        }
    }
}
