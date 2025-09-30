using Asce.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Managers
{
    public class CameraController : MonoBehaviourSingleton<CameraController>
    {
        [SerializeField] private Camera _camera;

        [Header("Settings")]
        [SerializeField] private Transform _target;
        [SerializeField] private Vector2 _offset;
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

        public Vector2 Offset
        {
            get => _offset;
            set => _offset = value;
        }
        

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }


        private void LateUpdate()
        {
            if (_target == null) return;
            Vector3 desiredPosition = new Vector3(Target.position.x, Target.position.y, MainCamera.transform.position.z) + (Vector3)Offset;
            Vector3 smoothedPosition = Vector3.Lerp(MainCamera.transform.position, desiredPosition, Speed * Time.deltaTime);
            MainCamera.transform.position = smoothedPosition;
        }
    }
}
