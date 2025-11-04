using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.Game.Supports;
using Asce.Managers;
using Asce.Managers.UIs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Players
{
    public class Player : MonoBehaviourSingleton<Player>
    {
        [SerializeField] private Transform _spawnPoint;

        [Header("Settings")]
        [SerializeField] private List<KeyCode> _callSupportKeys = new()
        {
            KeyCode.Z,
            KeyCode.X,
        };

        [SerializeField] private List<KeyCode> _useAbilityKeys = new()
        {
            KeyCode.Q,
            KeyCode.E,
            KeyCode.C,
        };

        [Header("Runtime")]
        [SerializeField] private Character _character;
        [SerializeField] private SupportCaller _supportCaller;
        [SerializeField] private List<string> _supports = new();

        public event Action<ValueChangedEventArgs<Character>> OnCharacterChanged;

        public List<KeyCode> CallSupportKeys => _callSupportKeys;
        public List<KeyCode> UseAbilityKeys => _useAbilityKeys;

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
        public List<string> Supports => _supports;

        public Vector2 SpawnPoint => _spawnPoint != null ? _spawnPoint.position : Vector2.zero;

        public void Initialize()
        {
            if (Character != null)
            {
                CameraController.Instance.Target = Character.transform;
                CameraController.Instance.SetToTarget();
            }
        }
        public void InitializeCharacter() 
        {
            Character.Initialize();
        }
        public void InitializeSupportCaller()
        {
            SupportCaller.Initialize(Supports);
        }

        public void ReviveCharacter(bool isReviveAtSpawnPoint = false)
        {
            if (Character == null) return;
            Character.ResetStatus();

            if (isReviveAtSpawnPoint)
            {
                Vector2 spawnPoint = _spawnPoint != null ? _spawnPoint.position : Vector2.zero;
                Character.transform.position = spawnPoint;
                CameraController.Instance.SetToTarget();
            }

            Character.gameObject.SetActive(true);
        }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;

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
            bool isReload = Input.GetKeyDown(KeyCode.R);
            bool isInteract = Input.GetKeyDown(KeyCode.F);

            for (int i = 0; i < _callSupportKeys.Count; i++)
            {
                KeyCode key = _callSupportKeys[i];
                if (Input.GetKeyDown(key))
                {
                    _supportCaller.Call(i, worldMousePosition);
                }
            }

            bool isPressU = Input.GetKeyDown(KeyCode.U);

            if (Character != null && Character.gameObject.activeInHierarchy)
            {
                Character.Move(moveDirection);
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

                for(int i = 0; i < _useAbilityKeys.Count; i++)
                {
                    KeyCode key = _useAbilityKeys[i];
                    if (Input.GetKeyDown(key))
                    {
                        Character.UseAbility(i, worldMousePosition);
                    }
                }

                if (isInteract) Character.Interact();

                if (isPressU)
                {
                    Character.Leveling.AddExp(998);
                    Debug.Log($"Character Add Exp({Character.Leveling.CurrentExp}/{Character.Leveling.ExpToLevelUp()}) - Level {Character.Leveling.CurrentLevel}");
                }
            }
        }
    }
}
