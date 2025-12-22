using Asce.Core.UIs;
using Asce.Core.Utils;
using Asce.Game.Managers;
using Asce.Game.Maps;
using Asce.Game.Progress;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.PrepareGame.UIs.HUDs
{
    public class UIEnemiesLevel : UIComponent
    {
        [SerializeField] private Pool<UIEnemyLevelItem> _pool = new();


        private void Start()
        {
            if (!GameManager.Instance.Shared.TryGet("MapLevel", out PickMapLevelShareData mapLevelData))
            {
                Debug.LogError("[NewGameController] Map Level Share Data is null", this);
            }

            Map mapPrefab = GameManager.Instance.AllMaps.Get(mapLevelData.MapName);
            SO_MapLevelInformation levelInformation = mapPrefab.Information.GetLevel(mapLevelData.Level);

            this.SetEnemies(levelInformation.Enemies);
        }

        private void SetEnemies(IEnumerable<MapLevelEnemy> enemies)
        {
            foreach (MapLevelEnemy enemy in enemies)
            {
                UIEnemyLevelItem item = _pool.Activate();
                item.Set(enemy);
                item.RectTransform.SetAsLastSibling();
                item.Show();
            }
        }
    }
}
