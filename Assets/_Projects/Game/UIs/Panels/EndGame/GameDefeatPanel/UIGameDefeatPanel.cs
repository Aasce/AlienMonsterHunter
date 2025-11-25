using Asce.Game.UIs;
using Asce.Game.UIs.Panels;
using Asce.MainGame.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asce.MainGame.UIs.Panels
{
    public class UIGameDefeatPanel : UIPanel
    {
        [SerializeField] private UIClickBlock _clickBlock;

        protected override void Reset()
        {
            base.Reset();
            _name = "Game Defeat";
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
