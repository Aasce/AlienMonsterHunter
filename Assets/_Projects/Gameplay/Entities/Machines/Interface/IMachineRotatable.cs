using System;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public interface IMachineRotatable
    {
        public float Angle { get; }
        public event Action<float> OnRotated;

        public void Rotate(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            this.Rotate(angle);
        }

        public void Rotate(float angle);
    }
}
