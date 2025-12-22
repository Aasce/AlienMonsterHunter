using Asce.Core.UIs;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game
{
    public class UIToggle : UIComponent
    {
        [SerializeField] private Button _button;
        [SerializeField] private RectTransform _content;

        [Space]
        [SerializeField] private bool _showOnStart = true;
        private UIComponent _cache;

        public Button Button => _button;
        public RectTransform Content => _content;

        private void Start()
        {
            _button.onClick.AddListener(Button_OnClicked);

            if (_content.TryGetComponent(out UIComponent ui))
            {
                _cache = ui;
                _cache.SetVisible(_showOnStart);
            }
            else _content.gameObject.SetActive(_showOnStart);
        }

        private void Button_OnClicked()
        {
            if (_cache != null) _cache.Toggle(); 
            else _content.gameObject.SetActive(!_content.gameObject.activeSelf);
        }

    }
}
