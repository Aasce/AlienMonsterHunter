using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Supports
{
    public class Support : GameComponent
    {
        [SerializeField] protected SO_SupportInformation _information;
        [SerializeField] protected Vector2 _callPosition;

        public SO_SupportInformation Information => _information;
        public virtual Vector2 CallPosition
        {
            get => _callPosition;
            set => _callPosition = value;
        }

        protected virtual void Start()
        {
            
        }

        public virtual void Initialize() { }
        public virtual void OnSpawn()
        {

        }

        public virtual void OnActive() { }
        public virtual void Recall() { }
    }
}
