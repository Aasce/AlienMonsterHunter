using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.Managers;
using Asce.PrepareGame.UIs;
using UnityEngine;

namespace Asce.PrepareGame.Players
{
    public class PrepareGamePlayer : MonoBehaviourSingleton<PrepareGamePlayer>
    {
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


        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;

            Vector2 worldMousePosition = CameraController.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            bool isReload = Input.GetKeyDown(KeyCode.R);
            bool isShoot = Input.GetMouseButtonDown(0);
            if (isShoot)
            {
                bool isPointerOverUI = UIPrepareGameController.Instance.IsPointerOverScreenUI();
                if (isPointerOverUI) isShoot = false;
            }

            if (Character != null)
            {
                Character.Move(moveDirection);
                Character.LookAt(worldMousePosition);
                if (isReload) Character.Reload();
                if (isShoot) Character.Shoot();
            }
        }
    }
}
