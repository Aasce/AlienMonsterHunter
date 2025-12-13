using Asce.Core;
using UnityEngine;

namespace Asce.Game.VFXs
{
    public abstract class VFXSpawner : GameComponent
    {
        [SerializeField] protected string _vfxName = string.Empty;

        public abstract void Spawn(params Vector2[] points);
    }
}