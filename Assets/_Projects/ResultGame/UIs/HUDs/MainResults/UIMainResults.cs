using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Core.Utils;
using Asce.Game.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.ResultGame.UIs.HUDs
{
    public class UIMainResults : UIComponent
    {
        [SerializeField] private UIAwards _awards;

        public UIAwards Awards => _awards;

        public void Initialize()
        {
            _awards.Initialize();
        }

        public void Ready()
        {
            _awards.Ready();
        }
    }
}
