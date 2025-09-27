using Asce.Game.Stats;
using Asce.Managers;
using System;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class EntityStats : GameComponent
    {
        [SerializeField] protected ResourceStat _health = new();
        [SerializeField] protected Stat _armor = new();
        [SerializeField] protected Stat _speed = new();

        public ResourceStat Health => _health;
        public Stat Armor => _armor;
        public Stat Speed => _speed;


        public virtual void Initialize(SO_EntityStats baseStats)
        {
            if (baseStats == null) return;
            Health.Add(baseStats.MaxHealth, StatValueType.Base);
            Armor.Add(baseStats.Armor, StatValueType.Base);
            Speed.Add(baseStats.Speed, StatValueType.Base);
        }

        public virtual void ResetStats()
        {
            this.ClearStats();
            Health.ToFull();
        }
        protected virtual void ClearStats()
        {
            Health.Clear();
            Armor.Clear();
            Speed.Clear();
        }
    }
}
