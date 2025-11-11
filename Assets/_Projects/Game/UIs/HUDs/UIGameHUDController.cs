using Asce.Game.UIs;
using Asce.Game.UIs.HUDs;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.MainGame.UIs.HUDs
{
    [RequireComponent(typeof(Canvas))]
    public class UIGameHUDController : UIHUDController
    {
        [Space]
        [SerializeField] UICharacterInformation _characterInformation;
        [SerializeField] UIGunInformation _gunInformation;
        [SerializeField] UISupportsInformation _supportsInformation;

        public UICharacterInformation CharacterInformation => _characterInformation;
        public UIGunInformation GunInformation => _gunInformation;
        public UISupportsInformation SupportsInformation => _supportsInformation;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _characterInformation);
            this.LoadComponent(out _gunInformation);
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
