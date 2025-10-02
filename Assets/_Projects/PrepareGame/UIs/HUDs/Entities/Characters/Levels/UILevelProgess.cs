using Asce.Game.Entities;
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
        [SerializeField] private Slider _levelSlider;


        public void Set(Character character)
        {

        }
    }
}
