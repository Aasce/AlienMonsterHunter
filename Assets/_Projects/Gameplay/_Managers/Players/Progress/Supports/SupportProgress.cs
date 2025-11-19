using Asce.Game.Supports;
using Asce.Game.Managers;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Players
{
    [System.Serializable]
    public class SupportProgress : PlayerItemProgress, ISaveable<SupportProgressSaveData>, ILoadable<SupportProgressSaveData>
    {
        [SerializeField] private int _level = 0;

        public event Action<int> OnLevelChanged;

        public int Level
        {
            get => _level;
            set
            {
                int newLevel = Mathf.Clamp(value, 0, MaxLevel);
                if (_level == newLevel) return;
                _level = newLevel;
                OnLevelChanged?.Invoke(_level);
            }
        }

        public int MaxLevel
        {
            get
            {
                Support gunPrefab = GameManager.Instance.AllSupports.Get(_name);
                if (gunPrefab == null) return 0;
                return gunPrefab.Information.Leveling.MaxLevel;
            }
        }

        public bool IsMaxLevel => _level >= MaxLevel;

        public SupportProgress(string name) : base(name) { }

        SupportProgressSaveData ISaveable<SupportProgressSaveData>.Save()
        {
            return new SupportProgressSaveData
            {
                name = _name,
                isUnlocked = _isUnlocked,
                level = _level,
            };
        }

        void ILoadable<SupportProgressSaveData>.Load(SupportProgressSaveData data)
        {
            if (data == null) return;
            _isUnlocked = data.isUnlocked;
            _level = data.level;
        }
    }
}
