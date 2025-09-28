using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public abstract class Ability : GameComponent
    {
        [SerializeField] private string _name;
        [SerializeField] private Cooldown _despawnTime = new (10f);

        public string Name => _name;
        public Cooldown DespawnTime => _despawnTime;

        public virtual void OnSpawn()
        {

        }

        public virtual void OnDespawn()
        {

        }
    }
}