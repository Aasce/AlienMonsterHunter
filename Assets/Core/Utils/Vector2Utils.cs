using System.Collections.Generic;
using UnityEngine;

namespace Asce.Core.Utils
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

        /// <summary>
        ///     Checks whether a ray (defined by origin and direction) intersects with a line segment
        ///     (defined by startPoint and endPoint).
        /// </summary>
        /// <param name="origin">The starting point of the ray.</param>
        /// <param name="direction">The direction of the ray (normalized internally).</param>
        /// <param name="startPoint">The start point of the line segment.</param>
        /// <param name="endPoint">The end point of the line segment.</param>
        /// <param name="intersectionPoint">Output intersection point if it exists.</param>
        /// <returns>True if the ray intersects the segment; otherwise false.</returns>
        public static bool SegmentIntersection(
            Vector2 origin,
            Vector2 direction,
            Vector2 startPoint,
            Vector2 endPoint,
            out Vector2 intersectionPoint)
        {
            intersectionPoint = Vector2.zero;
            direction.Normalize();

            // Vector from segment start to ray origin
            Vector2 originToStart = origin - startPoint;

            // Vector representing the segment
            Vector2 segmentVector = endPoint - startPoint;

            // Perpendicular vector to rayDirection
            Vector2 perpendicularToRay = new Vector2(-direction.y, direction.x);

            float dot = Vector2.Dot(segmentVector, perpendicularToRay);
            if (Mathf.Abs(dot) < Mathf.Epsilon)
                return false; // The ray and segment are parallel (or almost parallel)

            float rayDistanceFactor = Cross(segmentVector, originToStart) / dot;
            float segmentFactor = Vector2.Dot(originToStart, perpendicularToRay) / dot;

            // rayDistanceFactor >= 0, point is in front of the ray origin
            // 0 <= segmentFactor <= 1, intersection lies on the segment
            if (rayDistanceFactor >= 0f && segmentFactor >= 0f && segmentFactor <= 1f)
            {
                intersectionPoint = origin + direction * rayDistanceFactor;
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Checks whether a ray (origin + direction) intersects with a Rect, 
        ///     and returns the first intersection point (closest to origin).
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized ray direction.</param>
        /// <param name="rect">The rectangle to test against.</param>
        /// <param name="intersectionPoint">The first intersection point (closest to ray origin).</param>
        /// <returns>True if the ray hits the rect; otherwise false.</returns>
        public static bool RectIntersection(
            Vector2 origin,
            Vector2 direction,
            Rect rect,
            out Vector2 intersectionPoint)
        {
            intersectionPoint = Vector2.zero;
            direction.Normalize();

            // Get rectangle corners
            Vector2 bottomLeft = new Vector2(rect.xMin, rect.yMin);
            Vector2 bottomRight = new Vector2(rect.xMax, rect.yMin);
            Vector2 topRight = new Vector2(rect.xMax, rect.yMax);
            Vector2 topLeft = new Vector2(rect.xMin, rect.yMax);

            // Define edges
            Vector2[] edgeStarts = { bottomLeft, bottomRight, topRight, topLeft };
            Vector2[] edgeEnds = { bottomRight, topRight, topLeft, bottomLeft };

            bool hasIntersection = false;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < 4; i++)
            {
                if (SegmentIntersection(origin, direction, edgeStarts[i], edgeEnds[i], out Vector2 edgeHit))
                {
                    float distance = Vector2.Distance(origin, edgeHit);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        intersectionPoint = edgeHit;
                        hasIntersection = true;
                    }
                }
            }

            return hasIntersection;
        }

        /// <summary>
        ///     Checks whether a ray (origin + direction) intersects with a 2D Bounds,
        ///     and returns the first intersection point (closest to origin).
        /// </summary>
        /// <param name="origin">The origin of the ray.</param>
        /// <param name="direction">The normalized ray direction.</param>
        /// <param name="bounds">The 2D bounds to test against.</param>
        /// <param name="intersectionPoint">The first intersection point (closest to ray origin).</param>
        /// <returns>True if the ray hits the bounds; otherwise false.</returns>
        public static bool BoundsIntersection(
            Vector2 origin,
            Vector2 direction,
            Bounds bounds,
            out Vector2 intersectionPoint)
        {
            intersectionPoint = Vector2.zero;
            direction.Normalize();

            // Get 2D corners of bounds
            Vector2 min = bounds.min;
            Vector2 max = bounds.max;

            Vector2 bottomLeft = new Vector2(min.x, min.y);
            Vector2 bottomRight = new Vector2(max.x, min.y);
            Vector2 topRight = new Vector2(max.x, max.y);
            Vector2 topLeft = new Vector2(min.x, max.y);

            // Define edges
            Vector2[] edgeStarts = { bottomLeft, bottomRight, topRight, topLeft };
            Vector2[] edgeEnds = { bottomRight, topRight, topLeft, bottomLeft };

            bool hasIntersection = false;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < 4; i++)
            {
                if (SegmentIntersection(origin, direction, edgeStarts[i], edgeEnds[i], out Vector2 edgeHit))
                {
                    float distance = Vector2.Distance(origin, edgeHit);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        intersectionPoint = edgeHit;
                        hasIntersection = true;
                    }
                }
            }

            return hasIntersection;
        }


        /// <summary>
        ///     2D cross product (returns scalar value).
        /// </summary>
        private static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }
    }
}
