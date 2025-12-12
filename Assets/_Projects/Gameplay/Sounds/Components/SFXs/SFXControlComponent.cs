using UnityEngine;

namespace Asce.Game.Sounds 
{
	public abstract class SFXControlComponent : SFXComponent
    {
		public abstract AudioSource Source { get; set; }
	}
}