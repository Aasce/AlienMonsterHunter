using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Interactions
{
    public abstract class InteractiveObject : GameComponent
    {
        [SerializeField] private SO_InteractiveInformation _information;

        public SO_InteractiveInformation Information => _information;

        public virtual void Initialize() { }
        public virtual void OnLoad() { }
        public virtual void ResetStatus() { }
        public virtual void OnSpawn() { }
        public virtual void OnActive() { }
    }
}
