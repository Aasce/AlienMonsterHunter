using Asce.Core.UIs;
using Asce.Core.Utils;
using TMPro;
using UnityEngine;

namespace Asce.Game.UIs.Elements
{
    public class UIStatItem : UIComponent
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _valueText;

        public void Set(string name, float value)
        {
            _nameText.text = $"{name.SplitByUppercase()}:";
            _valueText.text = value.ToString();
        }
    }
}
