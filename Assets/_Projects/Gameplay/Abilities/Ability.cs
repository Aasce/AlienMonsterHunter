using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public abstract class Ability : GameComponent
    {
        [SerializeField] protected SO_AbilityInformation _information;
        [SerializeField] protected GameObject _owner;
        [SerializeField] protected Cooldown _despawnTime = new (10f);

        public SO_AbilityInformation Information => _information;
        public GameObject Owner
        {
            get => _owner;
            set => _owner = value;
        }
        public Cooldown DespawnTime => _despawnTime;
        protected virtual void Start()
        {
            DespawnTime.SetBaseTime(Information.DaspawnTime);
        }

        public virtual void OnSpawn()
        {

        }

        public virtual void OnDespawn()
        {

        }
    }
}