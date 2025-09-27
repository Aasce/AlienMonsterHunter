using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class Entity : GameComponent
    {
        [Header("Entity")]
        [SerializeField] protected SO_EntityInformation _information;
        [SerializeField] protected EntityStats _stats;

        public SO_EntityInformation Information => _information;
        public EntityStats Stats => _stats;


        protected virtual void Start()
        {
            if (Information == null) return;
            if (Stats != null) Stats.Initialize(Information.Stats);
        }

    }
}