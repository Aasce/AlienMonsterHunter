using Asce.Game.Managers;
using Asce.Core.UIs;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Asce.ResultGame.UIs.HUDs
{
    public class UIResultTitle : UIComponent
    {
        [SerializeField] private TextMeshProUGUI _resultText;

        [Space]
        [SerializeField] private List<ResultColor> _resultColors = new();

        public void Initialize()
        {

        }

        public void Set(GameResultType resultType)
        {
            int index = _resultColors.FindIndex(res => res.ResultType == resultType);
            Color color = index > 0 ? _resultColors[index].Color : Color.white;

            _resultText.text = resultType.ToString();
            _resultText.color = color;
        }

    }

    [System.Serializable]
    public struct ResultColor
    {
        [SerializeField] private GameResultType _resultType;
        [SerializeField] private Color _color;

        public readonly GameResultType ResultType => _resultType;
        public readonly Color Color => _color;
    }
}
