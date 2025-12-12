using System;
using UnityEngine;

namespace Asce.Game.Sounds 
{
	public abstract class SFXPlayerComponent : SFXComponent
    {
		public abstract event Action<AudioSource> OnSFXPlayed;

	}
}