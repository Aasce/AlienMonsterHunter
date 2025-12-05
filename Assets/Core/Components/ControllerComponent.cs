using Asce.Core.Attributes;
using UnityEngine;

namespace Asce.Core
{
    public abstract class ControllerComponent : GameComponent
    {
        public abstract string ControllerName { get; }

        protected override void Reset()
        {
            base.Reset();
        }

        public virtual void Initialize()
        {

        }

        public virtual void Ready()
        {

        }

    }
}