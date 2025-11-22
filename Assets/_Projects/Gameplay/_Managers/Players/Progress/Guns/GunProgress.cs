using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.Game.Progress;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Players
{
    [System.Serializable]
    public class GunProgress : ItemProgress, ISaveable<GunProgressSaveData>, ILoadable<GunProgressSaveData>
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
                this.SaveProgress();
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

        public GunProgress(string name, SO_ProgressInformation progress) : base(name, progress) { }

        public override void SaveProgress()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.SaveGunProgress(this);
        }

        public override void LoadProgress()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.LoadGunProgress(this);
        }

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
