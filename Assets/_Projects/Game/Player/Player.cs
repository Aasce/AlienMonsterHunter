using Asce.Game.Managers;
using Asce.Game.Entities.Characters;
using Asce.Managers;
using UnityEngine;
using System;

namespace Asce.Game.Players
{
    public class Player : MonoBehaviourSingleton<Player>
    {
        [SerializeField] private Character _characterPrefab;
        [SerializeField] private Transform _spawnPoint;

        [Header("Runtime")]
        [SerializeField] private Character _character;

        public event Action<Character> OnCharacterChanged;

        public Character Character
        {
            get => _character;
            set
            {
                if (_character == value) return;
                _character = value;
                OnCharacterChanged?.Invoke(_character);
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

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;

            Vector2 worldMousePosition = CameraController.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);

            bool isShoot = Input.GetMouseButtonDown(0);
            bool isAim = Input.GetMouseButton(2);
            bool isReload = Input.GetKeyDown(KeyCode.R);
            bool isUseAbility0 = Input.GetKeyDown(KeyCode.E);
            bool isUseAbility1 = Input.GetKeyDown(KeyCode.Q);

            if (Character != null)
            {
                Character.Move(moveDirection);
                Character.LookAt(worldMousePosition);
                if (isShoot) Character.Shoot();
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
