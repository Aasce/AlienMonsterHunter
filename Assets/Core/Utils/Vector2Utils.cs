using System.Collections.Generic;
using UnityEngine;

namespace Asce.Managers.Utils
{
    public static class Vector2Utils
    {
        /// <summary>
        ///     Rotates a 2D vector by a specified angle in degrees and returns the normalized result.
        /// </summary>
        /// <param name="vector"> The vector to rotate. </param>
        /// <param name="angleInDegrees"> The angle to rotate, in degrees. </param>
        /// <returns> The normalized rotated vector. </returns>
        public static Vector2 RotateVector(Vector2 vector, float angleInDegrees)
        {
            // Convert angle from degrees to radians
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

            // Calculate sine and cosine of the angle
            float sin = Mathf.Sin(angleInRadians);
            float cos = Mathf.Cos(angleInRadians);

            // Apply the 2D rotation matrix
            float rotatedX = (cos * vector.x) - (sin * vector.y);
            float rotatedY = (sin * vector.x) + (cos * vector.y);

            // Return the rotated vector, normalized
            return new Vector2(rotatedX, rotatedY).normalized;
        }


        /// <summary>
        ///     Checks if a given point lies within a circle defined by its center and radius.
        /// </summary>
        /// <param name="center"> The center position of the circle. </param>
        /// <param name="radius"> The radius of the circle. </param>
        /// <param name="point"> The point to test for inclusion within the circle. </param>
        /// <returns>
        ///     True if the point is inside the circle, false otherwise. 
        /// </returns>
        public static bool IsPointInsideCircle(Vector2 center, float radius, Vector2 point)
        {
            return Vector2.Distance(point, center) < radius;
        }

        /// <summary>
        ///     Gets the average (centroid) of a list of Vector2 points.
        /// </summary>
        /// <param name="vectors"> List of Vector2 points to calculate the average from. </param>
        /// <returns>
        ///     The average Vector2 of the list, or Vector2.zero if the list is null or empty.
        /// </returns>
        public static Vector2 GetAverage(List<Vector2> vectors)
        {
            if (vectors == null || vectors.Count == 0) return Vector2.zero;

            Vector2 result = Vector2.zero;
            foreach (Vector2 vector in vectors)
            {
                result += vector;
            }
            return result / vectors.Count;
        }
    }
}
