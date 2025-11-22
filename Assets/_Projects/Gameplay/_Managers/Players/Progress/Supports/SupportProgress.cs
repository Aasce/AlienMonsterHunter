using Asce.Game.Supports;
using Asce.Game.Managers;
using Asce.SaveLoads;
using System;
using UnityEngine;
using Asce.Game.Progress;

namespace Asce.Game.Players
{
    [System.Serializable]
    public class SupportProgress : ItemProgress, ISaveable<SupportProgressSaveData>, ILoadable<SupportProgressSaveData>
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
                Support gunPrefab = GameManager.Instance.AllSupports.Get(_name);
                if (gunPrefab == null) return 0;
                return gunPrefab.Information.Leveling.MaxLevel;
            }
        }

        public bool IsMaxLevel => _level >= MaxLevel;

        public SupportProgress(string name, SO_ProgressInformation progress) : base(name, progress) { }


        public override void SaveProgress()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.SaveSupportProgress(this);
        }

        public override void LoadProgress()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.LoadSupportProgress(this);
        }

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
