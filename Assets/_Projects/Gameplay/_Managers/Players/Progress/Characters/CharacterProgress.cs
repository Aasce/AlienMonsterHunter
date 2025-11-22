using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.Game.Progress;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Players
{
    [System.Serializable]
    public class CharacterProgress : ItemProgress, ISaveable<CharacterProgressSaveData>, ILoadable<CharacterProgressSaveData>
    {
        [SerializeField] private int _level = 0;
        [SerializeField] private int _exp = 0;

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

        public int Exp
        {
            get => _exp;
            set => _exp = value;
        }

        public int MaxLevel
        {
            get
            {
                Character characterPrefab = GameManager.Instance.AllCharacters.Get(_name);
                if (characterPrefab == null) return 0;
                return characterPrefab.Information.Leveling.MaxLevel;
            }
        }

        public bool IsMaxLevel => _level >= MaxLevel;

        public CharacterProgress(string name, SO_ProgressInformation progress) : base(name, progress) { }

        public void SetLevel(int currentLevel, int exp)
        {
            _level = currentLevel;
            _exp = exp;
            this.SaveProgress();
        }

        public int ExpToLevelUp(int currentLevel)
        {
            Character characterPrefab = GameManager.Instance.AllCharacters.Get(_name);
            if (characterPrefab == null) return 0;

            return characterPrefab.Information.Leveling.ExpToLevelUp(currentLevel);
        }

        public override void SaveProgress()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.SaveCharacterProgress(this);
        }

        public override void LoadProgress()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.LoadCharacterProgress(this);
        }

        CharacterProgressSaveData ISaveable<CharacterProgressSaveData>.Save()
        {
            return new CharacterProgressSaveData
            {
                name = _name,
                isUnlocked = _isUnlocked,
                level = _level,
                exp = _exp
            };
        }

        void ILoadable<CharacterProgressSaveData>.Load(CharacterProgressSaveData data)
        {
            if (data == null) return;
            _isUnlocked = data.isUnlocked;
            _level = data.level;
            _exp = data.exp;
        }
    }
}