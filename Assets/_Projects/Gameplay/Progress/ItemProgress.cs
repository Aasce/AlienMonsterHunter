using System;
using UnityEngine;

namespace Asce.Game.Progress
{
    [System.Serializable]
    public abstract class ItemProgress
    {
        [SerializeField] protected SO_ProgressInformation _information;
        [SerializeField] protected string _name = string.Empty;
        [SerializeField] protected bool _isUnlocked = false;

        public event Action OnUnlocked;

        public SO_ProgressInformation Information => _information;
        public string Name => _name;
        public bool IsUnlocked => _isUnlocked;
        

        public void Unlock(SO_UnlockCondition condition)
        {
            if (_isUnlocked) return;
            if (condition == null) return;
            if (!condition.IsMet()) return;

            _isUnlocked = true;
            condition.Met();
            this.SaveProgress();
            if (_isUnlocked) OnUnlocked?.Invoke();
        }

        public abstract void SaveProgress();
        public abstract void LoadProgress();

        public ItemProgress(string name, SO_ProgressInformation progress)
        {
            _name = name;
            _information = progress;
        }
    }
}
