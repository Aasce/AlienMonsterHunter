using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.Sounds
{
    public class ListenerController : MonoBehaviourSingleton<ListenerController>
    {
        [SerializeField, Readonly] private AudioListener _listener;

        [Header("Settings")]
        [SerializeField] private Transform _target;
        [SerializeField] private Vector2 _offset;

        public AudioListener Listener => _listener;
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


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _listener);
        }

        private void LateUpdate()
        {
            if (_target == null) return;
            _listener.transform.position = (Vector2)_target.position + Offset;
        }

    }
}
