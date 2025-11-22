using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.Panels
{
    public class UIUnlockCharacterPanel : UIUnlockPanel
    {

        protected override void Reset()
        {
            base.Reset();
            _name = "Unlock Character";
        }

    }
}