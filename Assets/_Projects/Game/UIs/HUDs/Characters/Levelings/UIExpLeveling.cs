using Asce.Game.Levelings;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.HUDs
{
    public class UIExpLeveling : UIObject
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _expSlider;

        [Space]
        [SerializeField, Readonly] private ExpLeveling _leveling;


        public ExpLeveling ExpLeveling
        {
            get => _leveling;
            set
            {
                if (_leveling == value) return;
                this.Unregister();
                _leveling = value;
                this.Register();
            }
        }


        private void Register()
        {
            if (ExpLeveling == null) return;

            _levelText.text = ExpLeveling.CurrentLevel.ToString();
            _expSlider.maxValue = ExpLeveling.ExpToLevelUp();
            _expSlider.value = ExpLeveling.CurrentExp;

            ExpLeveling.OnCurrentExpChanged += ExpLeveling_OnCurrentExpChanged;
            ExpLeveling.OnLevelChanged += ExpLeveling_OnLevelChanged;
        }

        private void Unregister()
        {
            if (ExpLeveling == null) return;
            ExpLeveling.OnCurrentExpChanged -= ExpLeveling_OnCurrentExpChanged;
            ExpLeveling.OnLevelChanged -= ExpLeveling_OnLevelChanged;
        }

        private void ExpLeveling_OnCurrentExpChanged(int newExp)
        {
            _expSlider.value = newExp;
        }

        private void ExpLeveling_OnLevelChanged(int newLevel)
        {
            _levelText.text = newLevel.ToString();
            _expSlider.maxValue = ExpLeveling.ExpToLevelUp();
        }
    }
}