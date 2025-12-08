using Asce.Game.UIs.HUDs;
using UnityEngine;

namespace Asce.MainMenu.UIs.HUDs
{
    public class UIMainMenuHUDController : UIHUDController
    {
        [SerializeField] private UIMainMenuPlayButton _playButton;

        public override void Initialize()
        {
            base.Initialize();
            _playButton.Initialize();
        }

    }
}