using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.UIs.Panels
{
    public class UIPanelController : UIObject
    {
        [SerializeField, Readonly] private List<UIPanel> _panels = new();
        private ReadOnlyCollection<UIPanel> _readonlyPanels;
        private Dictionary<System.Type, UIPanel> _panelDictionary;

        public ReadOnlyCollection<UIPanel> Panels => _readonlyPanels ??= _panels.AsReadOnly();


        protected override void RefReset()
        {
            base.RefReset();

            _panels.Clear();
            UIPanel[] panels = this.GetComponentsInChildren<UIPanel>(true);
            if (panels == null || panels.Length == 0) return;
            foreach (UIPanel panel in panels)
            {
                if (panel == null) continue;
                _panels.Add(panel);
            }
        }


        public T GetPanel<T>() where T : UIPanel
        {
            if (_panelDictionary == null) this.InitDictionary();
            if (_panelDictionary.TryGetValue(typeof(T), out UIPanel panel))
            {
                return panel as T;
            }
            return null;
        }

        public void HideAll()
        {
            foreach (UIPanel panel in _panels)
            {
                if (panel != null) panel.Hide();
            }
        }

        private void InitDictionary()
        {
            _panelDictionary = new Dictionary<System.Type, UIPanel>();
            foreach (UIPanel panel in _panels)
            {
                System.Type type = panel.GetType();
                if (_panelDictionary.ContainsKey(type)) continue;
                _panelDictionary.Add(type, panel);
            }
        }
    }
}