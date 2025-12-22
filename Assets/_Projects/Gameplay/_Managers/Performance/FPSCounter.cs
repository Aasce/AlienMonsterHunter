using Asce.Core;
using UnityEngine;

namespace Asce.Game.Managers.Performance
{
    public static class FPSCounter
    {
        public static float CurrentFPS
        {
            get => 1f / Time.unscaledDeltaTime;
        }

    }
}
