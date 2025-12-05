using System.Collections.Generic;
using UnityEngine;

namespace Asce.Core.Utils
{
    /// <summary>
    /// Utility class providing 2D cone-based overlap detection.
    /// </summary>
    public static class Physic2DUtils
    {
        /// <summary> </summary>
        /// <returns> Returns all Collider2D inside a cone (center + radius + angle). </returns>
        public static Collider2D[] OverlapConeAll(
            Vector2 origin,
            float radius,
            Vector2 direction,
            float coneAngle,
            LayerMask mask)
        {
            Collider2D[] rawHits = Physics2D.OverlapCircleAll(origin, radius, mask);
            if (rawHits == null || rawHits.Length == 0)
                return System.Array.Empty<Collider2D>();

            List<Collider2D> filtered = new(rawHits.Length);

            float sqrRadius = radius * radius;
            float halfAngle = coneAngle * 0.5f;

            for (int i = 0; i < rawHits.Length; i++)
            {
                Collider2D hit = rawHits[i];
                if (hit == null) continue;

                Vector2 toTarget = (Vector2)hit.transform.position - origin;

                // Distance check
                if (toTarget.sqrMagnitude > sqrRadius)
                    continue;

                // Angle check
                float angle = Vector2.Angle(direction, toTarget);
                if (angle <= halfAngle)
                    filtered.Add(hit);
            }

            return filtered.ToArray();
        }

        /// <summary>
        ///     Writes colliders inside a cone into the given buffer.
        ///     No memory allocation.
        /// </summary>
        /// <returns> Returns the number of valid hits written. </returns>
        public static int OverlapConeNonAlloc(
            Vector2 origin,
            float radius,
            Vector2 direction,
            float coneAngle,
            ContactFilter2D contactFilter,
            Collider2D[] resultsBuffer)
        {
            int count = Physics2D.OverlapCircle(
                origin,
                radius,
                contactFilter,
                resultsBuffer
            );
            if (count == 0) return 0;

            int validCount = 0;
            float halfAngle = coneAngle * 0.5f;

            for (int i = 0; i < count; i++)
            {
                Collider2D hit = resultsBuffer[i];
                if (hit == null) continue;

                if (Vector2.Distance((Vector2)hit.transform.position, origin) > radius) continue;

                Vector2 toTarget = (Vector2)hit.transform.position - origin;
                float angle = Vector2.Angle(direction.normalized, toTarget);
                if (angle > halfAngle) continue;

                resultsBuffer[validCount] = hit;
                validCount++;
            }

            return validCount;
        }

    }
}
