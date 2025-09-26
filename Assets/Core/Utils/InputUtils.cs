using UnityEngine;

namespace Asce.Managers.Utils
{
    /// <summary>
    ///     Utility class for handling input across different platforms.
    /// </summary>
    public static class InputUtils
    {
        /// <summary>
        ///     Get current pointer position.<br/>
        ///     - On PC: mouse position.<br/>
        ///     - On Mobile: first touch position.<br/>
        ///     Return default if no input.
        /// </summary>
        public static Vector2 GetCurrentPointerPosition()
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            // Always available on PC (mouse)
            return Input.mousePosition;

#elif UNITY_ANDROID || UNITY_IOS
        // On mobile, use touch if available
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;
        else
            return default;

#else
        // Fallback for other platforms
        return Input.mousePosition;
#endif
        }
    }
}