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

        /// <summary>
        ///     Draws a rectangle in the scene view using Debug.DrawLine.
        /// </summary>
        /// <param name="rect">The rectangle to draw.</param>
        /// <param name="color">The color of the rectangle lines.</param>
        /// <param name="duration">How long the lines should remain visible (in seconds).</param>
        public static void DrawRect(Rect rect, Color color, float duration = 0f)
        {
            Vector3 topLeft = new Vector3(rect.xMin, rect.yMax, 0f);
            Vector3 topRight = new Vector3(rect.xMax, rect.yMax, 0f);
            Vector3 bottomLeft = new Vector3(rect.xMin, rect.yMin, 0f);
            Vector3 bottomRight = new Vector3(rect.xMax, rect.yMin, 0f);

            Debug.DrawLine(topLeft, topRight, color, duration);
            Debug.DrawLine(topRight, bottomRight, color, duration);
            Debug.DrawLine(bottomRight, bottomLeft, color, duration);
            Debug.DrawLine(bottomLeft, topLeft, color, duration);
        }

        /// <summary>
        ///     Draws a rectangle centered at a position with a given size.
        /// </summary>
        /// <param name="center">Center of the rectangle.</param>
        /// <param name="size">Width and height of the rectangle.</param>
        /// <param name="color">Color of the lines.</param>
        /// <param name="duration">Duration for which the lines will be visible.</param>
        public static void DrawRect(Vector2 center, Vector2 size, Color color, float duration = 0f)
        {
            Rect rect = new Rect(center - size * 0.5f, size);
            DrawRect(rect, color, duration);
        }

        /// <summary>
        ///     Draws a circle using Debug.DrawLine.
        /// </summary>
        /// <param name="center">The center of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="color">The color of the circle line.</param>
        /// <param name="duration">How long the lines should remain visible (in seconds).</param>
        /// <param name="segmentCount">Number of line segments used to approximate the circle (default 16).</param>
        public static void DrawCircle(Vector2 center, float radius, Color color, float duration = 0f, int segmentCount = 16)
        {
            if (segmentCount < 4) segmentCount = 4;

            float angleStep = 360f / segmentCount;
            Vector3 previousPoint = center + new Vector2(Mathf.Cos(0f), Mathf.Sin(0f)) * radius;

            for (int i = 1; i <= segmentCount; i++)
            {
                float angle = angleStep * i * Mathf.Deg2Rad;
                Vector3 nextPoint = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                Debug.DrawLine(previousPoint, nextPoint, color, duration);
                previousPoint = nextPoint;
            }
        }
    }

}