using Asce.Managers.UIs;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Menu.UIs
{
    public class UIMainMenuHUDController : UIObject
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