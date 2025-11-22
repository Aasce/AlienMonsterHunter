using Asce.Game.Players;
using Asce.Game.Progress;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.Panels
{
    public abstract class UIUnlockPanel : UIWindowPanel
    {
        [Header("Unlock Panel")]
        [SerializeField] protected Image _icon;
        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected ToggleGroup _conditionsToggleGroup;

        [SerializeField] protected Pool<UIUnlockCondition> _unlockConditionPool = new();

        [Space]
        [SerializeField] protected Button _unlockButton;
        [SerializeField] protected Button _cancelButton;

        [Header("Runtime")]
        [SerializeField, Readonly] protected ItemProgress _itemProgress;

        public event Action<SO_UnlockCondition> OnUnlock;

        public Image Icon => _icon;
        public TextMeshProUGUI NameText => _nameText;

        public ItemProgress ItemProgress
        {
            get => _itemProgress;
            set
            {
                if (_itemProgress == value) return;
                this.Unregister();
                _itemProgress = value;
                this.Register();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            _unlockButton.onClick.AddListener(UnlockButton_OnClick);
            _cancelButton.onClick.AddListener(CloseButton_OnClick);

            _conditionsToggleGroup.SetAllTogglesOff();
            _unlockButton.interactable = false;
        }

        public override void Hide()
        {
            if (!this.IsShow) return;
            base.Hide();
            OnUnlock = null;
        }

        protected virtual void OnDestroy()
        {
            this.Unregister();
            OnUnlock = null;
        }

        protected virtual void Register()
        {
            if (_itemProgress == null) return;

            foreach (SO_UnlockCondition unlockCondition in _itemProgress.Information.UnlockConditions)
            {
                UIUnlockCondition uiUnlockCondition = _unlockConditionPool.Activate(out bool isCreated);
                if (isCreated)
                {
                    uiUnlockCondition.Checkbox.group = _conditionsToggleGroup;
                    uiUnlockCondition.Checkbox.onValueChanged.AddListener(_ => RefreshUnlockButton());
                }
                uiUnlockCondition.UnlockCondition = unlockCondition;
                uiUnlockCondition.transform.SetAsLastSibling();
                uiUnlockCondition.Show();
            }
            _conditionsToggleGroup.SetAllTogglesOff();
        }

        protected virtual void Unregister()
        {
            if (_itemProgress == null) return;
            _unlockConditionPool.Clear(isDeactive: true, (ui) => ui.Hide()); 
            _unlockButton.interactable = false;
        }

        protected void RefreshUnlockButton()
        {
            bool anyOn = _conditionsToggleGroup.AnyTogglesOn();
            _unlockButton.interactable = anyOn;
        }


        protected virtual void UnlockButton_OnClick()
        {
            UIUnlockCondition uiUnlockCondition = _unlockConditionPool.Activities.Find((ui) => ui.Checkbox.isOn);
            if (uiUnlockCondition == null) return;
            if (uiUnlockCondition.UnlockCondition == null) return;
            OnUnlock?.Invoke(uiUnlockCondition.UnlockCondition);
            this.Hide();
        }
    }
}
