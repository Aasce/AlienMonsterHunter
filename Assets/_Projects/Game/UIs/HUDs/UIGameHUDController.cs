using Asce.Game.Entities.Characters;
using Asce.Game.Players;
using Asce.MainGame.Managers;
using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.UIs.HUDs
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

        public void SetSettings(PlayerSettings settings)
        {
            CharacterInformation.Abilities.SetUseKeys(settings.UseAbilityKeys);
            SupportsInformation.SetCallKeys(settings.CallSupportKeys);
        }

    }
}
