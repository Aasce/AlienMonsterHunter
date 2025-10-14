using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.Managers;
using Asce.Managers.UIs;
using System;
using UnityEngine;

namespace Asce.Game.Players
{
    public class Player : MonoBehaviourSingleton<Player>
    {
        [SerializeField] private Transform _spawnPoint;

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

        public void Initialize()
        {
            if (Character != null)
            {
                Vector2 spawnPoint = _spawnPoint != null ? _spawnPoint.position : Vector2.zero;
                Character.transform.position = spawnPoint;
                CameraController.Instance.Target = Character.transform;
                CameraController.Instance.SetToTarget();
            }
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
            bool isUseAbility0 = Input.GetKey(KeyCode.Q);
            bool isUseAbility1 = Input.GetKey(KeyCode.E);

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
                if (isUseAbility0) Character.UseAbility(0, worldMousePosition);
                if (isUseAbility1) Character.UseAbility(1, worldMousePosition);
            }
        }
    }
}
