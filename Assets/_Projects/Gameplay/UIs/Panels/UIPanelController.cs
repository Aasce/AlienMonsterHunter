using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using Asce.Managers;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.UIs.Panels
{
    public class UIPanelController : UIObject, ICanvasController
    {
        [SerializeField, Readonly] private Canvas _canvas;
        [SerializeField, Readonly] private ListObjects<string, UIPanel> _panels = new((panel) =>
        {
            if (panel == null) return null;
            return panel.Name;
        });

        [Space]
        [SerializeField] private bool _isHideOnInitialze = true;

        public Canvas Canvas => _canvas;

        public ReadOnlyCollection<UIPanel> Panels => _panels.List;
        public UIPanel GetPanelByName(string name) => _panels.Get(name);


        protected override void RefReset()
        {
            base.RefReset();

            this.LoadComponent(out _canvas);
            this.LoadPanels();
        }

        public virtual void Initialize()
        {
            foreach (UIPanel panel in Panels)
            {
                if (panel == null) continue;
                panel.Initialize();
            }

            if (_isHideOnInitialze) this.HideAll();
        }

        public T GetPanel<T>() where T : UIPanel
        {
            foreach (UIPanel panel in Panels)
            {
                if (panel == null) continue;
                if (panel.GetType() == typeof(T)) return panel as T;
            }

            return null;
        }

        public void HideAll()
        {
            foreach (UIPanel panel in Panels)
            {
                if (panel == null) continue; 
                panel.Hide();
            }
        }

        private void LoadPanels()
        {
            UIPanel[] panels = this.GetComponentsInChildren<UIPanel>(true);
            if (panels == null || panels.Length == 0) return;
            _panels.Load(panels);
        }
    }
}