using Asce.Core;
using Asce.Game.Managers;
using Asce.Game.Maps;
using Asce.Game.Progress;
using Asce.MainMenu.SaveLoads;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.MainMenu.Picks
{
    public class PickController : MonoBehaviourSingleton<PickController>, ISaveable<LastMapLevelPickSaveData>, ILoadable<LastMapLevelPickSaveData>
    {
        [SerializeField] private Map _mapPrefab;
        [SerializeField] private int _level;

        public Map MapPrefab => _mapPrefab;
        public int Level => _level;


        public void PickMap(Map map, int level = 1)
        {
            if(_mapPrefab != map) _mapPrefab = map;
            _level = level;
        }

        public PickMapLevelShareData CreateLevelShareData()
        {
            PickMapLevelShareData levelShareData = new();

            if (_mapPrefab != null)
                levelShareData.MapName = _mapPrefab.Information.Name;

            if (_mapPrefab.Information.IsLevelValid(_level))
                levelShareData.Level = _level;

            return levelShareData;
        }


        LastMapLevelPickSaveData ISaveable<LastMapLevelPickSaveData>.Save()
        {
            LastMapLevelPickSaveData lastPickData = new()
            {
                mapName = _mapPrefab != null ? _mapPrefab.Information.Name : string.Empty,
                level = _level
            };

            return lastPickData;
        }

        void ILoadable<LastMapLevelPickSaveData>.Load(LastMapLevelPickSaveData data)
        {
            if (data == null) return;
            _mapPrefab = GameManager.Instance.AllMaps.Get(data.mapName);
            _level = data.level;
        }
    }
}