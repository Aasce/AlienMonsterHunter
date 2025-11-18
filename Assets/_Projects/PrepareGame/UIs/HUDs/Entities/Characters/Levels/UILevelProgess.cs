using Asce.Game.Entities.Characters;
using Asce.Game.Players;
using Asce.Managers.UIs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UILevelProgess : UIObject
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _progessText;
        [SerializeField] private Slider _expSlider;


        public void Set(CharacterProgress progress)
        {
            _levelText.text = $"Lv. {progress.Level}";

            int expToLevelUp = progress.ExpToLevelUp(progress.Level);
            _expSlider.maxValue = expToLevelUp;
            _expSlider.value = progress.Exp;
            if (progress.IsMaxLevel())
            {
                _progessText.text = "MAX";
            }
            else
            {
                _progessText.text = $"{progress.Exp}/{expToLevelUp}";
            }
        }
    }
}
