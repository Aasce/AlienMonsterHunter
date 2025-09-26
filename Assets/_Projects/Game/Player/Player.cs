using Asce.Game.Managers;
using Asce.Game.Entities;
using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Players
{
    public class Player : MonoBehaviourSingleton<Player>
    {
        [SerializeField] private Character _characterPrefab;
        [SerializeField] private Transform _spawnPoint;

        [Header("Runtime")]
        [SerializeField] private Character _character;

        public Character Character
        {
            get => _character;
            set
            {
                if (_character == value) return;
                _character = value;
            }
        }

        private void Start()
        {
            Character = GameObject.Instantiate(_characterPrefab);
            if (Character != null)
            {
                Vector2 spawnPoint = _spawnPoint != null ? _spawnPoint.position : Vector2.zero;
                Character.transform.position = spawnPoint;
                CameraController.Instance.Target = Character.transform;
            }
        }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;

            Vector2 worldMousePosition = CameraController.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);


            if (Character != null)
            {
                Character.Move(moveDirection);
                Character.LookAt(worldMousePosition);
            }
        }
    }
}
