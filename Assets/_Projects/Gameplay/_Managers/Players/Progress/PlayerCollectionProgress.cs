using Asce.Managers;
using Asce.SaveLoads;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Players
{
    public abstract class PlayerCollectionProgress<T, TProgress> : GameComponent where TProgress : PlayerItemProgress
    {
        [SerializeField] protected List<TProgress> _itemProgress = new();
        protected Dictionary<string, TProgress> _itemsProgressDictionary;
        protected ReadOnlyCollection<TProgress> _itemsProgressReadonly;

        protected abstract IEnumerable<T> Collection { get; }
        public ReadOnlyCollection<TProgress> AllProgresses => _itemsProgressReadonly ??= _itemProgress.AsReadOnly();

        protected abstract TProgress CreateProgressInstance(string name);
        protected abstract string GetInformationName(T item);
        protected abstract void SaveProgress(TProgress itemProgress);
        protected abstract void LoadProgress(TProgress itemProgress);

        public virtual void Initialize()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            foreach (T item in Collection)
            {
                TProgress itemProgress = CreateProgressInstance(GetInformationName(item));
                this.LoadProgress(itemProgress);
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

        public virtual void Unlock(string name)
        {
            TProgress itemProgress = Get(name);
            if (itemProgress == null) return;
            if (itemProgress.IsUnlocked) return;

            itemProgress.IsUnlocked = true;
            this.SaveProgress(itemProgress);
        }
    }
}
