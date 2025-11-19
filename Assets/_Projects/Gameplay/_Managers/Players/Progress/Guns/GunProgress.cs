using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Players
{
    [System.Serializable]
    public class GunProgress : PlayerItemProgress, ISaveable<GunProgressSaveData>, ILoadable<GunProgressSaveData>
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
                Gun gunPrefab = GameManager.Instance.AllGuns.Get(_name);
                if (gunPrefab == null) return 0;
                return gunPrefab.Information.Leveling.MaxLevel;
            }
        }

        public bool IsMaxLevel => _level >= MaxLevel;

        public GunProgress(string name) : base(name) { }

        GunProgressSaveData ISaveable<GunProgressSaveData>.Save()
        {
            return new GunProgressSaveData
            {
                name = _name,
                isUnlocked = _isUnlocked,
                level = _level,
            };
        }

        void ILoadable<GunProgressSaveData>.Load(GunProgressSaveData data)
        {
            if (data == null) return;
            _isUnlocked = data.isUnlocked;
            _level = data.level;
        }
    }
}
