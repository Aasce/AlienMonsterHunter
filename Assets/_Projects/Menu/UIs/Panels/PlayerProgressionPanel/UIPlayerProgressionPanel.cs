using Asce.Game.UIs.Panels;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UIPlayerProgressionPanel : UIFullScreenPanel
    {
        protected override void RefReset()
        {
            base.RefReset();
            _name = "Player Progression";
        }

        protected override void Start()
        {
            base.Start();

        }


    }
}
