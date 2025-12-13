using System;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    public interface IMachineAttackable
    {
        public event Action<ITargetable> OnAttacked;

        public void Attack(ITargetable target);
    }
}
