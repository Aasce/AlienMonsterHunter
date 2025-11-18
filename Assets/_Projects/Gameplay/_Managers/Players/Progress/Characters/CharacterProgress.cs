using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.SaveLoads;
using System;
using UnityEngine;

namespace Asce.Game.Players
{
    [System.Serializable]
    public class CharacterProgress : ISaveable<CharacterProgressSaveData>, ILoadable<CharacterProgressSaveData>
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField] private bool _isUnlocked = false;
        [SerializeField] private int _level = 0;
        [SerializeField] private int _exp = 0;

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

        public int Level
        {
            get => _level;
            set => _level = value;
        }

        public int Exp
        {
            get => _exp;
            set => _exp = value;
        }

        public CharacterProgress(string name) 
        {
            _name = name;
        }

        public int ExpToLevelUp(int currentLevel)
        {
            Character characterPrefab = GameManager.Instance.AllCharacters.Get(_name);
            if (characterPrefab == null) return 0;

            return characterPrefab.Information.Leveling.ExpToLevelUp(currentLevel);
        }

        public bool IsMaxLevel()
        {
            Character characterPrefab = GameManager.Instance.AllCharacters.Get(_name);
            if (characterPrefab == null) return true;
            return _level >= characterPrefab.Information.Leveling.MaxLevel;
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