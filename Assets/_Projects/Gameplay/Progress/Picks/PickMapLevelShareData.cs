using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Progress
{
    [System.Serializable]
    public class PickMapLevelShareData
    {
        [SerializeField] private string _mapName;
        [SerializeField] private int _level;

        public string MapName
        {
            get => _mapName;
            set => _mapName = value;
        }
        public int Level
        {
            get => _level;
            set => _level = value;
        }

    }
}