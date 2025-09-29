using Asce.Managers.UIs;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.UIs.HUDs
{
    [RequireComponent(typeof(Canvas))]
    public class UIHUDController : UIObject
    {
        [SerializeField] private Canvas _canvas;

        [Space]
        [SerializeField] UICharacterInformation _characterInformation;


        public Canvas Canvas => _canvas;
        public UICharacterInformation CharacterInformation => _characterInformation;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _canvas);
            this.LoadComponent(out _characterInformation);
        }
    }
}
