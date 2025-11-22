using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.Panels
{
    public class UIUnlockGunPanel : UIUnlockPanel
    {

        protected override void Reset()
        {
            base.Reset();
            _name = "Unlock Gun";
        }

    }
}