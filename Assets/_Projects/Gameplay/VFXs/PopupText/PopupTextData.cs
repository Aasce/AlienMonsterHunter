using TMPro;
using UnityEngine;

namespace Asce.Game.VFXs
{
    /// <summary>
    ///     Contains popup text display information.
    /// </summary>
    [System.Serializable]
    public struct PopupTextData
    {
        [SerializeField] private string _text;
        [SerializeField] private Color _color;
        [SerializeField] private float _size;
        [SerializeField] private FontStyles _style;

        public string Text 
        {
            readonly get => _text;
            set => _text = value;
        }
        public Color Color 
        {
            readonly get => _color;
            set => _color = value;
        }
        public float Size 
        {
            readonly get => _size;
            set => _size = value;
        }
        public FontStyles Style
        {
            readonly get => _style;
            set => _style = value;
        }

        public PopupTextData(string text, Color color, float size = 144f, FontStyles style = FontStyles.Normal)
        {
            _text = text;
            _color = color;
            _size = size;
            _style = style;
        }
    }
}