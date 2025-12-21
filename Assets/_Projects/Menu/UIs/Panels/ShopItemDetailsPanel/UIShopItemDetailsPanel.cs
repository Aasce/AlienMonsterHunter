using Asce.Game.UIs.Panels;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UIShopItemDetailsPanel : UIUnlockPanel
    {
        [Header("Shop Item Details Panel")]
        [SerializeField] private TextMeshProUGUI _description;

        protected override void Reset()
        {
            base.Reset();
            _name = "Shop Item Details";
        }

    }
}
