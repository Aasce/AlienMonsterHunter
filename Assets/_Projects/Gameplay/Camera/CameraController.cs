using Asce.Managers;
using Codice.CM.Common;
using UnityEngine;

namespace Asce.Game.Managers
{
    public class CameraController : MonoBehaviourSingleton<CameraController>
    {
        [SerializeField] private Camera _camera;

        [Header("Settings")]
        [SerializeField] private Transform _target;
        [SerializeField, Range(0f, 10f)] private float _speed = 1.0f;
        
        public Camera MainCamera
        {
            get
            {
                if (_camera == null) _camera = Camera.main;
                return _camera;
            }
        }

        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }


        private void LateUpdate()
        {
            if (_target == null) return;
            Vector3 desiredPosition = new(Target.position.x, Target.position.y, MainCamera.transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(MainCamera.transform.position, desiredPosition, Speed * Time.deltaTime);
            MainCamera.transform.position = smoothedPosition;
        }
    }
}
