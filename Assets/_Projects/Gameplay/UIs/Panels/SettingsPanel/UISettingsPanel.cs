using UnityEngine;

namespace Asce.Game.UIs.Panels
{
    public class UISettingsPanel : UIWindowPanel
    {
        protected override void Reset()
        {
            base.Reset();
            _name = "Settings";
        }
    }
}
