using Asce.Game.Players;
using Asce.Game.UIs;
using Asce.Game.UIs.HUDs;
using Asce.Managers.Utils;
using Asce.PrepareGame.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    [RequireComponent(typeof(Canvas))]
    public class UIPrepareGameHUDController : UIHUDController
    {
        [Space]
        [SerializeField] private Button _playbutton;
        [SerializeField] private UITabs _tabs;
        [SerializeField] private UIPicked _picked;
        [SerializeField] private UICurrencies _currencies;

        public Button PlayButton => _playbutton;
        public UITabs Tabs => _tabs;
        public UIPicked Picked => _picked;
        public UICurrencies Currencies => _currencies;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _picked);
        }

        public override void Initialize()
        {
            base.Initialize();
            if (PlayButton != null) PlayButton.onClick.AddListener(PlayButton_OnClick);
            Currencies.Set(PlayerManager.Instance.Currencies.AllCurrencies);
        }


        private void PlayButton_OnClick()
        {
            PrepareGameManager.Instance.PlayGame();
        }
    }
}
