using Asce.Game.Entities.Characters;
using Asce.Game.Enviroments;
using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Game.Supports;
using Asce.MainGame.Managers;
using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Core.Utils;
using System;
using UnityEngine;

namespace Asce.MainGame.Players
{
    public class MainGamePlayer : Player, IPlayerControlCharacter, IPlayerSupportCaller
    {
        [Header("References")]
        [SerializeField, Readonly] private SupportCaller _supportCaller;

        [Header("Runtime")]
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
                OnCharacterChanged?.Invoke(new (oldCharacter, _character));
            }
        }

        public SupportCaller SupportCaller => _supportCaller;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _supportCaller);
        }

        public override void Initialize()
        {
            base.Initialize();
            if (Character != null)
            {
                CameraController.Instance.Target = Character.transform;
                CameraController.Instance.SetToTarget();
            }

            OnCharacterChanged += MainGamePlayer_OnCharacterChanged;
            MainGameManager.Instance.GameStateController.OnEndGame += GameStateController_OnEndGame;
        }

        public void ReviveCharacter(bool isReviveAtSpawnPoint = false)
        {
            if (Character == null) return;
            Character.ResetStatus();

            if (isReviveAtSpawnPoint)
            {
                Vector2 spawnPoint = EnviromentController.Instance.CharacterSpawnPoint;
                Character.transform.position = spawnPoint;
                CameraController.Instance.SetToTarget();
            }

            Character.gameObject.SetActive(true);
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
            bool isAim = Input.GetMouseButton(2);

            bool isReload = Input.GetKeyDown(Settings.ReloadKey);
            bool isInteract = Input.GetKeyDown(Settings.InteractKey);

            for (int i = 0; i < Settings.CallSupportKeys.Count; i++)
            {
                KeyCode key = Settings.CallSupportKeys[i];
                if (Input.GetKeyDown(key))
                {
                    _supportCaller.Call(i, worldMousePosition);
                }
            }

            if (Character != null && Character.gameObject.activeInHierarchy)
            {
                Character.Move(Settings.MoveInput);
                Character.LookAt(worldMousePosition);
                if (isFire) Character.Fire();
                if (isAltFire) Character.AltFire();
                if (isReload) Character.Reload();
                if (isAim)
                {
                    Vector2 lookDirection = worldMousePosition - (Vector2)Character.transform.position;
                    float offsetLenght = Mathf.Min(lookDirection.magnitude, 12f) * 0.75f;
                    CameraController.Instance.Offset = lookDirection.normalized * offsetLenght;
                }
                else CameraController.Instance.Offset = Vector2.zero;

                for(int i = 0; i < Settings.UseAbilityKeys.Count; i++)
                {
                    KeyCode key = Settings.UseAbilityKeys[i];
                    if (Input.GetKeyDown(key))
                    {
                        Character.UseAbility(i, worldMousePosition);
                    }
                }

                if (isInteract) Character.Interact();

                if (Input.GetKeyDown(KeyCode.P)) Character.Gun.Leveling.SetLevel(10);
                if (Input.GetKeyDown(KeyCode.O)) Character.Leveling.SetLevel(10);
                if (Input.GetKeyDown(KeyCode.I)) foreach (var abi in Character.Abilities.ActiveAbilities) abi.Level = 10;
                if (Input.GetKeyDown(KeyCode.I)) foreach (var sup in SupportCaller.Supports) sup.Level = 10;
                if (Input.GetKeyDown(KeyCode.G)) Character.SetSize(Character.Size + 0.1f);
            }
        }

        private void MainGamePlayer_OnCharacterChanged(ValueChangedEventArgs<Character> args)
        {
            if (Character != null)
            {
                CameraController.Instance.Target = Character.transform;
                CameraController.Instance.SetToTarget();
            }
        }

        private void GameStateController_OnEndGame()
        {
            var progress = PlayerManager.Instance.Progress.CharactersProgress.Get(Character.Information.Name); 
            if (progress != null) progress.SetLevel (
                Character.Leveling.CurrentLevel,
                Character.Leveling.CurrentExp
            );
        }

    }
}
