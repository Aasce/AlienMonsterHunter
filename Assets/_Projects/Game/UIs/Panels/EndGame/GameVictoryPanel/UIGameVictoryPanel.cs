using Asce.Game.UIs;
using Asce.Game.UIs.Panels;
using Asce.MainGame.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Asce.MainGame.UIs.Panels
{

    public class UIGameVictoryPanel : UIPanel
    {
        [SerializeField] private UIClickBlock _clickBlock;

        protected override void Reset()
        {
            base.Reset();
            _name = "Game Victory";
        }

        public override void Initialize()
        {
            base.Initialize();
            _clickBlock.OnClick += ClickBlock_OnClick;
        }

        private void ClickBlock_OnClick(PointerEventData eventData)
        {
            MainGameManager.Instance.ToResultGame();
        }
    }
}
