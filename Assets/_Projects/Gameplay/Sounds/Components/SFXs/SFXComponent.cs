using Asce.Core;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.Sounds
{
    public class SFXComponent : GameComponent
    {
        [SerializeField, Readonly] protected AudioSource _source;

    }
}
