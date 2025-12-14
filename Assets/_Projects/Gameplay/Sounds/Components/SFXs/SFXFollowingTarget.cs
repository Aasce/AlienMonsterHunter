using Asce.Core;
using UnityEngine;

namespace Asce.Game.Sounds
{
    public class SFXFollowingTarget : SFXControlComponent
    {
        [SerializeField] private Transform _target;

        [Space]
        [SerializeField] private bool _isFollowing = true;

        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        public bool IsFollowing
        {
            get => _isFollowing;
            set => _isFollowing = value;
        }

        private void Update()
        {
            if (!_isFollowing) return;
            if (_source == null) return;
            if (_target == null) return;

            _source.transform.position = _target.position;
        }
    }
}
