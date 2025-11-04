using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Interactions
{
    public abstract class InteractiveObject : GameComponent
    {
        [SerializeField] private SO_InteractiveInformation _information;
        [SerializeField] private bool _isInteractable = true;
        [SerializeField] private float _interactDistance = 1.0f; 

        public SO_InteractiveInformation Information => _information;
        public bool IsInteractable => _isInteractable;
        public float InteractDistance => _interactDistance;

        public virtual void Initialize() { }
        public virtual void OnLoad() { }
        public virtual void ResetStatus() { }
        public virtual void OnSpawn() { }
        public virtual void OnActive() { }

        public abstract void Interact(GameObject interacter);
    }
}
