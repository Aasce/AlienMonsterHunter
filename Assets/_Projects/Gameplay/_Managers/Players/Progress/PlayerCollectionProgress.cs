using Asce.Game.Progress;
using Asce.Core;
using Asce.SaveLoads;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Players
{
    public abstract class PlayerCollectionProgress<T, TProgress> : GameComponent where TProgress : ItemProgress
    {
        [SerializeField] protected List<TProgress> _itemProgress = new();
        protected Dictionary<string, TProgress> _itemsProgressDictionary;
        protected ReadOnlyCollection<TProgress> _itemsProgressReadonly;

        protected abstract IEnumerable<T> Collection { get; }
        public ReadOnlyCollection<TProgress> AllProgresses => _itemsProgressReadonly ??= _itemProgress.AsReadOnly();

        protected abstract TProgress CreateProgressInstance(T item);
        protected abstract string GetInformationName(T item);

        public virtual void Initialize()
        {
            foreach (T item in Collection)
            {
                TProgress itemProgress = CreateProgressInstance(item);
                itemProgress.LoadProgress();
                _itemProgress.Add(itemProgress);
            }

            _itemsProgressDictionary = new Dictionary<string, TProgress>();
            foreach (var itemProgress in _itemProgress)
            {
                _itemsProgressDictionary[itemProgress.Name] = itemProgress;
            }
        }

        public TProgress Get(string name)
        {
            if (_itemsProgressDictionary == null) this.Initialize();
            if (_itemsProgressDictionary.TryGetValue(name, out TProgress progress))
            {
                return progress;
            }
            return null;
        }
    }
}
