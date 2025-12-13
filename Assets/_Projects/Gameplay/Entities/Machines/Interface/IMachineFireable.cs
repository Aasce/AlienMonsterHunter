using System;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public interface IMachineFireable
    {
        public event Action<Vector2, Vector2> OnFired;

        public void Fire(Vector2 position, Vector2 direction);
    }
}
