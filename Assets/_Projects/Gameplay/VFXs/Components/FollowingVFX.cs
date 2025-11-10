using Asce.Managers;
using UnityEngine;

namespace Asce.Game.VFXs
{
    public class FollowingVFX : GameComponent
    {
        [SerializeField] private Transform _target;

        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        private void LateUpdate()
        {
            if (Target == null) return;
            transform.position = Target.position;
        }
    }

}