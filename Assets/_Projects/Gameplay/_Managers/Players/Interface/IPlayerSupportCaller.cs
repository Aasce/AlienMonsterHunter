using Asce.Game.Supports;
using UnityEngine;

namespace Asce.Game.Players
{
    public interface IPlayerSupportCaller
    {
        public SupportCaller SupportCaller { get; }
    }
}