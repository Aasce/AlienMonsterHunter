using Asce.Game.UIs;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    [RequireComponent(typeof(Canvas))]
    public class UIPrepareGameHUDController : UIObject
    {
        [SerializeField] private Canvas _canvas;

        [Space]
        [SerializeField] private Button _playbutton;
        [SerializeField] private UITabs _tabs;
        [SerializeField] private UIPicked _picked;

        public Canvas Canvas => _canvas;

        public Button PlayButton => _playbutton;
        public UITabs Tabs => _tabs;
        public UIPicked Picked => _picked;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _canvas);
            this.LoadComponent(out _picked);
        }

        private void Start()
        {
            if (PlayButton != null) PlayButton.onClick.AddListener(PlayButton_OnClick);
        }

        private void PlayButton_OnClick()
        {
            PrepareGameManager.Instance.PlayGame();
        }
    }
}
