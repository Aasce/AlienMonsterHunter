using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Game.Maps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UIMapLevelConditionInformationItem : UIComponent
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        [Header("Runtime")]
        [SerializeField, Readonly] private MapLevelGameStateCondition _levelCondition;

        public void Set(MapLevelGameStateCondition condition)
        {
            this.Unregister();
            _levelCondition = condition;
            this.Register();
        }

        private void Register()
        {
            if (_levelCondition == null) return;
            _nameText.text = _levelCondition.ConditionName;
            _descriptionText.text = _levelCondition.Description;
        }

        private void Unregister()
        {
            if (_levelCondition == null) return;

        }
    }
}
