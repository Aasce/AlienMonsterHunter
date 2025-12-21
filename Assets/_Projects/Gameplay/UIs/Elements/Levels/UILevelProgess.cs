using Asce.Game.Players;
using Asce.Core.UIs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.Elements
{
    public class UILevelProgess : UIComponent
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _progessText;
        [SerializeField] private Slider _expSlider;

        public void Set(CharacterProgress progress)
        {
            _levelText.text = $"Lv. {progress.Level}";

            if (progress.IsMaxLevel)
            {
                _progessText.text = "MAX";
                _expSlider.maxValue = 1000;
                _expSlider.value = 1000;
            }
            else
            {
                int expToLevelUp = progress.ExpToLevelUp(progress.Level);
                _progessText.text = $"{progress.Exp}/{expToLevelUp}"; 
                _expSlider.maxValue = expToLevelUp;
                _expSlider.value = progress.Exp;
            }
        }
    }
}
