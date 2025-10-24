using Asce.Game.SaveLoads;
using Asce.Game.Stats;
using Asce.Managers;
using Asce.SaveLoads;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class EntityStats : GameComponent, ISaveable<StatsSaveData>, ILoadable<StatsSaveData>
    {
        [SerializeField] protected List<StatContainer> _stats = new();

        [Space]
        [SerializeField] protected ResourceStat _health = new();
        [SerializeField] protected Stat _armor = new();
        [SerializeField] protected Stat _speed = new();

        public ResourceStat Health => _health;
        public Stat Armor => _armor;
        public Stat Speed => _speed;


        public virtual void Initialize(SO_EntityStats baseStats)
        {
            _stats.Add(new StatContainer(nameof(Health), Health));
            _stats.Add(new StatContainer(nameof(Armor), Armor));
            _stats.Add(new StatContainer(nameof(Speed), Speed));

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

        protected virtual void ClearStats(bool isClearBase = false)
        {
            Health.Clear(isClearBase);
            Armor.Clear(isClearBase);
            Speed.Clear(isClearBase);
        }

        StatsSaveData ISaveable<StatsSaveData>.Save()
        {
            StatsSaveData statsData = new();
            foreach(StatContainer container in _stats)
            {
                if (container.Stat is ISaveable<ResourceStatSaveData> resourceSaveable)
                {
                    ResourceStatSaveData resData = resourceSaveable.Save();
                    statsData.stats.Add(new(container.Name, resData));
                }
                else if (container.Stat is ISaveable<StatSaveData> saveable)
                {
                    statsData.stats.Add(new(container.Name, saveable.Save()));
                }
            }
            return statsData;
        }

        void ILoadable<StatsSaveData>.Load(StatsSaveData data)
        {
            if (data == null) return;
            ClearStats(isClearBase: true);
            foreach (StatContainerSaveData statData in data.stats)
            {
                StatContainer container = _stats.Find((c) => c.Name == statData.Name);
                if (container.Stat is ILoadable<ResourceStatSaveData> resLoadable)
                {
                    resLoadable.Load(statData.Stat as ResourceStatSaveData);
                }
                else if (container.Stat is ILoadable<StatSaveData> loadable)
                {
                    loadable.Load(statData.Stat);
                }
            }
        }
    }
}
