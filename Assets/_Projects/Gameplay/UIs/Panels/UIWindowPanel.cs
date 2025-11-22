using Asce.Managers.Utils;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asce.Game.UIs.Panels
{
    public abstract class UIWindowPanel : UIPanel
    {
        [SerializeField] protected UIClickBlock _clickBlock;
        [SerializeField] protected Button _closeButton;
        [SerializeField] protected TextMeshProUGUI _titleText;

        public UIClickBlock ClickBlock => _clickBlock;
        public Button CloseButton => _closeButton;
        public TextMeshProUGUI TitleText => _titleText;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _clickBlock);
        }

        protected virtual void Start()
        {
            if (_closeButton != null) _closeButton.onClick.AddListener(CloseButton_OnClick);
            if (_clickBlock != null) _clickBlock.OnClick += ClickBlock_OnClick;
        }

        protected virtual void CloseButton_OnClick()
        {
            if (IsShow) this.Hide();
        }

        protected virtual void ClickBlock_OnClick(PointerEventData data)
        {
            if (IsShow) this.Hide();
        }
    }
}