using Asce.Core.Attributes;
using Asce.Core.UIs;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.Panels
{
    public class UISettingContent : UIComponent
    {
        [SerializeField, Readonly] protected UISettingsPanel _panel;

        public UISettingsPanel Panel
        {
            get => _panel;
            set => _panel = value;
        }

        public virtual void Initialize()
        {

        }

    }
}
