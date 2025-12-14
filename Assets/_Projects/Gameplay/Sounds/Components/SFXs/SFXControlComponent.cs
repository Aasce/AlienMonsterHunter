using UnityEngine;

namespace Asce.Game.Sounds 
{
	public abstract class SFXControlComponent : SFXComponent
    {
        public virtual AudioSource Source
        {
            get => _source;
            set => _source = value;
        }

    }
}