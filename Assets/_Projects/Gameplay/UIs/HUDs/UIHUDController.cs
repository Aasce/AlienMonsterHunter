using Asce.Game.Entities;
using Asce.Game.Guns;
using Asce.Game.Players;
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
        [SerializeField] UIGunInformation _gunInformation;

        [Space]
        [SerializeField] private Character _character;

        public Canvas Canvas => _canvas;
        public UICharacterInformation CharacterInformation => _characterInformation;
        public UIGunInformation GunInformation => _gunInformation;

        public Character Character
        {
            get => _character;
            set
            {
                if (_character == value) return;
                this.Unregister();
                _character = value;
                this.Register();
            }
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _canvas);
            this.LoadComponent(out _characterInformation);
            this.LoadComponent(out _gunInformation);
        }

        private void Start()
        {
            Character = Player.Instance.Character;
            Player.Instance.OnCharacterChanged += Player_OnCharacterChanged;
        }

        private void Register()
        {
            if (CharacterInformation != null) CharacterInformation.Character = Character;
            if (GunInformation != null) GunInformation.Character = Character;
        }

        private void Unregister()
        {
            if (Character == null) return;
        }


        private void Player_OnCharacterChanged(Character character)
        {
            Character = character;
        }
    }
}
