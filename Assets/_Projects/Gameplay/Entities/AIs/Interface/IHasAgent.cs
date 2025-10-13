using UnityEngine;
using UnityEngine.AI;

namespace Asce.Game.Entities
{
    public interface IHasAgent
    {
        public NavMeshAgent Agent { get; }
    }
}