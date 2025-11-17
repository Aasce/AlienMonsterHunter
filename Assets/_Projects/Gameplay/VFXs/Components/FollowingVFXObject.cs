using Asce.Managers;
using UnityEngine;

namespace Asce.Game.VFXs
{
    public class FollowingVFXObject : GameComponent
    {
        [SerializeField] private Transform _target;

        [Space]
        [SerializeField] private bool _followPosition = true;
        [SerializeField] private bool _rotateWithReceiver = false;

        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        public bool FollowPosition
        {
            get => _followPosition;
            set => _followPosition = value;
        }

        public bool RotateWithReceiver
        {
            get => _rotateWithReceiver;
            set => _rotateWithReceiver = value;
        }

        private void LateUpdate()
        {
            if (Target == null) return;

            if (_followPosition)
                transform.position = Target.position;

            if (_rotateWithReceiver)
                transform.rotation = Target.rotation;
            
        }

    }
}