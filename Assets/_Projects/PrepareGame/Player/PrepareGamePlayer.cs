using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.Game.VFXs;
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
            
            bool isShoot = Input.GetMouseButtonDown(0);
            if (isShoot)
            {
                bool isPointerOverUI = UIPrepareGameController.Instance.IsPointerOverScreenUI();
                if (isPointerOverUI) isShoot = false;
            }
            bool isReload = Input.GetKeyDown(KeyCode.R);
            bool isUseAbility0 = Input.GetKeyDown(KeyCode.Q);
            bool isUseAbility1 = Input.GetKeyDown(KeyCode.E);

            if (Character != null)
            {
                Character.Move(moveDirection);
                Character.LookAt(worldMousePosition);
                if (isReload) Character.Reload();
                if (isShoot) Character.Shoot();
                if (isUseAbility0) Character.UseAbility(0, worldMousePosition);
                if (isUseAbility1) Character.UseAbility(1, worldMousePosition);
            }
        }
    }
}
