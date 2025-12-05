using Asce.Core.Utils;
using TMPro;
using UnityEngine;

namespace Asce.Game.VFXs
{
    public class PopupTextVFXObject : VFXObject
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextMeshProUGUI _textMesh;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _canvas);
            this.LoadComponent(out _textMesh);
        }

        public void SetText(string text, Color color, float size, FontStyles style)
        {
            _textMesh.text = text;
            _textMesh.color = color;
            _textMesh.fontSize = size;
            _textMesh.fontStyle = style;
        }
    }
}