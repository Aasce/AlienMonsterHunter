namespace Asce.Managers.Utils
{
    using UnityEngine;

    public static class DebugUtils
    {
        /// <summary>
        ///     Draws a 3D Bounds box using <see cref="Debug.DrawLine"/>.
        /// </summary>
        public static void DrawBounds(Bounds bounds, Color color = default, float duration = 0f)
        {
            if (color == default) color = Color.green;

            Vector3 c = bounds.center;
            Vector3 e = bounds.extents;

            // 8 corners of the bounds
            Vector3 v000 = c + new Vector3(-e.x, -e.y, -e.z);
            Vector3 v001 = c + new Vector3(-e.x, -e.y, e.z);
            Vector3 v010 = c + new Vector3(-e.x, e.y, -e.z);
            Vector3 v011 = c + new Vector3(-e.x, e.y, e.z);
            Vector3 v100 = c + new Vector3(e.x, -e.y, -e.z);
            Vector3 v101 = c + new Vector3(e.x, -e.y, e.z);
            Vector3 v110 = c + new Vector3(e.x, e.y, -e.z);
            Vector3 v111 = c + new Vector3(e.x, e.y, e.z);

            // bottom square
            Debug.DrawLine(v000, v001, color, duration);
            Debug.DrawLine(v001, v101, color, duration);
            Debug.DrawLine(v101, v100, color, duration);
            Debug.DrawLine(v100, v000, color, duration);

            // top square
            Debug.DrawLine(v010, v011, color, duration);
            Debug.DrawLine(v011, v111, color, duration);
            Debug.DrawLine(v111, v110, color, duration);
            Debug.DrawLine(v110, v010, color, duration);

            // vertical lines
            Debug.DrawLine(v000, v010, color, duration);
            Debug.DrawLine(v001, v011, color, duration);
            Debug.DrawLine(v101, v111, color, duration);
            Debug.DrawLine(v100, v110, color, duration);
        }

        /// <summary>
        ///     Draws a 2D Bounds rectangle (on XY plane) using <see cref="Debug.DrawLine"/>.
        /// </summary>
        public static void DrawBounds2D(Bounds bounds, Color color = default, float duration = 0f)
        {
            if (color == default) color = Color.green;

            Vector3 c = bounds.center;
            Vector3 e = bounds.extents;

            // 4 corners in XY plane
            Vector3 v00 = c + new Vector3(-e.x, -e.y, 0);
            Vector3 v01 = c + new Vector3(-e.x, e.y, 0);
            Vector3 v11 = c + new Vector3(e.x, e.y, 0);
            Vector3 v10 = c + new Vector3(e.x, -e.y, 0);

            Debug.DrawLine(v00, v01, color, duration);
            Debug.DrawLine(v01, v11, color, duration);
            Debug.DrawLine(v11, v10, color, duration);
            Debug.DrawLine(v10, v00, color, duration);
        }
    }

}