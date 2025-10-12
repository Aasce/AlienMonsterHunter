using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs
{
    [System.Serializable]
    public struct TabView
    {
        [SerializeField] private Button _tab;
        [SerializeField] private RectTransform _view;

        public readonly Button Tab => _tab;
        public readonly RectTransform View => _view;
    }
}