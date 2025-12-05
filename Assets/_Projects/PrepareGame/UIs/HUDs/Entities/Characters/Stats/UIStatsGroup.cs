using Asce.Game.Entities;
using Asce.Core.UIs;
using TMPro;
using UnityEngine;

namespace Asce.PrepareGame.UIs
{
    public class UIStatsGroup : UIComponent
    {
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _armorText;
        [SerializeField] private TextMeshProUGUI _speedText;

        public void Set(SO_EntityStats stats)
        {
            if (stats == null)
            {
                this.SetText(_healthText, 0f);
                this.SetText(_armorText, 0f);
                this.SetText(_speedText, 0f);
            }
            else
            {
                this.SetText(_healthText, stats.MaxHealth);
                this.SetText(_armorText, stats.Armor);
                this.SetText(_speedText, stats.Speed);
            }
        }

        private void SetText(TextMeshProUGUI text, float value)
        {
            if (text == null) return;
            text.text = value.ToString();
        }
    }
}
