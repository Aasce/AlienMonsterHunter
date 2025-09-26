using UnityEngine;

namespace Asce.Game.Entities.FOVs
{
    public struct ViewCastInfo
    {
        public bool isHit;
        public Vector2 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool isHit = false, Vector2 point = default, float distance = 0, float angle = 0)
        {
            this.isHit = isHit;
            this.point = point;
            this.distance = distance;
            this.angle = angle;
        }
    }
}