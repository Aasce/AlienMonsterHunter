using Asce.Game.UIs.Panels;
using Asce.Core.UIs;
using Asce.Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    [RequireComponent(typeof(Button))]
    public class UIOpenPanelButton : UIComponent
    {
        [SerializeField] private Button _button;
        [SerializeField] private UIPanelController _panelController;
        [SerializeField] private string _panelName = string.Empty;


        public string PanelName
        {
            get => _panelName;
            set => _panelName = value;
        }


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _button);
            _panelController = GameObject.FindFirstObjectByType<UIPanelController>();
        }

        private void Start()
        {
            _button.onClick.AddListener(Button_OnClick);
        }

        private void Button_OnClick()
        {
            if (_panelController == null) return;
            UIPanel panel = _panelController.GetPanelByName(_panelName);
            if (panel == null) return;
            panel.Toggle();
        }
    }
}
