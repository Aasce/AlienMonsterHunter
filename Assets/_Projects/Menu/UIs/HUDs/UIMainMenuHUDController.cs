using Asce.Game.UIs.HUDs;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Menu.UIs.HUDs
{
    public class UIMainMenuHUDController : UIHUDController
    {
        [SerializeField] private Button _playButton;

        private void Start()
        {
            if (_playButton != null) _playButton.onClick.AddListener(PlayButton_OnClick);
        }

        private void PlayButton_OnClick()
        {
            MenuManager.Instance.PlayNewGame();
        }
    }
}