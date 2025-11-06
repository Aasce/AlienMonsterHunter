using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Players
{
    public abstract class Player : GameComponent
    {
        [SerializeField] protected PlayerSettings _settings;

        public PlayerSettings Settings => _settings;


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out  _settings);
        }

        public virtual void Initialize()
        {

        }
    }
}