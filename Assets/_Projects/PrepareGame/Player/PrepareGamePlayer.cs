using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Core;
using Asce.Core.UIs;
using System;
using UnityEngine;

namespace Asce.PrepareGame.Players
{
    public class PrepareGamePlayer : Player, IPlayerControlCharacter
    {
        [SerializeField] private Character _character;

        public event Action<ValueChangedEventArgs<Character>> OnCharacterChanged;

        public Character Character
        {
            get => _character;
            set
            {
                if (_character == value) return;
                Character oldCharacter = _character;
                _character = value;
                OnCharacterChanged?.Invoke(new(oldCharacter, _character));
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void Update()
        {
            Vector2 worldMousePosition = CameraController.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);

            bool isFire = Input.GetMouseButton(0);
            if (isFire)
            {
                bool isPointerOverUI = UIManager.Instance.IsPointerOverScreenUI();
                if (isPointerOverUI) isFire = false;
            }

            bool isAltFire = Input.GetMouseButtonDown(1);
            if (isAltFire)
            {
                bool isPointerOverUI = UIManager.Instance.IsPointerOverScreenUI();
                if (isPointerOverUI) isAltFire = false;
            }
            bool isReload = Input.GetKeyDown(Settings.ReloadKey);
            bool isInteract = Input.GetKeyDown(Settings.InteractKey);

            if (Character != null && Character.gameObject.activeInHierarchy)
            {
                Character.Move(Settings.MoveInput);
                Character.LookAt(worldMousePosition);
                if (isFire) Character.Fire();
                if (isAltFire) Character.AltFire();
                if (isReload) Character.Reload();

                for (int i = 0; i < Settings.UseAbilityKeys.Count; i++)
                {
                    KeyCode key = Settings.UseAbilityKeys[i];
                    if (Input.GetKeyDown(key))
                    {
                        Character.UseAbility(i, worldMousePosition);
                    }
                }

                if (isInteract) Character.Interact();
            }
        }
    }
}
