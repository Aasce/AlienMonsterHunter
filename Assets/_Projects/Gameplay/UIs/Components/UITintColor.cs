using Asce.Managers.UIs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    public class UITintColor : UIObject
    {
        [SerializeField] private Color _tintColor = Color.white;

        private readonly List<Graphic> _graphics = new();
        private readonly Dictionary<Graphic, Color> _originalColors = new();

        public Color TintColor
        {
            get => _tintColor;
            set
            {
                _tintColor = value;
                this.ApplyTintColor();
            }
        }

        protected virtual void Awake()
        {
            this.GetComponentsInChildren(true, _graphics);

            foreach (Graphic g in _graphics)
            {
                if (g != null) _originalColors[g] = g.color;
            }

            this.ApplyTintColor();
        }

        private void ApplyTintColor()
        {
            foreach (Graphic g in _graphics)
            {
                if (g == null) continue;
                g.color = _originalColors[g] * _tintColor;
            }
        }
    }
}
