using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Managers
{
    public abstract class GameStateCondition : GameComponent
    {
        public virtual void Initialize() { }
        public virtual void OnCheck() { }

        public abstract bool IsSatisfied();
    }
}