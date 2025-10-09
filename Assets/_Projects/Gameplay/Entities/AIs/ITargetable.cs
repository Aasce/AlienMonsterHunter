using UnityEngine;

namespace Asce.Game.Entities
{
    public interface ITargetable
    {
        bool IsTargetable { get; }
        Transform transform { get; }
    }
}