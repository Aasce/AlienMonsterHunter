using Asce.Game.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Progress
{
    [System.Serializable]
    public class ResultShareData
    {
        [SerializeField] private GameResultType _finalResult = GameResultType.Unknown;
        [SerializeField] private float _elapsedTime = 0f;

        [Space]
        [SerializeField] private int _startCharacterLevel = 0;
        [SerializeField] private int _startCharacterExp = 0;

        [SerializeField] private int _endCharacterLevel = 0;
        [SerializeField] private int _endCharacterExp = 0;

        [Space]
        [SerializeField] private Dictionary<string, int> _spoils = new();

        public GameResultType FinalResult
        {
            get => _finalResult;
            set => _finalResult = value;
        }

        public float ElapsedTime
        {
            get => _elapsedTime;
            set => _elapsedTime = value;
        }

        public int StartCharacterLevel
        {
            get => _startCharacterLevel;
            set => _startCharacterLevel = value;
        }

        public int StartCharacterExp
        {
            get => _startCharacterExp;
            set => _startCharacterExp = value;
        }

        public int EndCharacterLevel
        {
            get => _endCharacterLevel;
            set => _endCharacterLevel = value;
        }

        public int EndCharacterExp
        {
            get => _endCharacterExp;
            set => _endCharacterExp = value;
        }

        public Dictionary<string, int> Spoils => _spoils;
    }
}
