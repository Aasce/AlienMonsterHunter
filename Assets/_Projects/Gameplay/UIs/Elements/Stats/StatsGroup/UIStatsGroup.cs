using Asce.Core.UIs;
using Asce.Core.Utils;
using Asce.Game.Entities;
using UnityEngine;

namespace Asce.Game.UIs.Elements
{
    public class UIStatsGroup : UIComponent
    {
        [SerializeField] private Pool<UIStatItem> _statsPool = new();

        public void Set(SO_EntityInformation information)
        {
            _statsPool.Clear(isDeactive: true, (item) => item.Hide());
            if (information == null || information.Stats == null)
            {
                return;
            }

            this.Set("Health", information.Stats.MaxHealth);
            this.Set("Armor", information.Stats.Armor);
            this.Set("Speed", information.Stats.Speed);
            foreach (CustomValue stat in information.Stats.CustomStats)
            {
                this.Set(stat.Name, stat.Value);
            }
        }

        private void Set(string name, float value)
        {
            UIStatItem uiStat = _statsPool.Activate();
            uiStat.Set(name, value);
            uiStat.RectTransform.SetAsLastSibling();
            uiStat.Show();
        }

    }
}
