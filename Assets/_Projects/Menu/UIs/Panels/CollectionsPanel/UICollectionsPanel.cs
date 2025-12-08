using Asce.Game.UIs.Panels;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UICollectionsPanel : UIFullScreenPanel
    {
        protected override void RefReset()
        {
            base.RefReset();
            _name = "Collections";
        }

        protected override void Start()
        {
            base.Start();

        }


    }
}
