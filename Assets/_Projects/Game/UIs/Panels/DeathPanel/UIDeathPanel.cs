using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.Panels
{
    public class UIDeathPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI _deathReasonText;
        [SerializeField] private Button _reviveButton; 
        
        public event Action OnReviveClicked;

        protected override void Reset()
        {
            base.Reset();
            _name = "Death";
        }

        public override void Initialize()
        {
            base.Initialize();
            if (_reviveButton != null) _reviveButton.onClick.AddListener(ReviveButton_OnClick);
        }

        private void ReviveButton_OnClick()
        {
            OnReviveClicked?.Invoke();
            this.Hide();
        }
    }
}