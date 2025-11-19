using System;
using UnityEngine;

namespace Asce.Game.Players
{
    [System.Serializable]
    public abstract class PlayerItemProgress
    {
        [SerializeField] protected string _name = string.Empty;
        [SerializeField] protected bool _isUnlocked = false;

        public event Action OnUnlocked;

        public string Name => _name;
        public bool IsUnlocked
        {
            get => _isUnlocked;
            set
            {
                if (_isUnlocked == value) return;
                _isUnlocked = value;
                if (_isUnlocked) OnUnlocked?.Invoke();
            }
        }

        public PlayerItemProgress(string name)
        {
            _name = name;
        }

    }
}
