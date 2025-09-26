using UnityEngine;

namespace Asce.Managers.Utils
{
    public static class CoordinateUtils
    {
        /// <summary>
        ///     Check if a given point in viewport coordinates is within the viewport bounds [0,1] for both x and y.
        /// </summary>
        /// <param name="point"> Point in viewport coordinates to check.  </param>
        /// <returns>
        ///     Returns true if the point is within the viewport (0 <= x <= 1 and 0 <= y <= 1), false otherwise.
        /// </returns>
        public static bool IsPointInViewport(Vector2 point)
        {
            return point.x >= 0f && point.x <= 1f 
                && point.y >= 0f && point.y <= 1f;
        }

    }
}